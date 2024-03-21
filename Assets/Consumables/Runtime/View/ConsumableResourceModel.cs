using System.Collections.Generic;

namespace Common.Consumables.View
{
    public class ConsumableResourceModel
    {
        public ConsumableResourceModel(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; }
        public string Category { get; internal set; } = ConsumablesEnvironment.Categories.Currency;
        public string DataKey { get; internal set; }
        public int StartCount { get; internal set; }
        public string DisplayNameKey { get; set; }
        public IDictionary<string, string> IconAddresses { get; internal set; } = new Dictionary<string, string>();

        public Result<string> GetIconAddress(string iconType)
        {
            return IconAddresses.ContainsKey(iconType) 
                ? Result<string>.Success(IconAddresses[iconType]) 
                : Result<string>.Fail($"Icon type {iconType} not found.");
        }
    }
}