using System;

namespace Core.UI
{
    /// <summary>
    /// UI层级特性描述，用于指定UI层级
    /// </summary>
    public class UILayerAttribute : Attribute
    {
        public UIPanelLayer layer { get; }

        /// <summary>
        /// 指定层级面板
        /// </summary>
        /// <param name="layer"></param>
        public UILayerAttribute(UIPanelLayer layer)
        {
            this.layer = layer;
        }
    }
}