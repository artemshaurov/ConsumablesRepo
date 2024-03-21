using NUnit.Framework;

namespace Common.Consumables.Tests
{
    public class ConsumablesLogicTests
    {
        private ConsumableResourceData m_GoldData;
        private ConsumableResourceData m_CrystalsData;
        private ConsumableResourceData m_JokerBoosterData;
        private IConsumableResourcesCacheManager m_CacheManager;
        private IConsumableResourcesInfoLoader m_ResInfoInfoLoader;
        private IConsumableResourcesController m_ConsumableResourcesController;

        [SetUp]
        public void SetUp()
        {
            m_ConsumableResourcesController = new ConsumableResourcesController();
            m_ResInfoInfoLoader = new ConsumableResourcesInfoLoader();
            m_GoldData = new ConsumableResourceData("gold_currency", 0);
            m_CrystalsData = new ConsumableResourceData("crystals_currency", 0);
            m_JokerBoosterData = new ConsumableResourceData("joker_booster", 0);

            m_CacheManager = new MockConsumableResourcesCacheManager(
                m_GoldData,
                m_CrystalsData, 
                m_JokerBoosterData);

            var registrar = new ConsumableResourceEarnerRegistrar(
                m_ResInfoInfoLoader.Load(),
                m_CacheManager, 
                m_ConsumableResourcesController);
            
            registrar.RegisterAll();
        }
        
        [Test] 
        public void Should_earn_10_gold()
        {
            var earnResult = m_GoldData.Earn(new EarnArgs
            {
                Amount = 10, 
                Source = "Should_earn_10_gold"
            });
            
            Assert.IsTrue(earnResult.IsSuccess);
            Assert.AreEqual(10, m_GoldData.Count);
        }

        [Test]
        public void Should_spend_5_gold()
        {
            Should_earn_10_gold();

            var spendResult = m_GoldData.Spend(new SpendArgs
            {
                Amount = 5,
                Source = "Should_spend_5_gold",
            });
            
            Assert.IsTrue(spendResult.IsSuccess);
            Assert.AreEqual(5, m_GoldData.Count);
        }
        
        [Test]
        public void Should_not_earn_negative_10_gold()
        {
            var earnResult = m_GoldData.Earn(new EarnArgs
            {
                Amount = -10,
                Source = "Should_not_earn_negative_10_gold"
            });
            
            Assert.IsFalse(earnResult.IsSuccess);
            Assert.AreEqual(0, m_GoldData.Count);
        }

        [Test]
        public void Should_not_spend_15_gold()
        {
            Should_earn_10_gold();

            var spendResult = m_GoldData.Spend(new SpendArgs
            {
                Amount = 15,
                Source = "Should_not_earn_negative_10_gold",
            });
            
            Assert.IsFalse(spendResult.IsSuccess);
            Assert.AreEqual(10, m_GoldData.Count);
        }

        [Test]
        public void Should_rise_on_earn_event()
        {
            var prevCount = -1;
            var newCount = -1;
            var source = string.Empty;
            var resourceName = string.Empty;
            
            m_GoldData.OnEarned += args =>
            {
                prevCount = args.PrevCount;
                newCount = args.NewCount;
                source = args.Source;
                resourceName = args.ResourceName;
            };

            var earnResult = m_GoldData.Earn(new EarnArgs
            {
                Amount = 10,
                Source = "Should_rise_on_earn_event"
            });

            Assert.IsTrue(earnResult.IsSuccess);
            Assert.AreEqual(0, prevCount);
            Assert.AreEqual(10, newCount);
            Assert.AreEqual("Should_rise_on_earn_event", source);
            Assert.AreEqual("gold_currency", resourceName);
        }
        
        [Test]
        public void Should_rise_on_spend_event()
        {
            Should_earn_10_gold();
            
            var prevCount = -1;
            var newCount = -1;
            var source = string.Empty;
            var resourceName = string.Empty;

            m_GoldData.OnSpend += args =>
            {
                prevCount = args.PrevCount;
                newCount = args.NewCount;
                source = args.Source;
                resourceName = args.ResourceName;
            };

            var spendResult = m_GoldData.Spend(new SpendArgs
            {
                Amount = 5,
                Source = "Should_rise_on_spend_event",
            });

            Assert.IsTrue(spendResult.IsSuccess);
            Assert.AreEqual(10, prevCount);
            Assert.AreEqual(5, newCount);
            Assert.AreEqual("Should_rise_on_spend_event", source);
            Assert.AreEqual("gold_currency", resourceName);
        }
        
