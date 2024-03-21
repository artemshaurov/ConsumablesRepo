using System.Collections.Generic;

namespace Common.Consumables.View
{
    public class ConsumableResourcesCountLabelController : IConsumableResourcesCountLabelController
    {
        private readonly Dictionary<string, IConsumableResourceCountLabelConverter> m_CountLabelConverterMap
            = new Dictionary<string, IConsumableResourceCountLabelConverter>();

        public void RegisterConverter(string category, IConsumableResourceCountLabelConverter converter)
        {
            m_CountLabelConverterMap.Add(category, converter);
        }

        public string Convert(string category, int count)
        {
            return !m_CountLabelConverterMap.TryGetValue(
                category,
                out var converter) 
                ? $"{count}" 
                : converter.Convert(count);
        }
    }
}