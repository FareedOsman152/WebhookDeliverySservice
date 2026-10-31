using Ardalis.SmartEnum;
using Domain.Marketplaces;
using Domain.Shippers;

namespace Domain.ShippingContracts;

public sealed class ShippingContract: BaseEntity
{
    private ShippingContract() { }

    public ShippingContract(Guid marketplaceId, Guid shipperId)
    {
        MarketplaceId = marketplaceId;
        ShipperId = shipperId;
        Status = ShippingContractStatus.Active;
        _orders = [];
    }
    
    public ShippingContractStatus Status { get; private set; }
    // here other details based on the real life
    public Guid MarketplaceId { get; private set; }
    public Marketplace Marketplace { get; private set; }
    public Guid ShipperId { get; private set; }
    public Shipper Shipper { get; private set; }
    
    public IEnumerable<MarketplaceOrder> Orders => _orders;
    private List<MarketplaceOrder> _orders;

}

public sealed class ShippingContractStatus : SmartEnum<ShippingContractStatus, string>
{
    private ShippingContractStatus(string id, string englishName, string arabicName) : base(englishName, id)
    {
        NameAr = arabicName;
    }

    public string Id => Value;
    public string NameEn => Name;
    public string NameAr { get; private set; }

    public static readonly ShippingContractStatus Active = new("ACTIVE", nameof(Active), "نشط");
    public static readonly ShippingContractStatus Cancelled = new("CANCELLED", nameof(Cancelled), "ملغى");
    public static readonly ShippingContractStatus Closed = new("CLOSED", nameof(Closed), "مغلق");
}
