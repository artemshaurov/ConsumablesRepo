using UnityEngine;

namespace Common.Consumables.View
{
    public struct SpriteResourceIcon : IResourceIcon
    {
        public Sprite Icon { get; }
        public string AssetName => Icon.name;
        
        public SpriteResourceIcon(Sprite icon)
        {
            Icon = icon;
        }
    }
}