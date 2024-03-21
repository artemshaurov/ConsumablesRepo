using System;
using System.Diagnostics.CodeAnalysis;

namespace Common.Consumables.View
{
    public class GenericCountLabelConverter : IConsumableResourceCountLabelConverter
    {
        private readonly Func<int, string> m_CountProvider;

        public GenericCountLabelConverter([NotNull] Func<int, string> countProvider)
        {
            m_CountProvider = countProvider;
        }

        public string Convert(int count)
        {
            return m_CountProvider.Invoke(count);
        }
    }
}