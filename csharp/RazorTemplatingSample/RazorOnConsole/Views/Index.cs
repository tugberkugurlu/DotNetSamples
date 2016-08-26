namespace RazorOnConsole
{
#line 2 "Index.cshtml"
using System.Linq

#line default
#line hidden
    ;
    using System;
    using System.Threading.Tasks;

    public class Index : RazorOnConsole.Views.BaseView<string>
    {
        #line hidden
        public Index()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#line 4 "Index.cshtml"
  
    var foo = "bar" + "foo";
    var link = "http://tugberkugurlu.com";

#line default
#line hidden

            WriteLiteral("\r\n\r\n<a");
            WriteAttribute("href", Tuple.Create(" href=\"", 156), Tuple.Create("\"", 168), 
            Tuple.Create(Tuple.Create("", 163), Tuple.Create<System.Object, System.Int32>(link, 163), false));
            WriteLiteral(">");
#line 9 "Index.cshtml"
           Write(foo);

#line default
#line hidden
            WriteLiteral("</a>\r\n\r\n");
#line 11 "Index.cshtml"
 foreach (var i in Enumerable.Range(0, 10))
{

#line default
#line hidden

            WriteLiteral("     <a href=\"#");
#line 13 "Index.cshtml"
             Write(i);

#line default
#line hidden
            WriteLiteral("\">");
#line 13 "Index.cshtml"
                   Write(i);

#line default
#line hidden
            WriteLiteral("</a>\r\n");
#line 14 "Index.cshtml"
}

#line default
#line hidden

            WriteLiteral("\r\n<p>This is model: ");
#line 16 "Index.cshtml"
             Write(Model);

#line default
#line hidden
            WriteLiteral("..</p>");
        }
        #pragma warning restore 1998
    }
}
