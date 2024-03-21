using System;

namespace Common.Consumables
{
    public interface IConsumableResourcesCacheManager
    {
        public ConsumableResourceData Load(
            string resourceName, Func<ConsumableResourceData> createDefault);
    }
}