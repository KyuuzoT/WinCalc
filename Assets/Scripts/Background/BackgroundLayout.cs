using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundLayout : LayoutsManagerBase
{
    List<Button> buttonsInternalVar;

    void Update()
    {
        SetButtonsBehaviour(GetButtons(transform));
    }

    internal override List<Button> GetButtons(Transform layoutTransform)
    {
        List<Button> buttonsOnLayout = new List<Button>();
        buttonsOnLayout.AddRange(layoutTransform.GetComponentsInChildren<Button>());
        buttonsInternalVar = new List<Button>(buttonsOnLayout);
        return buttonsOnLayout;
    }

    internal override void SetButtonsBehaviour(List<Button> buttons)
    {
        List<Button> buttonsOnLayout = new List<Button>(buttons);

        foreach (var btn in buttonsOnLayout)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick
                .AddListener(
                                delegate 
                                {
                                    ButtonClicked(btn);
                                });
        }
    }

    private void ButtonClicked(Button button)
    {
        //button.interactable = false;
        var calcParent = transform.parent;
        Debug.LogWarning("Parent: " + calcParent);
        switch (button.name)
        {
            case "StandartCalc":
                Debug.LogWarning("Standart: " + calcParent.GetComponentsInChildren<Transform>(includeInactive: true).Where(x => x.name.Contains("ForegroundStandart")).FirstOrDefault());
                ChangeActiveLayout(calcParent.GetComponentsInChildren<Transform>(includeInactive: true).Where(x => x.name.Contains("ForegroundStandart")).FirstOrDefault());
                break;
            case "VolumeConverter":
                Debug.LogWarning("Volume: " + calcParent.GetComponentsInChildren<Transform>(includeInactive: true).Where(x => x.name.Contains("ForegroundVolume")).FirstOrDefault());
                ChangeActiveLayout(calcParent.GetComponentsInChildren<Transform>(includeInactive: true).Where(x => x.name.Contains("ForegroundVolume")).FirstOrDefault());
                break;
            case "LengthConverter":
                Debug.LogWarning("Length: " + calcParent.GetComponentsInChildren<Transform>(includeInactive: true).Where(x => x.name.Contains("ForegroundLength")).FirstOrDefault());
                ChangeActiveLayout(calcParent.GetComponentsInChildren<Transform>(includeInactive: true).Where(x => x.name.Contains("ForegroundLength")).FirstOrDefault());
                break;
            default:
                break;
        }
    }

    internal override void ChangeActiveLayout(Transform layoutTransform)
    {
        base.ChangeActiveLayout(layoutTransform);
        Transform active = default;
        foreach (var item in transform.parent.GetComponentsInChildren<Transform>().Where(x => x.name.Contains("Foreground")))
        {
            if(item.gameObject.activeSelf)
            {
                active = item;
            }

            if(layoutTransform.Equals(active))
            {
                return;
            }
            Debug.Log("Active: " + active);
            Debug.Log("LayoutTransform: " + layoutTransform);
            active.gameObject.SetActive(false);
            layoutTransform.gameObject.SetActive(true);
            Debug.Log("LayoutTransform: " + layoutTransform.gameObject.activeSelf);
        }
    }

    private void OnDisable()
    {
        foreach (var btn in buttonsInternalVar)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}
