namespace Common.Consumables.View
{
    public class DisposeItemsWrapper : IDisposeItemsWrapper
    {
        private readonly IConsumableItemsPool m_Pool;

        internal DisposeItemsWrapper(IConsumableItemsPool pool)
        {
            m_Pool = pool;
        }

        public void DestroyAllItems()
        {
            m_Pool.Clear();
        }
    }
}