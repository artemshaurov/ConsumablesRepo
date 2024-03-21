using Common.Consumables.View;
using UnityEngine.Pool;

namespace Common.Consumables.Tests
{
    public class MockConsumableItemsPool : IConsumableItemsPool
    {
        private readonly ObjectPool<MockConsumableResourceView> m_Pool;

        public MockConsumableItemsPool()
        {
            m_Pool = new ObjectPool<MockConsumableResourceView>(
                () => new MockConsumableResourceView());
        }

        public IConsumableResourceView Pop()
        {
            return m_Pool.Get();
        }

        public void Return(IConsumableResourceView view)
        {
            if (view is MockConsumableResourceView resourceView)
            {
                m_Pool.Release(resourceView);
            }
        }

        public void ReturnAll()
        {
        }

        public void Clear()
        {
            m_Pool.Clear();
        }
    }
}