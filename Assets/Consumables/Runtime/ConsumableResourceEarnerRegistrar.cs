using UnityEngine;

namespace Common.Consumables
{
    public class ConsumableResourceEarnerRegistrar
    {
        private readonly Result<ConsumableResourcesInfo> m_ResourcesInfoResult;
        private readonly IConsumableResourcesCacheManager m_CacheManager;
        private readonly IConsumableResourcesController m_Controller;

        public ConsumableResourceEarnerRegistrar(
            Result<ConsumableResourcesInfo> resourcesInfoResult,
            IConsumableResourcesCacheManager cacheManager, 
            IConsumableResourcesController controller)
        {
            m_ResourcesInfoResult = resourcesInfoResult;
            m_CacheManager = cacheManager;
            m_Controller = controller;
        }
        
        public void RegisterAll()
        {
            if (!m_ResourcesInfoResult.IsExist)
            {
                Debug.LogError($"Failed to load ConsumableResources: {m_ResourcesInfoResult.ErrorMessage}");
                return;
            }

            foreach (var resourceInfo in m_ResourcesInfoResult.Object.Resources)
            {
                var consumableResourceData = m_CacheManager.Load(
                    resourceInfo.ResourceName, 
                    () => new ConsumableResourceData(
                        resourceInfo.ResourceName, 
                        resourceInfo.StartCount));

                m_Controller.Register(resourceInfo.ResourceName, consumableResourceData);
            }
        }
    }
}