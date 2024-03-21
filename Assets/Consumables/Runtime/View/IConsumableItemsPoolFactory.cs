namespace Common.Consumables.View
{
    public interface IConsumableItemsPoolFactory
    {
        IConsumableItemsPool Create(IConsumableResourceView view);
    }
}