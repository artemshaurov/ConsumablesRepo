namespace Common.Consumables
{
    public interface IConsumableResourcesInfoLoader
    {
        Result<ConsumableResourcesInfo> Load();
    }
}