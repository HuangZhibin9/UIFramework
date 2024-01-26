using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// UIPopUpPanel所需的数据类型
    /// </summary>
    public class UIPopUpPanelData
    {
        public string titleStr;
        public string popUpStr;

        public UIPopUpPanelData(string titleStr, string popUpStr)
        {
            this.titleStr = titleStr;
            this.popUpStr = popUpStr;
        }
    }

    [UILayer(UIPanelLayer.PopUp)]
    public class UIPopUpPanel : UIPanelBase
    {
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI popUpText;
        private Button closeButton;

        public override void OnUIAwake()
        {
            base.OnUIAwake();
            titleText = transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
            popUpText = transform.Find("PopUpText").GetComponent<TextMeshProUGUI>();
            closeButton = transform.Find("CloseButton").GetComponent<Button>();
        }

        public override void OnUIStart()
        {
            base.OnUIStart();
        }

        public override void SetData(object data)
        {
            base.SetData(data);
            var myData = data as UIPopUpPanelData;
            titleText.text = myData.titleStr;
            popUpText.text = myData.popUpStr;
            Debug.Log(myData);
        }

        public override void OnUIEnable()
        {
            base.OnUIEnable();
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        public override void OnUIDisable()
        {
            base.OnUIDisable();
            closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        public override void OnUIDestory()
        {
            base.OnUIDestory();
        }

        private void OnCloseButtonClick()
        {
            UIManager.Close(this);
        }
    }
}