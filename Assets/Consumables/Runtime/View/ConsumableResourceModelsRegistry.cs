using System.Collections.Generic;

namespace Common.Consumables.View
{
    public class ConsumableResourceModelsRegistry
    {
        private readonly Dictionary<string, ConsumableResourceModel> m_ModelsMap
            = new Dictionary<string, ConsumableResourceModel>();

        private ConsumableResourceModel GetOrNew(string resourceName)
        {
            if (!m_ModelsMap.ContainsKey(resourceName))
            {
                m_ModelsMap.Add(resourceName, new ConsumableResourceModel(resourceName));
            }
            
            return m_ModelsMap[resourceName];
        }
        
        public ConsumableResourceModel GetOrCreateResourceModel(string resourceName)
        {
            return GetOrNew(resourceName);
        }
        
        public Result<ConsumableResourceModel> GetResourceModel(string resourceName)
        {
            if (!m_ModelsMap.ContainsKey(resourceName))
            {
                return Result<ConsumableResourceModel>.Fail($"Resource model for {resourceName} not found.");
            }
            
            return Result<ConsumableResourceModel>.Success(m_ModelsMap[resourceName]);
        }

        public void Register(ConsumableResourceInfo resourceInfo)
        {
            var resourceModel = GetOrNew(resourceInfo.ResourceName);
            resourceModel.Category = resourceInfo.Category;
            resourceModel.DataKey = resourceInfo.DataKey;
            resourceModel.StartCount = resourceInfo.StartCount;
            resourceModel.DisplayNameKey = resourceInfo.DisplayNameKey;
            foreach (var (key, value) in resourceInfo.IconAddress)
            {
                resourceModel.IconAddresses[key] = value;
            }
        }
    }
}