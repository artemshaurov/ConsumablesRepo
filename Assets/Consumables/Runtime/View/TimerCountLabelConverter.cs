using System;
using System.Collections.Generic;

namespace Common.Consumables.View
{
    public class TimerCountLabelConverter : IConsumableResourceCountLabelConverter
    {
        private const string m_DaysStr = "d";
        private const string m_HoursStr = "h";
        private const string m_MinutesStr = "m";
        private const string m_SecondsStr = "s";

        public string Convert(int count)
        {
            var timeSpan = TimeSpan.FromSeconds(count);

            var countParts = new List<string>();
            if (timeSpan.TotalDays >= 1)
            {
                countParts.Add($"{timeSpan.TotalDays:0}{m_DaysStr}");
            }

            if (timeSpan.TotalHours >= 1
                && timeSpan.Hours > double.Epsilon)
            {
                countParts.Add($"{timeSpan.Hours}{m_HoursStr}");
            }

            if (timeSpan.TotalMinutes >= 1 
                && timeSpan.Minutes > double.Epsilon)
            {
                countParts.Add($"{timeSpan.Minutes}{m_MinutesStr}");
            }

            if (timeSpan.Seconds > double.Epsilon)
            {
                countParts.Add($"{timeSpan.Seconds}{m_SecondsStr}");
            }
            
            return string.Join(" ",countParts);
        }
    }
}