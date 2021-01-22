using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LayoutsManagerBase : MonoBehaviour
{
    internal Transform ActiveLayoutTransform { get; set; }

    private string expression;
    internal string Expression
    {
        get
        {
            if(string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression))
            {
                return "0";
            }

            return expression;
        }
        set
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                expression = "0";
            }

            expression = value;
        }
    }

    private string pattern = "[-+*/=]";
    public string RegexPattern 
    { 
        get
        {
            return pattern;
        }
    }

    [SerializeField] private TMPro.TextMeshProUGUI calcSequence;

    public TMPro.TextMeshProUGUI CalculationSequence 
    { 
        get
        {
            return calcSequence;
        }
        
        set
        {
            calcSequence = value;
        }
    }

    [SerializeField] private TMPro.TextMeshProUGUI volumeFrom;

    public TMPro.TextMeshProUGUI VolumeFrom
    {
        get
        {
            return volumeFrom;
        }

        set
        {
            volumeFrom = value;
        }
    }

    [SerializeField] private TMPro.TextMeshProUGUI lengthFrom;

    public TMPro.TextMeshProUGUI LengthFrom
    {
        get
        {
            return lengthFrom;
        }

        set
        {
            lengthFrom = value;
        }
    }

    internal virtual void ChangeActiveLayout(Transform layoutTransform)
    {

    }

    internal virtual Transform ActivateLayout(Transform layoutTransform)
    {
        Transform activeLayout = layoutTransform ;
        activeLayout.gameObject.SetActive(true);

        return activeLayout;
    }

    internal abstract List<UnityEngine.UI.Button> GetButtons(Transform layoutTransform);

    internal abstract void SetButtonsBehaviour(List<UnityEngine.UI.Button> buttons);
}
