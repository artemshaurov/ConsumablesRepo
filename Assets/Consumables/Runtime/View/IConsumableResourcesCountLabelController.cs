namespace Common.Consumables.View
{
    public interface IConsumableResourcesCountLabelController
    {
        string Convert(string resourceName, int count);
    }
}