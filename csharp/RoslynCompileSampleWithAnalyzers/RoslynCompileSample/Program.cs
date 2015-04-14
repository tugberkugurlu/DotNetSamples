using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynCompileSample
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            var prefix = GetMessagePrefix();
                            Console.WriteLine(prefix + ""-"" + message);
                        }

                        public string GetMessagePrefix()
                        {
                            return ""pre"";
                        }
                    }
                }");

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            string analyzerAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\lib\DotNetDoodle.Analyzers.dll");
            ImmutableArray<DiagnosticAnalyzer> diagnosticAnalyzers = new AnalyzerFileReference(analyzerAssemblyPath).GetAnalyzers(LanguageNames.CSharp);

            CompilationWithAnalyzers compilationWithAnalyzers = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)).WithAnalyzers(diagnosticAnalyzers);

            ImmutableArray<Diagnostic> diagsnostics = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilationWithAnalyzers.Compilation.Emit(ms);
                ImmutableArray<Diagnostic> allDiagsnostics = result.Diagnostics.Concat(diagsnostics).ToImmutableArray();

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = allDiagsnostics.Where(diagnostic => 
                        diagnostic.IsWarningAsError || 
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("ERROR: {0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    WriteWarnings(allDiagsnostics);
                }
                else
                {
                    WriteWarnings(allDiagsnostics);

                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("RoslynCompileSample.Writer");
                    object obj = Activator.CreateInstance(type);
                    type.InvokeMember("Write",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { "Hello World" });
                }
            }

            Console.ReadLine();
        }

        private static void WriteWarnings(ImmutableArray<Diagnostic> diagnostics)
        {
            IEnumerable<Diagnostic> warnings = diagnostics.Where(diagnostic =>
                diagnostic.Severity == DiagnosticSeverity.Warning);

            foreach (Diagnostic diagnostic in warnings)
            {
                Console.Error.WriteLine("WARNING: {0}: {1}", diagnostic.Id, diagnostic.GetMessage());
            }
        }
    }
}