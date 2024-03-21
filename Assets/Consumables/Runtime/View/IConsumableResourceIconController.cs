namespace Common.Consumables.View
{
    public interface IConsumableResourceIconController
    {
        void PreloadAll(string[] addresses);
        Result<IResourceIcon> Load(string iconAddress);
    }
}