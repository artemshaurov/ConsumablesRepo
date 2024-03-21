namespace Common.Consumables
{
    public struct ConsumeRewardArgs
    {
        public string Source { get; set; }
        public string PackName { get; set; }
        public ConsumableItem[] RewardItems { get; set; }
    }
}