using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index(int pageIndex = 1)
    {
        var result = await _orderService.GetOrdersAsync(new GetOrderRequestModel()
        {
            PageIndex = pageIndex - 1,
            PageSize = 10,
            UserId = GetCurrentUserId()
        });

        return View(result);
    }

    public async Task<IActionResult> OrderDetail(string orderNumber)
    {
        var result = await _orderService.GetOrderDetailAsync(new GetOrderDetailRequestModel()
        {
            OrderNumber = orderNumber,
            UserId = GetCurrentUserId(),
        });

        return View(result);
    }
}