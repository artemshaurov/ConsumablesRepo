using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Consumables
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class ConsumableResourcesInfo
    {
        [field: SerializeField, JsonProperty("resources")]
        public ConsumableResourceInfo[] Resources { get; } 
            = Array.Empty<ConsumableResourceInfo>();
    }
}