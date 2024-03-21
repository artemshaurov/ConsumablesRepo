using Common.Consumables.View;

namespace Common.Consumables
{
    public class ConsumableResourceViewController
    {
        private readonly IConsumableResourceView m_View;
        private readonly ConsumableResourceData m_Data;
        private readonly ConsumableResourceViewConfigurator m_ViewConfigurator;
        private readonly string m_IconType;

        public ConsumableResourceViewController(
            IConsumableResourceView view,
            ConsumableResourceData data,
            ConsumableResourceViewConfigurator viewConfigurator,
            string iconType)
        {
            m_View = view;
            m_Data = data;
            m_ViewConfigurator = viewConfigurator;
            m_IconType = iconType;
        }

        public void BindAndUpdate()
        {
            m_Data.OnEarned += OnChanged;
            m_Data.OnSpend += OnChanged;
            Update();
        }

        private void OnChanged(ResourceChangedArgs obj)
        {
            m_ViewConfigurator.ConfigureCountLabel(m_View, obj.ResourceName, obj.NewCount);
        }

        public void Update()
        {
            m_ViewConfigurator.Configure(m_View, new ConsumableResourceViewArgs
            {
                ResourceName = m_Data.ResourceName,
                Count = m_Data.Count,
                IconType = m_IconType
            });
        }
        
        public void UpdateCountLabel()
        {
            m_ViewConfigurator.ConfigureCountLabel(m_View, m_Data.ResourceName, m_Data.Count);
        }
        
        public void UpdateIcon()
        {
            m_ViewConfigurator.ConfigureIcon(m_View, m_Data.ResourceName, m_IconType);
        }
    }
}