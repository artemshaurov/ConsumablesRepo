using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Consumables
{
    public class ConsumableResourcesInfoLoader : IConsumableResourcesInfoLoader
    {
        private Result<ConsumableResourcesInfo> m_CachedResult
            = Result<ConsumableResourcesInfo>.Fail("Not loaded yet.");

        public Result<ConsumableResourcesInfo> Load()
        {
            if (m_CachedResult.IsExist)
            {
                return m_CachedResult;
            }
            
            try
            {
                var json = Resources.Load<TextAsset>("ConsumableResourcesInfo").text;
                var resources = JsonConvert.DeserializeObject<ConsumableResourcesInfo>(json);
                return m_CachedResult = Result<ConsumableResourcesInfo>.Success(resources);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return Result<ConsumableResourcesInfo>.Fail(
                    $"Failed to load ConsumableResources with exception: {e.Message}");
            }
        }
    }
}