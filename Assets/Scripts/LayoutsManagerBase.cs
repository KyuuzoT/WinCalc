using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LayoutsManagerBase : MonoBehaviour
{
    private Transform activeLayoutTransform;
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

    private string pattern = "[-+*/]";
    public string RegexPattern { get; }

    internal virtual void ChangeActiveLayout()
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
