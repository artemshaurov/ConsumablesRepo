using System;

namespace Common.Consumables
{
    public interface IConsumableResourceSpender
    {
        event Action<ResourceChangedArgs> OnSpend;
        SpendResult Spend(SpendArgs args);
        SpendResult CanSpend(SpendArgs spendArgs);
    }
}