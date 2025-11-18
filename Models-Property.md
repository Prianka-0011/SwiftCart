using System;
using System.Collections.Generic;

namespace SwiftCart.Domain;

// =========================
//        ENUMS
// =========================
public enum OrderStatus
{
    Pending,
    Confirmed,
    Packed,
    Shipped,
    OutForDelivery,
    Delivered,
    Canceled,
    Returned
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded
}

public enum PaymentMethod
{
    Card,
    PayPal,
    CashOnDelivery
}

// =========================
//        USER
// =========================
public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public List<Address> Addresses { get; set; } = new();
    public List<Order> Orders { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// =========================
//        ADDRESS
// =========================
public class Address
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public User User { get; set; }
}

// =========================
//     CATEGORY
// =========================
public class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public List<Category> SubCategories { get; set; } = new();
    public List<Product> Products { get; set; } = new();
}

// =========================
//        PRODUCT
// =========================
public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }

    public string SKU { get; set; } = string.Empty;
    public int StockQuantity { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public List<ProductImage> Images { get; set; } = new();
    public List<ProductReview> Reviews { get; set; } = new();
}

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    public Product Product { get; set; }
}

public class ProductReview
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Product Product { get; set; }
    public User User { get; set; }
}

// =========================
//         CART
// =========================
public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public User User { get; set; }
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Cart Cart { get; set; }
    public Product Product { get; set; }
}

// =========================
//        WISHLIST
// =========================
public class Wishlist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public User User { get; set; }
    public List<WishlistItem> Items { get; set; } = new();
}

public class WishlistItem
{
    public Guid Id { get; set; }
    public Guid WishlistId { get; set; }
    public Guid ProductId { get; set; }

    public Wishlist Wishlist { get; set; }
    public Product Product { get; set; }
}

// =========================
//       ORDERS
// =========================
public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public Guid ShippingAddressId { get; set; }

    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public Address ShippingAddress { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }
}

// =========================
//       PAYMENT
// =========================
public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }

    public string? TransactionId { get; set; }
    public DateTime PaidAt { get; set; }

    public Order Order { get; set; }
}
