using UnityEngine;

namespace Core.UI
{
    /// <summary>
    /// UI������
    /// </summary>
    public abstract class UIPanelBase : MonoBehaviour
    {
        protected UIPanelBase()
        { }

        public virtual void OnUIAwake()
        { }

        public virtual void OnUIStart()
        { }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="data">������</param>
        public virtual void SetData(object data)
        { }

        public virtual void OnUIEnable()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnUIDisable()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnUIDestory()
        {
            Destroy(gameObject);
        }
    }
}