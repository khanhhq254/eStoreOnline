using eStoreOnline.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Areas.Admin.Controllers;

[Area(AreaConstants.AdminAreaName)]
[Authorize(Roles = RoleConstants.Admin)]
public class AdminBaseController : Controller
{
}