using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundLayoutButtonsBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject standartCalcCanvas;
    [SerializeField] private GameObject buttonsLayout;
    private Button[] buttons;
    private Transform thisParent;
    private Transform historyForeground;
    private Transform menuForeground;

    private void Awake()
    {
        thisParent = gameObject.transform.parent;
        buttons = buttonsLayout.GetComponentsInChildren<Button>();
        historyForeground = thisParent.Find("ForegroundHistoryOnTop");
        menuForeground = thisParent.Find("ForegroundMenuOnTop");
        foreach (var item in buttons)
        {
            Debug.Log(item.name);
        }

        if (!standartCalcCanvas.activeSelf)
        {
            foreach (var item in buttons)
            {
                item.enabled = false;
            }
        }
    }

    private void Update()
    {
        buttons[0].onClick
                  .AddListener(
                                delegate
                                {
                                    ButtonClicked(0);
                                });

        buttons[1].onClick
                  .AddListener(
                                delegate
                                {
                                    ButtonClicked(1);
                                });
    }

    private void ButtonClicked(int buttonIndex)
    {
        if (buttonIndex == 0)
        {
            historyForeground.gameObject.SetActive(false);
            if (!menuForeground.gameObject.activeSelf)
            {
                menuForeground.gameObject.SetActive(true);
            }
            else
            {
                menuForeground.gameObject.SetActive(false);
            }
        }

        if (buttonIndex == 1)
        {
            menuForeground.gameObject.SetActive(false);
            if (!historyForeground.gameObject.activeSelf)
            {
                historyForeground.gameObject.SetActive(true);
            }
            else
            {
                historyForeground.gameObject.SetActive(false);
            }
        }
    }
}
