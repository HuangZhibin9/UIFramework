using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [UILayer(UIPanelLayer.Normal)]
    public class UIDemoPanel : UIPanelBase
    {
        public string Messagename { get; set; }

        private Button testButton = null;

        public override void OnUIAwake()
        {
            base.OnUIAwake();
            Debug.Log("UI Awake");
            testButton = transform.Find("TestButton").GetComponent<Button>();
        }

        public override void OnUIStart()
        {
            base.OnUIStart();
            Debug.Log("UI Start");
        }

        public override void SetData(object data)
        {
            base.SetData(data);
            Messagename = data as string;
            Debug.Log($"UI SetData");
        }

        public override void OnUIEnable()
        {
            base.OnUIEnable();
            Debug.Log("UI Enable");
            testButton.onClick.AddListener(TestButtonClick);
        }

        public override void OnUIDisable()
        {
            base.OnUIDisable();
            Debug.Log("UI Disable");
            testButton.onClick.RemoveListener(TestButtonClick);
        }

        public override void OnUIDestory()
        {
            base.OnUIDestory();
            Debug.Log("UI Destory");
        }

        private void TestButtonClick()
        {
            Debug.Log(Messagename);
        }
    }
}