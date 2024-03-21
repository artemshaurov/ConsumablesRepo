using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Consumables
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public struct ConsumableItem
    {
        [field: SerializeField, JsonProperty("resource_name")]
        public string ResourceName { get; }

        [field: SerializeField, JsonProperty("amount")]
        public int Amount { get; }

        public ConsumableItem(string resourceName, int amount)
        {
            ResourceName = resourceName;
            Amount = amount;
        }
    }
}