        [Test]
        public void Should_consume_pack()
        {
            m_ConsumableResourcesController.ConsumeReward(new ConsumeRewardArgs
            {
                Source = "Should_consume_pack",
                PackName = "test_pack",
                RewardItems = new[]
                {
                    new ConsumableItem("gold_currency", 10),
                    new ConsumableItem("crystals_currency", 5)
                }
            });
            
            Assert.AreEqual(10, m_GoldData.Count);
            Assert.AreEqual(5, m_CrystalsData.Count);
        }

        [Test]
        public void Should_pay_single_currency()
        {
            const string source = nameof(Should_pay_single_currency);
            m_GoldData.Earn(new EarnArgs
            {
                Source = source,
                Amount = 10
            });
            
            var pack = new PaymentArgs
            {
                ProductName = "10_gold_product",
                Source = source,
                PriceItems = new[]
                {
                    new ConsumableItem("gold_currency", 10)
                }
            };

            var buyProductResult = m_ConsumableResourcesController.Pay(pack);
            Assert.IsTrue(buyProductResult.IsSuccess);
            Assert.AreEqual(0, m_GoldData.Count);
        }
        
        [Test]
        public void Should_pay_multiple_currencies()
        {
            const string source = nameof(Should_pay_multiple_currencies);
            m_GoldData.Earn(new EarnArgs
            {
                Source = source,
                Amount = 10
            });

            m_CrystalsData.Earn(new EarnArgs
            {
                Source = source,
                Amount = 10
            });
            
            var pack = new PaymentArgs
            {
                ProductName = "10_gold+10_crystals_product",
                Source = source,
                PriceItems = new[]
                {
                    new ConsumableItem("gold_currency", 10),
                    new ConsumableItem("crystals_currency", 10)
                }
            };

            var buyProductResult = m_ConsumableResourcesController.Pay(pack);
            Assert.IsTrue(buyProductResult.IsSuccess);
            Assert.AreEqual(0, m_GoldData.Count);
            Assert.AreEqual(0, m_CrystalsData.Count);
        }
        
        [Test]
        public void Should_not_pay_when_not_enough_currency()
        {
            const string source = nameof(Should_not_pay_when_not_enough_currency);
            m_GoldData.Earn(new EarnArgs
            {
                Source = source,
                Amount = 5
            });

            var pack = new PaymentArgs
            {
                ProductName = "10_gold_product",
                Source = source,
                PriceItems = new[]
                {
                    new ConsumableItem("gold_currency", 10),
                }
            };
            
            var buyProductResult = m_ConsumableResourcesController.Pay(pack);
            Assert.IsFalse(buyProductResult.IsSuccess);
            Assert.AreEqual(5, m_GoldData.Count);
        }
        
        [Test]
        public void Should_not_pay_when_not_enough_currency_for_pack()
        {
            const string source = nameof(Should_not_pay_when_not_enough_currency_for_pack);
            m_GoldData.Earn(new EarnArgs
            {
                Source = source,
                Amount = 10
            });

            m_CrystalsData.Earn(new EarnArgs
            {
                Source = source,
                Amount = 5
            });

            var pack = new PaymentArgs
            {
                ProductName = "10_gold+10_crystals_product",
                Source = source,
                PriceItems = new[]
                {
                    new ConsumableItem("gold_currency", 10),
                    new ConsumableItem("crystals_currency", 10)
                }
            };
            
            var buyProductResult = m_ConsumableResourcesController.Pay(pack);
            Assert.IsFalse(buyProductResult.IsSuccess);
            Assert.AreEqual(10, m_GoldData.Count);
        }

        [Test]
        public void Should_buy_test_booster_product()
        {
            const string source = nameof(Should_buy_test_booster_product);
            
            m_CrystalsData.Earn(new EarnArgs
            {
                Amount = 1000,
                Source = source
            });

            var product = new ProductArgs
            {
                ProductName = "joker_booster",
                Source = source,
                PriceItems = new[]
                {
                    new ConsumableItem("crystals_currency", 700)
                },
                RewardItems = new []
                {
                    new ConsumableItem("joker_booster", 3)
                }
            };
            
            var buyProductResult = m_ConsumableResourcesController.BuyProduct(product);
            Assert.IsTrue(buyProductResult.IsSuccess);
            Assert.AreEqual(300, m_CrystalsData.Count);
            Assert.AreEqual(3, m_JokerBoosterData.Count);
        }
    }
}
