namespace Common.Consumables.View
{
    public class IconLoadingWrapper
    {
        public bool IsCompleted { get; set; }
        public bool IsSucceed { get; set; }
        public string ErrorMessage { get; set; }
        public IResourceIcon Result { get; set; }
    }
}