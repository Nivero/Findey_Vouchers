using System;

namespace FindeyVouchers.Domain.EfModels
{
    public class MerchantVoucher
    {
        public MerchantVoucher()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public ApplicationUser Merchant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidUntil { get; set; }
        public Decimal Price { get; set; }
    }
}