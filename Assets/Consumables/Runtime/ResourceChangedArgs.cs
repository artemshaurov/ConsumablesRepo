namespace Common.Consumables
{
    public struct ResourceChangedArgs
    {
        public string ResourceName;
        public int PrevCount;
        public int NewCount;
        public string Source;
    }
}