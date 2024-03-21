using Common.Consumables.View;

namespace Common.Consumables.Tests
{
    public class MockConsumableResourceView : IConsumableResourceView
    {
        public string CountLabel { get; private set; }
        public IResourceIcon ResourceIcon { get;  private set; }
        
        public void SetCountLabelText(string text)
        {
            CountLabel = text;
        }

        public void SetIcon(IResourceIcon resourceIcon)
        {
            ResourceIcon = resourceIcon;
        }
    }
}