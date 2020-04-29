namespace FindeyVouchers.Cms.Models
{
    public class HomeViewModel
    {
        public string Email { get; set; }
        public bool AccountComplete { get; set; }
        public bool StripeComplete { get; set; }
        public string StripeUrl { get; set; }
    }
}