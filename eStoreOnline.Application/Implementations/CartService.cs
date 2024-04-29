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
            .FirstOrDefaultAsync(x => x.UserId == model.UserId);

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
                CartId = cart.Id,
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
            .FirstOrDefaultAsync(x => x.UserId == model.UserId);

        if (cart == null)
            throw new NotFoundException(nameof(Cart), model.UserId);

        var cartDetail = cart.CartDetails.Find(x => x.ProductId == model.ProductId);

        if (cartDetail == null)
            return;

        cart.CartDetails.Remove(cartDetail);

        await _context.SaveChangesAsync();
    }

    public async Task<CartModel> GetCartByUserAsync(string userId)
    {
        var cart = await _context.Carts.Include(x => x.CartDetails).ThenInclude(cartDetail => cartDetail.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId && !x.DeletedDate.HasValue);

        if (cart == null)
            return new CartModel();

        return new CartModel
        {
            CartId = cart.Id,
            CartDetail = cart.CartDetails.Select(x => new CartDetailModel()
            {
                ProductId = x.ProductId,
                ImageUrl = x.Product.ImageUrl,
                Quantity = x.Quantity,
                Price = x.Product.Price,
                Amount = x.Quantity * x.Product.Price,
                ProductName = x.Product.ProductName,
                ShortDescription = x.Product.ShortDescription
            }).ToImmutableList()
        };
    }

    public async Task<CartModel> UpdateCartAsync(UpdateCartRequestModel request)
    {
        var cart = await _context.Carts.Include(x => x.CartDetails).ThenInclude(cartDetail => cartDetail.Product)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.DeletedDate.HasValue);

        if (cart == null)
            return new CartModel();

        foreach (var item in request.CartDetails)
        {
            var cartDetail = cart.CartDetails.Find(x => x.ProductId == item.ProductId);

            if (cartDetail == null)
            {
                var cartItem = new CartDetail
                {
                    CartId = cart.Id,
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

        var result = await GetCartByUserAsync(request.UserId);

        return result;
    }
}