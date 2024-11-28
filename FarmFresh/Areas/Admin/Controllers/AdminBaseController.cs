using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FarmFresh.Commons.GeneralApplicationConstants;

namespace FarmFresh.Areas.Admin.Controllers;

[Area(AdminAreaName)]
[Authorize(Roles = AdminRoleName)]
public class AdminBaseController : Controller
{

}
