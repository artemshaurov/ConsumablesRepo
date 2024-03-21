using System.Diagnostics.CodeAnalysis;

namespace Common.Consumables.View
{
    public struct ConsumableItemsPackViewConfig
    {
        public string IconType { get; set; }
        public ConsumableItem[] ConsumableItems { get; set; }
        public IConsumableResourceView ResourceViewSample { get; set; }
        [AllowNull] public ILayoutConfigurator LayoutConfigurator { get; set; }
    }
}