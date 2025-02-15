using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public ClaimsPrincipal CurrentUser
        {
            get
            {
                if (HttpContext.User is not null && HttpContext.User.Identity.IsAuthenticated)
                    return HttpContext.User;
                else
                    return null;
            }
        }
        protected int UserId => int.Parse(CurrentUser?.FindFirstValue("userId")?.ToString() ?? "0");

    }
}
