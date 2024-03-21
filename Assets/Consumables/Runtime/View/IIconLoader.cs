namespace Common.Consumables.View
{
    public interface IIconLoader
    {
        IconLoadingWrapper Load(string address);
    }
}