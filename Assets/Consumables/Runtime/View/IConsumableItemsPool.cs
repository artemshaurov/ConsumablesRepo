namespace Common.Consumables.View
{
    public interface IConsumableItemsPool
    {
        IConsumableResourceView Pop();
        void Return(IConsumableResourceView view);
        void ReturnAll();
        void Clear();
    }
}