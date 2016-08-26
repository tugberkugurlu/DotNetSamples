using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Razor;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace RazorTemplatingSample.Web
{
    public static class Templater
    {
        private const string RootNamespace = "RazorOnConsole";
        private const string MainClassNamePrefix = "ASPV_";

        public static string Run(IEnumerable<Person> people)
        {
            if (people == null)
            {
                throw new ArgumentNullException("people");
            }

            const string className = RootNamespace + "." + MainClassNamePrefix + "Index";
            var razorGeneratedCode = GetRazorSyntaxTree();
            Assembly assembly = Compile(razorGeneratedCode);

            return GenerateHtml(assembly, className, people);
        }

        private static string GenerateHtml(Assembly assembly, string className, IEnumerable<Person> model)
        {
            Type type = assembly.GetType(className);
            var modelProperty = type.GetProperty("Model");
            object obj = Activator.CreateInstance(type);
            modelProperty.SetValue(obj, model);

            using (var ms = new MemoryStream())
            using (var reader = new StreamReader(ms))
            {
                var task = (Task)type.GetTypeInfo().GetMethods()
                    .First(dm => dm.Name == "ExecuteAsync" && dm.GetParameters().Any())
                    .Invoke(obj, new object[] {ms});

                task.Wait();

                ms.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
        }

        private static Assembly Compile(string razorGeneratedCode)
        {
            var razorSyntaxTree = CSharpSyntaxTree.ParseText(razorGeneratedCode);
            var assemblyName = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(Templater).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { razorSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(IsError);
                    throw new InvalidOperationException();
                }

                ms.Seek(0, SeekOrigin.Begin);
                return Assembly.Load(ms.ToArray());
            }
        }

        private static string GetRazorSyntaxTree()
        {
            var viewPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Views\Index.cshtml");
            var fileName = Path.GetFileName(viewPath);
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            var className = MainClassNamePrefix + fileNameNoExtension;

            var codeLang = new CSharpRazorCodeLanguage();
            var host = new RazorEngineHost(codeLang)
            {
                DefaultBaseClass = typeof(BaseView).FullName,
                GeneratedClassContext = new GeneratedClassContext(
                    executeMethodName: GeneratedClassContext.DefaultExecuteMethodName,
                    writeMethodName: GeneratedClassContext.DefaultWriteMethodName,
                    writeLiteralMethodName: GeneratedClassContext.DefaultWriteLiteralMethodName,
                    writeToMethodName: "WriteTo",
                    writeLiteralToMethodName: "WriteLiteralTo",
                    templateTypeName: "HelperResult",
                    defineSectionMethodName: "DefineSection",
                    generatedTagHelperContext: new GeneratedTagHelperContext())
            };

            host.NamespaceImports.Add("System");

            var engine = new RazorTemplateEngine(host);

            using (var fileStream = File.OpenText(viewPath))
            {
                GeneratorResults code = engine.GenerateCode(
                    input: fileStream,
                    className: className,
                    rootNamespace: RootNamespace,
                    sourceFileName: fileName);

                return code.GeneratedCode;
            }
        }

        private static bool IsError(Diagnostic diagnostic)
        {
            return diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error;
        }
    }
}
