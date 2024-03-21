namespace Common.Consumables.View
{
    public interface IConsumableItemsPackView
    {
        void AddItem(IConsumableResourceView view);
        void UpdateLayout();
        void SetLayoutConfigurator(ILayoutConfigurator layoutConfigurator);
    }
}