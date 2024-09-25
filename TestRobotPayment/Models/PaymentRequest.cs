namespace PaymentApiProject.Models
{
    public class PaymentRequest
    {
        public string StoreId { get; set; }      // Store ID
        public decimal Amount { get; set; }      // Amount
        public decimal Tax { get; set; }         // Tax
        public decimal ShippingFee { get; set; } // Shipping Fee
    }
}
