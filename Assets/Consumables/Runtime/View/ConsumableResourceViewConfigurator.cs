using UnityEngine;

namespace Common.Consumables.View
{
    public class ConsumableResourceViewConfigurator : IConsumableResourceViewConfigurator
    {
        private readonly ConsumableResourceModelsRegistry m_ModelsRegistry;
        private readonly IConsumableResourcesCountLabelController m_CountLabelController;
        private readonly IConsumableResourceIconController m_IconController;
        private readonly IConsumableItemsPoolFactory m_PoolFactory;
        private readonly ILogger m_Logger;

        public ConsumableResourceViewConfigurator(
            ConsumableResourceModelsRegistry modelsRegistry,
            IConsumableResourcesCountLabelController countLabelController, 
            IConsumableResourceIconController iconController, 
            IConsumableItemsPoolFactory poolFactory)
        {
            m_ModelsRegistry = modelsRegistry;
            m_CountLabelController = countLabelController;
            m_IconController = iconController;
            m_PoolFactory = poolFactory;
            m_Logger = Debug.unityLogger;
        }

        public void Configure(IConsumableResourceView view, ConsumableResourceViewArgs args)
        {
            ConfigureCountLabel(view, args.ResourceName, args.Count);
            ConfigureIcon(view, args.ResourceName, args.IconType);
        }

        public IDisposeItemsWrapper Configure(IConsumableItemsPackView view, ConsumableItemsPackViewConfig config)
        {
            var pool = m_PoolFactory.Create(config.ResourceViewSample);

            foreach (var item in config.ConsumableItems)
            {
                var resourceView = pool.Pop();
                
                Configure(resourceView, new ConsumableResourceViewArgs
                {
                    ResourceName = item.ResourceName,
                    Count = item.Amount,
                    IconType = config.IconType
                });

                view.AddItem(resourceView);
            }

            if (config.LayoutConfigurator != null)
            {
                view.SetLayoutConfigurator(config.LayoutConfigurator);   
            }
            
            view.UpdateLayout();

            return new DisposeItemsWrapper(pool);
        }

        public void ConfigureCountLabel(IConsumableResourceCountLabelView view, string resourceName, int count)
        {
            var countText = GetCountLabel(resourceName, count);
            view.SetCountLabelText(countText);
        }

        public string GetCountLabel(string resourceName, int count)
        {
            var model = m_ModelsRegistry.GetOrCreateResourceModel(resourceName);
            
            return m_CountLabelController.Convert(model.Category, count);
        }

        public void ConfigureIcon(IConsumableResourceIconView view, string resourceName, string iconType)
        {
            var model = m_ModelsRegistry.GetOrCreateResourceModel(resourceName);
            
            var iconAddressResult = model.GetIconAddress(iconType);
            if (!iconAddressResult.IsExist)
            {
                m_Logger.Log(LogType.Error, $"Resource '{resourceName}' have no icon with type '{iconType}'.");
                return;
            }
            
            var iconResult = m_IconController.Load(iconAddressResult.Object);
            if (!iconResult.IsExist)
            {
                m_Logger.Log(LogType.Error, $"Can't load icon for resource '{resourceName}' with type '{iconType}'.");
                return;
            }
            
            view.SetIcon(iconResult.Object);
        }
    }
}