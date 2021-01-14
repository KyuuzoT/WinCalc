using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LayoutsManager : LayoutsManagerBase
{
    internal override List<Button> GetButtons(Transform layoutTransform)
    {
        List<Button> buttonsOnLayout = new List<Button>();
        buttonsOnLayout.AddRange(layoutTransform.GetComponentsInChildren<Button>());
        return buttonsOnLayout;
    }

    internal override void SetButtonsBehaviour(List<Button> buttons)
    {
        List<Button> buttonsOnLayout = new List<Button>(buttons);

        //Hardcode part
        foreach (var item in buttonsOnLayout)
        {
            item.onClick
                .AddListener(
                                delegate 
                                { 
                                    ButtonClicked(item.name); 
                                });
        }
        throw new System.NotImplementedException();
    }

    private void ButtonClicked(string name)
    {
        switch (name)
        {
            case "ZeroButton":
                AddSymbolToExpression("0");
                break;
            case "OneButton":
                AddSymbolToExpression("0");
                break;
            case "TwoButton":
                AddSymbolToExpression("0");
                break;
            case "ThreeButton":
                AddSymbolToExpression("0");
                break;
            case "FourButton":
                AddSymbolToExpression("0");
                break;
            case "FiveButton":
                AddSymbolToExpression("0");
                break;
            case "SixButton":
                AddSymbolToExpression("0");
                break;
            case "SevenButton":
                AddSymbolToExpression("0");
                break;
            case "EightButton":
                AddSymbolToExpression("0");
                break;
            case "NineButton":
                AddSymbolToExpression("0");
                break;
            case "DotButton":
                AddSymbolToExpression("0");
                break;
            case "CEButton":
                //Replace last part of expression to 0
                ZeroOutLastPartOfExpression();
                break;
            case "RemoveButton":
                AddSymbolToExpression("0");
                break;
            case "PercentButton":
                AddSymbolToExpression("0");
                break;
            case "CButton":
                //Replace all expression to 0
                Expression.Replace(Expression, "0");
                break;
            case "FractionButton":
                AddSymbolToExpression("0");
                break;
            case "SqPowButton":
                AddSymbolToExpression("0");
                break;
            case "SqrtButton":
                AddSymbolToExpression("0");
                break;
            case "DivideButton":
                AddSymbolToExpression("0");
                break;
            case "MultiplyButton":
                AddSymbolToExpression("0");
                break;
            case "MinusButton":
                AddSymbolToExpression("0");
                break;
            case "PlusButton":
                AddSymbolToExpression("0");
                break;
            case "EqualsButton":
                AddSymbolToExpression("0");
                break;
            case "PlusMinusButton":
                AddSymbolToExpression("0");
                break;
            case "Expression":
                AddSymbolToExpression("0");
                break;
        }
    }

    private void ZeroOutLastPartOfExpression()
    {
        int lastPart;
        string[] splittedExpression = Expression.Split(Regex.Match(Expression, "[-+*/]").Value.ToCharArray());

        splittedExpression[1] = "0";
        Expression = $"{splittedExpression[0]}{splittedExpression[1]}";
    }

    private void AddSymbolToExpression(string symbol)
    {
        int number;
        int lastSymbol;

        //If symbol is digit
        if (int.TryParse(symbol, out number))
        {
            //If last symbol of expression is digit
            if (int.TryParse(Expression.Last().ToString(), out lastSymbol))
            {
                if (lastSymbol.Equals(0))
                {
                    Expression = Expression.Replace(Expression.Last().ToString(), number.ToString());
                }
                else
                {
                    Expression = $"{Expression}{number}";
                }
            }
            else
            {
                Expression = $"{Expression}{number}";
            }
        }
    }

    private void ButtonClicked(int index)
    {
        //switch (index)
        //{
        //    default:
        //        break;
        //}
    }
}
