using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNet.Razor;
using Microsoft.AspNet.Razor.Generator;

namespace RazorOnConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string rootNamespace = "RazorOnConsole";
            var viewPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Views\Index.cshtml");
            var basePath = Path.GetDirectoryName(viewPath);
            var fileName = Path.GetFileName(viewPath);
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            using (var file = File.Create(fileNameNoExtension + ".html")) { new Index { Model = "foobarfoo" }.ExecuteAsync(file).Wait(); }

            var codeLang = new CSharpRazorCodeLanguage();
            var host = new RazorEngineHost(codeLang)
            {
                DefaultBaseClass = "RazorOnConsole.Views.BaseView",
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
                    className: fileNameNoExtension,
                    rootNamespace: rootNamespace,
                    sourceFileName: fileName);

                string source = code.GeneratedCode;
                File.WriteAllText(Path.Combine(basePath, @"..\..\..\", "Views", string.Format("{0}.cs", fileNameNoExtension)), source);
            }
        }
    }
}
