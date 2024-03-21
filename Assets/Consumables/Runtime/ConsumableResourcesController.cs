using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.Consumables
{
    public class ConsumableResourcesController : IConsumableResourcesController
    {
        private readonly Dictionary<string, IConsumableResourceData> m_DataMap
            = new Dictionary<string, IConsumableResourceData>();

        public Result<IConsumableResourceData> GetResourceData(string resourceName)
        {
            if (m_DataMap.TryGetValue(resourceName, out IConsumableResourceData data))
            {
                return Result<IConsumableResourceData>.Success(data);
            }
            else
            {
                return Result<IConsumableResourceData>.Fail($"Can't find resource data with name ''.");
            }
        }
        
        public void Register(string resourceName, IConsumableResourceData consumableResourceData)
        {
            if (!m_DataMap.ContainsKey(resourceName))
            {
                m_DataMap.Add(resourceName, consumableResourceData);
            }
        }

        public void ConsumeReward(ConsumeRewardArgs rewardArgs)
        {
            var invalidResults = new List<InvalidResourceEarnInfo>();
            foreach (var consumableReward in rewardArgs.RewardItems)
            {
                if (!m_DataMap.ContainsKey(consumableReward.ResourceName))
                {
                    invalidResults.Add(new InvalidResourceEarnInfo
                    {
                        ResourceName = consumableReward.ResourceName,
                        ErrorMessage =  $"Resource {consumableReward.ResourceName} is not registered",
                    });
                    
                    continue;
                }

                var earnResult = m_DataMap[consumableReward.ResourceName].Earn(new EarnArgs
                {
                    Amount = consumableReward.Amount,
                    Source = rewardArgs.Source, 
                });

                if (!earnResult.IsSuccess)
                {
                    invalidResults.Add(new InvalidResourceEarnInfo
                    {
                        ResourceName = consumableReward.ResourceName,
                        ErrorMessage = earnResult.Message,
                    });
                }
            }

            if (!invalidResults.Any())
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine(
                $"Consumable Resource Controller >>> " +
                $"Can't consume resources from pack '{rewardArgs.PackName}':");
                
            foreach (var invalidResourceEarnInfo in invalidResults)
            {
                sb.AppendLine(
                    $"Resource '{invalidResourceEarnInfo.ResourceName}'; " +
                    $"Reason: {invalidResourceEarnInfo.ErrorMessage}");
            }
                
            Debug.LogError(sb.ToString());
        }

        public PaymentResult Pay(PaymentArgs args)
        {
            foreach (var item in args.PriceItems)
            {
                if (!m_DataMap.ContainsKey(item.ResourceName))
                {
                    return new PaymentResult
                    {
                        IsSuccess = false,
                        Message = $"Resource '{item.ResourceName}' not found"
                    };
                }
                
                var spendResult = m_DataMap[item.ResourceName].CanSpend(new SpendArgs
                {
                    Amount = item.Amount,
                    Source = args.Source,
                    ProductName = args.ProductName
                });

                if (!spendResult.IsSuccess)
                {
                    return new PaymentResult
                    {
                        IsSuccess = false,
                        Message = spendResult.Message
                    };
                }
            }

            foreach (var item in args.PriceItems)
            {
                m_DataMap[item.ResourceName].Spend(new SpendArgs
                {
                    Amount = item.Amount,
                    Source = args.Source,
                    ProductName = args.ProductName
                });
            }

            return new PaymentResult
            {
                IsSuccess = true,
                Message = $"Bought {args.ProductName}"
            };
        }

        public BuyProductResult BuyProduct(ProductArgs args)
        {
            var paymentResult = Pay(new PaymentArgs
            {
                ProductName = args.ProductName,
                Source = args.Source,
                PriceItems = args.PriceItems
            });
            
            if (!paymentResult.IsSuccess)
            {
                return new BuyProductResult
                {
                    IsSuccess = false,
                    Message = $"Payment failed! Reason: {paymentResult.Message}"
                };
            }
            
            ConsumeReward(new ConsumeRewardArgs
            {
                PackName = args.ProductName,
                RewardItems = args.RewardItems,
                Source = args.Source
            });

            return new BuyProductResult
            {
                IsSuccess = true,
                Message = $"Bought {args.ProductName}"
            };
        }
    }
}