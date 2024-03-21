using System;
using System.Collections.Generic;

namespace Common.Consumables.Tests
{
    public class MockConsumableResourcesCacheManager : IConsumableResourcesCacheManager
    {
        private readonly Dictionary<string, ConsumableResourceData> m_CacheMap
            = new Dictionary<string, ConsumableResourceData>();

        public MockConsumableResourcesCacheManager(
            params ConsumableResourceData [] consumableResourceDatas)
        {
            foreach (var consumableResourceData in consumableResourceDatas)
            {
                m_CacheMap.Add(consumableResourceData.ResourceName, consumableResourceData);
            }
        }

        public ConsumableResourceData Load(string resourceName, Func<ConsumableResourceData> createDefault)
        {
            if (m_CacheMap.TryGetValue(resourceName, out var data))
            {
                return data;
            }

            data = new ConsumableResourceData(resourceName, 0);
            m_CacheMap.Add(resourceName, data);

            return data;
        }
    }
}