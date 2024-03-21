namespace Common.Consumables.View
{
    public interface IConsumableResourceViewConfigurator
    {
        void Configure(IConsumableResourceView view, ConsumableResourceViewArgs args);
        void ConfigureCountLabel(IConsumableResourceCountLabelView view, string resourceName, int count);
        void ConfigureIcon(IConsumableResourceIconView view, string resourceName, string iconType);
        IDisposeItemsWrapper Configure(IConsumableItemsPackView view, ConsumableItemsPackViewConfig config);
        string GetCountLabel(string resourceName, int count);
    }
}