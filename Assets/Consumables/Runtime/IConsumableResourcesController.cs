namespace Common.Consumables
{
    public interface IConsumableResourcesController
    {
        void ConsumeReward(ConsumeRewardArgs rewardArgs);
        PaymentResult Pay(PaymentArgs pack);
        BuyProductResult BuyProduct(ProductArgs args);
        Result<IConsumableResourceData> GetResourceData(string resourceName);
        void Register(string resourceName, IConsumableResourceData consumableResourceData);
    }
}