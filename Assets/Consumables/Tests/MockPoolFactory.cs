using Common.Consumables.View;

namespace Common.Consumables.Tests
{
    public class MockPoolFactory : IConsumableItemsPoolFactory
    {
        public IConsumableItemsPool Create(IConsumableResourceView view)
        {
            return new MockConsumableItemsPool();
        }
    }
}