using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Consumables
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class ConsumableResourceInfo
    {
        [field: SerializeField, JsonProperty("resource_name")]
        public string ResourceName { get; }

        [field: SerializeField, JsonProperty("data_key")]
        public string DataKey { get; }
        
        [field: SerializeField, JsonProperty("start_count")]
        public int StartCount { get; }
        
        [field: SerializeField, JsonProperty("icon_address")]
        public Dictionary<string, string> IconAddress { get; }
        
        [field: SerializeField, JsonProperty("category")]
        public string Category { get; }
        
        [field: SerializeField, JsonProperty("display_name")]
        public string DisplayNameKey { get; }

        public ConsumableResourceInfo()
        {
        }

        public ConsumableResourceInfo(
            string resourceName,
            string dataKey,
            int startCount,
            string category,
            string displayNameKey,
            Dictionary<string, string> iconAddress)
        {
            ResourceName = resourceName;
            DataKey = dataKey;
            StartCount = startCount;
            IconAddress = iconAddress;
            Category = category;
            DisplayNameKey = displayNameKey;
        }
    }
}