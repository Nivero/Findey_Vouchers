using System;

namespace FindeyVouchers.Domain
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public ApplicationUser Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}