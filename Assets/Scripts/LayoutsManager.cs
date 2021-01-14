using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutsManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;

    private void Awake()
    {
        buttons.AddRange(gameObject.GetComponentsInChildren<Button>());
    }
}
