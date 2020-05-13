using System.ComponentModel.DataAnnotations;

namespace FindeyVouchers.Domain.EfModels
{
    public class StripeSecret
    {
        [Key] public string Email { get; set; }
        public string Secret { get; set; }
    }
}