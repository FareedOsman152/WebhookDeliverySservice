using Ardalis.SmartEnum;

namespace Domain.Order.Enums;

public sealed class OrderStatus : SmartEnum<OrderStatus, string>
{
    private OrderStatus(string id, string englishName, string arabicName) : base(englishName, id)
    {
        NameAr = arabicName;
    }

    public string Id => Value;
    public string NameEn => Name;
    public string NameAr { get; private set; }

    public static readonly OrderStatus ReadyToShipping = new("READY_TO_SHIPPING", nameof(ReadyToShipping), "جاهز للشحن");
    public static readonly OrderStatus InShipping = new("IN_SHIPPING", nameof(InShipping), "قيد الشحن");
    public static readonly OrderStatus Refused = new("REFUSED", nameof(Refused), "مرفوض");
    public static readonly OrderStatus Cancelled = new("CANCELLED", nameof(Cancelled), "ملغي");
}
