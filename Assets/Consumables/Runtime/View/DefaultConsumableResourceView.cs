using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Consumables.View
{
    public class DefaultConsumableResourceView : MonoBehaviour, IConsumableResourceView
    {
        [Serializable] private class UnityStringEvent : UnityEvent<string> {}
        [Serializable] private class UnitySpriteEvent : UnityEvent<Sprite> {}
        
        [SerializeField] private UnityStringEvent m_OnResourceNameChanged;
        [SerializeField] private UnitySpriteEvent m_OnResourceIconChanged;
        private IResourceIcon m_ResourceIcon;

        public void SetCountLabelText(string text)
        {
            m_OnResourceNameChanged?.Invoke(text);
        }

        public void SetIcon(IResourceIcon resourceIcon)
        {
            if (m_ResourceIcon != null 
                && m_ResourceIcon.AssetName == resourceIcon.AssetName)
            {
                return;
            }
            
            m_ResourceIcon = resourceIcon;
            if (resourceIcon is SpriteResourceIcon spriteResourceIcon)
            {
                m_OnResourceIconChanged?.Invoke(spriteResourceIcon.Icon);
            }
        }
    }
}