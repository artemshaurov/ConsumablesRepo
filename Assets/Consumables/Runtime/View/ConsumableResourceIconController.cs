using System.Collections.Generic;

namespace Common.Consumables.View
{
    public class ConsumableResourceIconController : IConsumableResourceIconController
    {
        private readonly IIconLoader m_IconLoader;
        private readonly Dictionary<string, IconLoadingWrapper> m_LoadingWrappers
            = new Dictionary<string, IconLoadingWrapper>();

        public ConsumableResourceIconController(IIconLoader iconLoader)
        {
            m_IconLoader = iconLoader;
        }

        public void PreloadAll(string[] addresses)
        {
            foreach (var address in addresses)
            {
                m_LoadingWrappers.Add(address, m_IconLoader.Load(address));
            }
        }

        public Result<IResourceIcon> Load(string iconAddress)
        {
            if (m_LoadingWrappers.TryGetValue(iconAddress, out var wrapper) 
                && wrapper.IsSucceed
                && wrapper.IsCompleted)
            {
                return Result<IResourceIcon>.Success(wrapper.Result);
            }
            
            return Result<IResourceIcon>.Fail($"Icon {iconAddress} not found.");
        }
    }
}