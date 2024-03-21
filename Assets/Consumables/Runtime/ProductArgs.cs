namespace Common.Consumables
{
    public struct ProductArgs
    {
        public string ProductName { get; set; }
        public string Source { get; set; }
        public ConsumableItem[] PriceItems { get; set; }
        public ConsumableItem[] RewardItems { get; set; }
    }
}