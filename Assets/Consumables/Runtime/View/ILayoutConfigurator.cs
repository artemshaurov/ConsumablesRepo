using UnityEngine;

namespace Common.Consumables.View
{
    public interface ILayoutConfigurator
    {
        void UpdateLayout(RectTransform root);
    }
}