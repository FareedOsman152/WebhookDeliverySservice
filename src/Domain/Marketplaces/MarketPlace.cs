using Domain.ShippingContracts;

namespace Domain.Marketplaces;

public sealed class Marketplace: BaseEntity
{
    private Marketplace() {}
    public Marketplace(string tradeLicenseNumber, string nameEn, string nameAr, string? address, string websiteLink, string email, string phoneNumber)
    {
        TradeLicenseNumber = tradeLicenseNumber;
        NameEn = nameEn;
        NameAr = nameAr;
        Address = address;
        WebsiteLink = websiteLink;
        Email = email;
        PhoneNumber = phoneNumber;
        _contracts = [];
    }
    public string TradeLicenseNumber { get; private set; }
    public string NameEn { get; private set; }
    public string NameAr { get; private set; }
    public string? Address { get; private set; } // company  can just have online site
    public string WebsiteLink { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public IEnumerable<ShippingContract> Contracts => _contracts;
    private List<ShippingContract> _contracts;
}