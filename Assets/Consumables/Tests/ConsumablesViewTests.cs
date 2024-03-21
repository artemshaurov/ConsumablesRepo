using System;
using System.Collections.Generic;
using System.Globalization;
using Common.Consumables.View;
using NUnit.Framework;

namespace Common.Consumables.Tests
{
    public class ConsumablesViewTests
    {
        private ConsumableResourceViewConfigurator m_ConsumableResourceViewConfigurator;
        private ConsumableResourceModelsRegistry m_ConsumableResourceModelsRegistry;
        private ConsumableResourcesCountLabelController m_CountLabelController;
        private ConsumableResourceIconController m_IconController;

        [SetUp]
        public void SetUp()
        {
            m_ConsumableResourceModelsRegistry = new ConsumableResourceModelsRegistry();
            m_CountLabelController = new ConsumableResourcesCountLabelController();
            
            m_CountLabelController.RegisterConverter(
                ConsumablesEnvironment.Categories.Currency, 
                new GenericCountLabelConverter(num => num.ToString("#,##0",
                    new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." })));
            
            m_CountLabelController.RegisterConverter(
                ConsumablesEnvironment.Categories.Booster, 
                new GenericCountLabelConverter(num => $"x{num}"));
            
            m_CountLabelController.RegisterConverter(
                ConsumablesEnvironment.Categories.Timer, 
                new TimerCountLabelConverter());

            m_IconController = new ConsumableResourceIconController(new MockIconLoader());
            m_ConsumableResourceViewConfigurator 
                = new ConsumableResourceViewConfigurator(
                    m_ConsumableResourceModelsRegistry,
                    m_CountLabelController, 
                    m_IconController, new MockPoolFactory());
            
            m_IconController.PreloadAll(new []
            {
                "gold_icon", 
                "joker_icon",
                "infinity_fan_icon", 
                "gold_icon_big",
                "joker_icon_big",
                "infinity_fan_icon_big"
            });
            
            RegResources();
        }

        private void RegResources()
        {
            var consumableResourceInfos =
                new ConsumableResourceInfo[]
                {
                    new ("gold_currency",
                        "gold_currency_data",
                        0,
                        ConsumablesEnvironment.Categories.Currency,
                        "Gold",
                        new Dictionary<string, string>()
                        {
                            {"default", "gold_icon"},
                            {"big", "gold_icon_big"}
                        }),
                    new("joker_booster",
                        "joker_booster_data",
                        0,
                        ConsumablesEnvironment.Categories.Booster,
                        "Joker",
                        new Dictionary<string, string>()
                        {
                            {"default", "joker_icon"},
                            {"big", "joker_icon_big"}
                        }),
                    new("infinity_fan",
                        "infinity_fan_data",
                        0,
                        ConsumablesEnvironment.Categories.Booster,
                        "Infinity Fan",
                        new Dictionary<string, string>()
                        {
                            {"default", "infinity_fan_icon"},
                            {"big", "infinity_fan_icon_big"}
                        })
                };
            
            foreach (var consumableResourceInfo in consumableResourceInfos)
            {
                m_ConsumableResourceModelsRegistry
                    .Register(consumableResourceInfo);
            }
        }

        [Test]
        public void Should_convert_currency_label()
        {
            var convert = m_CountLabelController.Convert(ConsumablesEnvironment.Categories.Currency, 1_000_000);
            Assert.AreEqual("1.000.000", convert);
        }
        
        [Test]
        public void Should_convert_booster_label()
        {
            var convert = m_CountLabelController.Convert(ConsumablesEnvironment.Categories.Booster, 1);
            Assert.AreEqual("x1", convert);
        }
        
        [Test]
        public void Should_convert_timer_label()
        {
            var convert = m_CountLabelController.Convert(ConsumablesEnvironment.Categories.Timer, (int) TimeSpan.FromHours(25).TotalSeconds);
            Assert.AreEqual("1d 1h", convert);
        }
        
        [Test]
        public void Should_load_gold_icon()
        {
            var result = m_IconController.Load("gold_icon");
            Assert.IsTrue(result.IsExist);
            Assert.AreEqual("gold_icon", result.Object.AssetName);
        }
        
        [Test]
        public void Should_load_joker_icon()
        {
            var result = m_IconController.Load("joker_icon");
            Assert.IsTrue(result.IsExist);
            Assert.AreEqual("joker_icon", result.Object.AssetName);
        }

        [Test]
        public void Should_configure_gold_view()
        {
            var view = new MockConsumableResourceView();
            var args = new ConsumableResourceViewArgs()
            {
                ResourceName = "gold_currency",
                Count = 1_000_000,
                IconType = "default"
            };
            
            m_ConsumableResourceViewConfigurator.Configure(view, args);
            
            Assert.AreEqual("1.000.000", view.CountLabel);
            Assert.AreEqual("gold_icon", view.ResourceIcon.AssetName);
        }
        
        [Test]
        public void Should_configure_gold_view_with_big_icon()
        {
            var view = new MockConsumableResourceView();
            var args = new ConsumableResourceViewArgs
            {
                ResourceName = "gold_currency",
                Count = 1_000_000,
                IconType = "big"
            };
            
            m_ConsumableResourceViewConfigurator.Configure(view, args);
            
            Assert.AreEqual("1.000.000", view.CountLabel);
            Assert.AreEqual("gold_icon_big", view.ResourceIcon.AssetName);
        }

        [Test]
        public void Should_configure_joker_view_with_big_icon()
        {
            var view = new MockConsumableResourceView();
            var args = new ConsumableResourceViewArgs
            {
                ResourceName = "joker_booster",
                Count = 1,
                IconType = "big"
            };
            
            m_ConsumableResourceViewConfigurator.Configure(view, args);
            
            Assert.AreEqual("x1", view.CountLabel);
            Assert.AreEqual("joker_icon_big", view.ResourceIcon.AssetName);
        }
        
        [Test]
        public void Should_auto_update_view_with_view_controller()
        {
            var mockConsumableResourceView = new MockConsumableResourceView();
            var resourceData = new ConsumableResourceData("gold_currency", 1_000_000);
            
            var viewController
                = new ConsumableResourceViewController(
                    mockConsumableResourceView, 
                    resourceData, 
                    m_ConsumableResourceViewConfigurator, 
                    "default");
            
            viewController.BindAndUpdate();
            viewController.Update();

            Assert.AreEqual("1.000.000", mockConsumableResourceView.CountLabel);

            resourceData.Spend(new SpendArgs
            {
                Amount = 1_000,
                ProductName = "test_product",
                Source = "Should_auto_update_view_with_view_controller"
            });
            
            Assert.AreEqual("999.000", mockConsumableResourceView.CountLabel);

            resourceData.Earn(new EarnArgs
            {
                Amount = 100_000,
                Source = "Should_auto_update_view_with_view_controller"
            });
            
            Assert.AreEqual("1.099.000", mockConsumableResourceView.CountLabel);
        }
    }
}