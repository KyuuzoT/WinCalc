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
                AddSymbolToExpression("1");
                break;
            case "TwoButton":
                AddSymbolToExpression("2");
                break;
            case "ThreeButton":
                AddSymbolToExpression("3");
                break;
            case "FourButton":
                AddSymbolToExpression("4");
                break;
            case "FiveButton":
                AddSymbolToExpression("5");
                break;
            case "SixButton":
                AddSymbolToExpression("6");
                break;
            case "SevenButton":
                AddSymbolToExpression("7");
                break;
            case "EightButton":
                AddSymbolToExpression("8");
                break;
            case "NineButton":
                AddSymbolToExpression("9");
                break;
            case "DotButton":
                AddSymbolToExpression(".");
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
                AddOperationToExpression("/");
                break;
            case "MultiplyButton":
                AddOperationToExpression("/");
                break;
            case "MinusButton":
                AddOperationToExpression("/");
                break;
            case "PlusButton":
                AddOperationToExpression("/");
                break;
            case "EqualsButton":
                AddSymbolToExpression("0");
                break;
            case "PlusMinusButton":
                AddSymbolToExpression("-");
                break;
            case "Expression":
                AddSymbolToExpression("0");
                break;
        }
    }

    private void ZeroOutLastPartOfExpression()
    {
        char operation = Regex.Match(Expression, RegexPattern).Value.FirstOrDefault();
        string[] splittedExpression = Expression.Split(operation);

        if(splittedExpression.Length == 1)
        {
            splittedExpression[0] = "0";
            Expression = splittedExpression[0];
        }
        else
        {
            splittedExpression[1] = "0";
            Expression = $"{splittedExpression[0]}{operation}{splittedExpression[1]}";
        }
    }

    private void AddSymbolToExpression(string input)
    {
        string firstPart = string.Empty;
        string secondPart = string.Empty;
        char operation = Regex.Match(Expression, RegexPattern).Value.FirstOrDefault();
        string[] splittedExpression = Expression.Split(operation);

        firstPart = splittedExpression[0];
        if(splittedExpression.Length > 1)
        {
            secondPart = splittedExpression[1];
        }

        if(input.Contains('.'))
        {
            firstPart = DotValidation(firstPart, input);
            if(!string.IsNullOrEmpty(secondPart))
            {
                secondPart = DotValidation(secondPart, input);
            }
        }
        else if(input.Contains('0'))
        {
            firstPart = ZeroValidation(firstPart, input);
            if (!string.IsNullOrEmpty(secondPart))
            {
                secondPart = DotValidation(secondPart, input);
            }
        }
        else if(input.Contains('-'))
        {
            firstPart = PlusMinusValidation(firstPart, input);
        }

        Expression = $"{firstPart}{operation}{secondPart}";
    }

    private string PlusMinusValidation(string valStr, string input)
    {
        if(input.Contains('-'))
        {
            return $"{input}{valStr}";
        }
        return valStr;
    }

    private string ZeroValidation(string valStr, string input)
    {
        if(input.Contains('0'))
        {
            if(valStr.Contains('.'))
            {
                return $"{valStr}{0}";
            }
            else
            {
                if(valStr.Length == 1 && valStr.First().Equals('0'))
                {
                    return valStr;
                }

                if(valStr.Length > 1)
                {
                    return $"{valStr}{input}";
                }
            }
        }
        throw new NotImplementedException();
    }

    private string DotValidation(string valStr, string input)
    {
        if (input.First().Equals('.') && valStr.Contains('.'))
        {
            return valStr;
        }
        else if (input.First().Equals('.') && !valStr.Contains('.'))
        {
            return $"{valStr}{input}";
        }

        return valStr;
    }

    private void AddOperationToExpression(string input)
    {
        if(!Regex.IsMatch(Expression, RegexPattern))
        {
            Expression = $"{Expression}{input}";
        }
        else
        {
            return;
        }
    }
}
