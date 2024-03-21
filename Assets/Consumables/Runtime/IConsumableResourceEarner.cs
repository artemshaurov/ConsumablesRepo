using System;

namespace Common.Consumables
{
    public interface IConsumableResourceEarner
    {
        event Action<ResourceChangedArgs> OnEarned;
        EarnResult Earn(EarnArgs args);
    }
}