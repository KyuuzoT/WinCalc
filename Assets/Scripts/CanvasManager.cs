using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
