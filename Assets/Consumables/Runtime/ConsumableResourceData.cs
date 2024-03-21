using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Consumables
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class ConsumableResourceData : IConsumableResourceData
    {
        [SerializeField] private string m_ResourceName;
        [SerializeField] private int m_Count;
        
        public string ResourceName => m_ResourceName;
        public int Count => m_Count;

        [field: NonSerialized, JsonIgnore]
        public event Action<ResourceChangedArgs> OnEarned;
        
        [field: NonSerialized, JsonIgnore]
        public event Action<ResourceChangedArgs> OnSpend;

        public ConsumableResourceData(string resourceName, int count)
        {
            m_ResourceName = resourceName;
            m_Count = count;
        }

        public EarnResult Earn(EarnArgs args)
        {
            if (args.Amount <= 0)
            {
                return new EarnResult
                {
                    IsSuccess = false,
                    Message = "Amount must be greater than zero"
                };
            }
            
            var prevCount = m_Count;
            m_Count += args.Amount;
            
            var changedArgs = new ResourceChangedArgs
            {
                PrevCount = prevCount,
                NewCount = m_Count,
                ResourceName = m_ResourceName,
                Source = args.Source,
            };
            
            OnEarned?.Invoke(changedArgs);
            return new EarnResult
            {
                IsSuccess = true,
                Message = $"Earned {args.Amount} {m_ResourceName}"
            };
        }

        public SpendResult Spend(SpendArgs args)
        {
            var spendResult = CanSpend(args);
            if (!spendResult.IsSuccess)
            {
                return spendResult;
            }

            var prevCount = m_Count;
            m_Count = Mathf.Max(0, m_Count - args.Amount);
            
            var changedArgs = new ResourceChangedArgs
            {
                PrevCount = prevCount,
                NewCount = m_Count,
                ResourceName = m_ResourceName,
                Source = args.Source,
            };

            OnSpend?.Invoke(changedArgs);
            return new SpendResult
            {
                IsSuccess = true,
                Message = $"Spent {args.Amount} {ResourceName}"
            };
        }

        public SpendResult CanSpend(SpendArgs spendArgs)
        {
            if (spendArgs.Amount <= 0)
            {
                return new SpendResult
                {
                    IsSuccess = false,
                    Message = "Amount must be greater than zero"
                };
            }
            
            if (m_Count < spendArgs.Amount)
            {
                return new SpendResult
                {
                    IsSuccess = false,
                    Message = $"Not enough {m_ResourceName}"
                };
            }

            return new SpendResult
            {
                IsSuccess = true,
                Message = $"Can spend {spendArgs.Amount} {ResourceName}"
            };
        }
    }
}