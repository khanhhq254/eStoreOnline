using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Areas.Admin.Controllers;

public class OrderController : AdminBaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index(GetOrderRequestModel request)
    {
        if (request.PageIndex != 0)
        {
            request.PageIndex--;
        }

        var result = await _orderService.GetOrdersAsync(request);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        ViewBag.Id = id;
        var orderDetail = await _orderService.GetOrderDetailAsync(new GetOrderDetailRequestModel()
        {
            OrderId = id
        });

        return View(orderDetail);
    }
}