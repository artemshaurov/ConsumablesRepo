namespace Common.Consumables
{
    public struct PaymentArgs
    {
        public string ProductName { get; set; }
        public string Source { get; set; }
        public ConsumableItem[] PriceItems { get; set; }
    }
}