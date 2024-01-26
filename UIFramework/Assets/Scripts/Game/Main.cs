using Core.UI;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        UIManager.Init();
    }

    private void Start()
    {
        UIManager.Open<UIDemoPanel>("My message");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UIManager.Close<UIDemoPanel>();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UIManager.Open<UIDemoPanel>("My another message");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            UIManager.Open<UIPopUpPanel>(new UIPopUpPanelData
            (
                titleStr: "PopUp",
                popUpStr: "Hello"
            ));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            UIManager.Close<UIPopUpPanel>();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.CloseAll();
        }
    }
}