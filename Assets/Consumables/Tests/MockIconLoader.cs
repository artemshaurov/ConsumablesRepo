using Common.Consumables.View;
using NSubstitute;

namespace Common.Consumables.Tests
{
    public class MockIconLoader : IIconLoader
    {
        public IconLoadingWrapper Load(string address)
        {
            var result = Substitute.For<IResourceIcon>();
            result.AssetName.Returns(address);
            var iconLoadingWrapper = new IconLoadingWrapper()
            {
                IsCompleted = true,
                IsSucceed = true,
                Result = result
            };
            return iconLoadingWrapper;
        }
    }
}