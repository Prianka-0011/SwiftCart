namespace SwiftCart.Domain.Enums;

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
