using System.Collections.Immutable;
using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Domain.Entities;
using eStoreOnline.Domain.Exceptions;
using eStoreOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eStoreOnline.Application.Implementations;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddItemToCartAsync(AddItemToCartModel model)
    {
        var cart = await _context.Carts.Include(x => x.CartDetails)
            .FirstOrDefaultAsync(x => x.Id == model.CartId && x.UserId == model.UserId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = model.UserId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = model.UserId,
                ModifiedBy = model.UserId
            };

            _context.Carts.Add(cart);
        }

        var cartDetail = cart.CartDetails.Find(x => x.ProductId == model.ProductId);

        if (cartDetail == null)
        {
            var cartItem = new CartDetail
            {
                CartId = model.CartId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = model.UserId,
                ModifiedBy = model.UserId
            };

            cart.CartDetails.Add(cartItem);
        }
        else
        {
            cartDetail.Quantity += model.Quantity;
            cartDetail.ModifiedDate = DateTime.Now;
            cartDetail.ModifiedBy = model.UserId;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemFromCartAsync(RemoveItemFromCartModel model)
    {
        var cart = await _context.Carts.Include(x => x.CartDetails)
            .FirstOrDefaultAsync(x => x.Id == model.CartId && x.UserId == model.UserId);

        if (cart == null)
            throw new NotFoundException(nameof(Cart), model.CartId);

        var cartDetail = cart.CartDetails.Find(x => x.ProductId == model.ProductId);

        if (cartDetail == null)
            return;

        cart.CartDetails.Remove(cartDetail);

        await _context.SaveChangesAsync();
    }

    public async Task<CartModel> GetCartByUserAsync(CartRequestModel request)
    {
        var cart = await _context.Carts.Include(x => x.CartDetails).ThenInclude(cartDetail => cartDetail.Product)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.DeletedDate.HasValue);

        if (cart == null)
            return new CartModel();

        return new CartModel
        {
            CartDetail = cart.CartDetails.Select(x => new CartDetailModel()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Price,
                Amount = x.Quantity * x.Price,
                ProductName = x.Product.ProductName,
                ShortDescription = x.Product.ShortDescription
            }).ToImmutableList()
        };
    }

    public async Task<CartModel> UpdateCartAsync(UpdateCartRequestModel request)
    {
        var cart = await _context.Carts.Include(x => x.CartDetails).ThenInclude(cartDetail => cartDetail.Product)
            .FirstOrDefaultAsync(x => x.Id == request.CartId && x.UserId == request.UserId && !x.DeletedDate.HasValue);

        if (cart == null)
            return new CartModel();

        if (request.CartDetails.Count == 0)
            return new CartModel();

        foreach (var item in request.CartDetails)
        {
            var cartDetail = cart.CartDetails.Find(x => x.ProductId == item.ProductId);

            if (cartDetail == null)
            {
                var cartItem = new CartDetail
                {
                    CartId = request.CartId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = request.UserId,
                    ModifiedBy = request.UserId
                };

                cart.CartDetails.Add(cartItem);
            }
            else
            {
                cartDetail.Quantity = item.Quantity;
                cartDetail.ModifiedDate = DateTime.Now;
                cartDetail.ModifiedBy = request.UserId;
            }
        }

        // get products that were removed from cart
        var removedProducts = cart.CartDetails
            .Where(x => !request.CartDetails.Select(y => y.ProductId).Contains(x.ProductId)).ToList();

        foreach (var item in removedProducts)
        {
            cart.CartDetails.Remove(item);
        }

        await _context.SaveChangesAsync();

        var result = await GetCartByUserAsync(new CartRequestModel()
        {
            UserId = request.UserId
        });

        return result;
    }
}