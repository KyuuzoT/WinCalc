using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("Class CanvasManager is deprecated from now on (14.01.2020 10:28). Use Layouts manager on ForegroundLayouts object instead!")]
public class CanvasManager : MonoBehaviour
{
    private Button[] buttons;

    private void Awake()
    {
        buttons = gameObject.GetComponentsInChildren<Button>();
        PrintNames(buttons);
    }

    private void PrintNames(Button[] buttons)
    {
        foreach (var item in buttons)
        {
            Debug.Log(item.name);
        }
    }
}
