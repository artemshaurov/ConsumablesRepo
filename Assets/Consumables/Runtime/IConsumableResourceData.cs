namespace Common.Consumables
{
    public interface IConsumableResourceData : IConsumableResourceEarner, IConsumableResourceSpender
    {
        public int Count { get; }
    }
}