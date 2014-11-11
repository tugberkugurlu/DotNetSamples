using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace IdvSrvSample.Mvc.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult About()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        [ResourceAuthorize("Read", "ContactDetails")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }

    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            switch (context.Resource.First().Value)
            {
            case "ContactDetails":
                return AuthorizeContactDetails(context);
            default:
                return Nok();
            }
        }

        private Task<bool> AuthorizeContactDetails(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
            case "Read":
                return Eval(context.Principal.HasClaim("role", "Geek"));
            case "Write":
                return Eval(context.Principal.HasClaim("role", "Operator"));
            default:
                return Nok();
            }
        }
    }
}