using Ardalis.SmartEnum;

namespace Domain.Order.Enums;

public sealed class OrderEventType : SmartEnum<OrderEventType, string>
{
    private OrderEventType(string id, string englishName, string arabicName) : base(englishName, id)
    {
        NameAr = arabicName;
    }

    public string Id => Value;
    public string NameEn => Name;
    public string NameAr { get; private set; }

    public static readonly OrderEventType MarketplaceSentNewOrder = new("MARKETPLACE_SENT_NEW_ORDER", nameof(MarketplaceSentNewOrder), "الماركت أرسل طلباً جديداً");
    public static readonly OrderEventType SentToShipper = new("SENT_TO_SHIPPER", nameof(SentToShipper), "تم إرساله إلى شركة الشحن");
    public static readonly OrderEventType FailedToSentToShipper = new("FAILED_TO_SENT_TO_SHIPPER", nameof(FailedToSentToShipper), "فشل الإرسال إلى شركة الشحن");
    public static readonly OrderEventType ShipperRefusedTheOrder = new("SHIPPER_REFUSED_THE_ORDER", nameof(ShipperRefusedTheOrder), "رفضت شركة الشحن الطلب");
    public static readonly OrderEventType ReturnedToTheMarketplace = new("RETURNED_TO_THE_MARKETPLACE", nameof(ReturnedToTheMarketplace), "تم إرجاعه إلى الماركت");
    public static readonly OrderEventType MarketplaceUpdatedAndReSentTheOrder = new("MARKETPLACE_UPDATED_AND_SENT", nameof(MarketplaceUpdatedAndReSentTheOrder), "الماركت قام بالتعديل وإعادة الإرسال");
    public static readonly OrderEventType MarketplaceCancelledTheOrder = new("MARKETPLACE_CANCELLED_THE_ORDER", nameof(MarketplaceCancelledTheOrder), "الماركت قام بإلغاء الطلب");
}