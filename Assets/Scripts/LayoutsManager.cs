using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;

public class LayoutsManager : LayoutsManagerBase
{
    private const float CubicCmToPints = 0.0021f;
    private const float CubicCmToLitres = 0.001f;
    private const float PintsToLitres = 0.47f;
    private int optionSelected = 0;

    private List<Button> buttonsInternalPuroposeVar;
    private void Awake()
    {
        Debug.Log(CalculationSequence);
    }

    private void Update()
    {
        ActiveLayoutTransform = GetCurrentActiveLayout();

        if (!string.IsNullOrEmpty(Expression))
        {
            if (ActiveLayoutTransform.tag.Equals("StandartCalc"))
            {
                CalculationSequence.text = Expression;
            }
            else if(ActiveLayoutTransform.tag.Equals("VolumeConverter"))
            {
                VolumeFrom.text = Expression;
                var volumeTo = GetTMPComponent("VolumeTo");
                var volumeFromDropDown = GetDropDownBasedOnName("VolumeFromDropDown");
                var volumeToDropDown = GetDropDownBasedOnName("VolumeToDropDown");
                volumeTo.text = ConvertFromTo(volumeFromDropDown, volumeToDropDown);
            }
            else if(ActiveLayoutTransform.tag.Equals("LengthConverter"))
            {
                LengthFrom.text = Expression;
            }
        }

        SetButtonsBehaviour(GetButtons(ActiveLayoutTransform));
    }

    private Transform GetCurrentActiveLayout()
    {
        Transform active = default;
        foreach (var item in transform.GetComponentsInChildren<Transform>())
        {
            if(item.name.Contains("Foreground"))
            {
                if(item.gameObject.activeSelf)
                {
                    active = item;
                }
            }
        }

        return active;
    }
    internal override List<Button> GetButtons(Transform layoutTransform)
    {
        List<Button> buttonsOnLayout = new List<Button>();
        buttonsOnLayout.AddRange(layoutTransform.GetComponentsInChildren<Button>());
        buttonsInternalPuroposeVar = new List<Button>(buttonsOnLayout);
        return buttonsOnLayout;
    }
    internal override void SetButtonsBehaviour(List<Button> buttons)
    {
        List<Button> buttonsOnLayout = new List<Button>(buttons);

        //Hardcode part
        foreach (var item in buttonsOnLayout)
        {
            item.onClick.RemoveAllListeners();
            item.onClick
                .AddListener(
                                delegate
                                {
                                    ButtonClicked(item.name);
                                });

        }
    }
    private void ButtonClicked(string name)
    {
        switch (name)
        {
            case "ZeroButton":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
            case "OneButton":
                AddSymbolToExpressionBasedOnLayout("1");
                break;
            case "TwoButton":
                AddSymbolToExpressionBasedOnLayout("2");
                break;
            case "ThreeButton":
                AddSymbolToExpressionBasedOnLayout("3");
                break;
            case "FourButton":
                AddSymbolToExpressionBasedOnLayout("4");
                break;
            case "FiveButton":
                AddSymbolToExpressionBasedOnLayout("5");
                break;
            case "SixButton":
                AddSymbolToExpressionBasedOnLayout("6");
                break;
            case "SevenButton":
                AddSymbolToExpressionBasedOnLayout("7");
                break;
            case "EightButton":
                AddSymbolToExpressionBasedOnLayout("8");
                break;
            case "NineButton":
                AddSymbolToExpressionBasedOnLayout("9");
                break;
            case "DotButton":
                AddSymbolToExpressionBasedOnLayout(".");
                break;
            case "CEButton":
                //Replace last part of expression to 0
                ZeroOutLastPartOfExpression();
                break;
            case "RemoveButton":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
            case "PercentButton":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
            case "CButton":
                //Replace all expression to 0
                Expression.Replace(Expression, "0");
                break;
            case "FractionButton":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
            case "SqPowButton":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
            case "SqrtButton":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
            case "DivideButton":
                AddOperationToExpression("/");
                break;
            case "MultiplyButton":
                AddOperationToExpression("*");
                break;
            case "MinusButton":
                AddOperationToExpression("-");
                break;
            case "PlusButton":
                Debug.Log("+");
                AddOperationToExpression("+");
                break;
            case "EqualsButton":
                var result = EvaluateExpression();
                Expression = result;
                break;
            case "PlusMinusButton":
                Expression = PlusMinusProcessing();
                Debug.Log("Out");
                break;
            case "Expression":
                AddSymbolToExpressionBasedOnLayout("0");
                break;
        }
    }

    /// <summary>
    /// VolumeFrom Dropdown options:
    /// 1 - Cubic cm
    /// 2 - Litres
    /// 3 - Pints
    /// 
    /// VolumeTo Dropdown options:
    /// 1 - Pints
    /// 2 - Litres
    /// 3 - Cubic cm
    /// </summary>
    /// <param name="volumeFromDropDown"></param>
    /// <param name="volumeToDropDown"></param>
    /// <returns></returns>
    private string ConvertFromTo(Dropdown volumeFromDropDown, Dropdown volumeToDropDown)
    {
        int optionSelectedVolumeFrom = 0;
        int optionSelectedVolumeTo = 0;
        float result = 0;
        float expressionPart = 0;
        optionSelectedVolumeFrom = GetSelectedOption(volumeFromDropDown);
        optionSelectedVolumeTo = GetSelectedOption(volumeToDropDown);
        if(float.TryParse(Expression, out expressionPart))
        {
            Debug.Log("Expression: " + expressionPart);
            Debug.Log("optionSelectedVolumeTo: " + optionSelectedVolumeTo);
            Debug.Log("optionSelectedVolumeFrom: " + optionSelectedVolumeFrom);
            //Cubic cm
            if (optionSelectedVolumeFrom.Equals(0))
            {
                switch (optionSelectedVolumeTo)
                {
                    case 0:
                        result = expressionPart*CubicCmToPints;
                        break;
                    case 1:
                        result = expressionPart;
                        break;
                    case 2:
                        result = expressionPart * CubicCmToLitres;
                        break;
                    default:
                        result = 0.0f;
                        break;
                }
            }
            //Litres
            else if(optionSelectedVolumeFrom.Equals(1))
            {
                switch (optionSelectedVolumeTo)
                {
                    case 0:
                        result = expressionPart / PintsToLitres;
                        Debug.Log(result);
                        break;
                    case 1:
                        result = expressionPart / CubicCmToLitres;
                        break;
                    case 2:
                        result = expressionPart;
                        break;
                    default:
                        result = 0.0f;
                        break;
                }
            }
            //Pints
            else if(optionSelectedVolumeFrom.Equals(2))
            {
                switch (optionSelectedVolumeTo)
                {
                    case 0:
                        result = expressionPart;
                        break;
                    case 1:
                        result = expressionPart / CubicCmToPints;
                        break;
                    case 2:
                        result = expressionPart * PintsToLitres;
                        break;
                    default:
                        result = 0.0f;
                        break;
                }
            }
        }

        return result.ToString();
    }


    private int GetSelectedOption(Dropdown targetDropdown)
    {
        int tmp = 0;
        targetDropdown.onValueChanged.RemoveAllListeners();
        targetDropdown.onValueChanged
            .AddListener(
                            delegate
                            {
                                DropDownValueHandler(targetDropdown);
                            });

        return optionSelected;
    }

    private void DropDownValueHandler(Dropdown target)
    {
        Debug.Log("Selected: " + target.value);
        optionSelected = target.value;
    }

    private Dropdown GetDropDownBasedOnName(string name)
    {
        return ActiveLayoutTransform.GetComponentsInChildren<Transform>().Where(x => x.name.Equals(name)).First().GetComponentInChildren<Dropdown>();
    }

    private TMPro.TextMeshProUGUI GetTMPComponent(string name)
    {
        return ActiveLayoutTransform.GetComponentsInChildren<Transform>().Where(x => x.name.Equals(name)).First().GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }




    private void OnDisable()
    {
        foreach (var btn in buttonsInternalPuroposeVar)
        {
            btn.onClick.RemoveAllListeners();
        }
    }


    private string PlusMinusProcessing()
    {
        string[] splitted;
        char operation;
        SplitExpression(out splitted, out operation);

        if(splitted.Length > 1)
        {
            splitted[1] = PlusMinusValidation(splitted[1]);
            return $"{splitted[0]}{operation}{splitted[1]}";
        }
        splitted[0] = PlusMinusValidation(splitted[0]);
        return $"{splitted[0]}{operation}";
    }

    private string EvaluateExpression()
    {
        string[] splitted;
        char operation;
        SplitExpression(out splitted, out operation);

        float firstPart = 0.0f;
        float secondPart = 0.0f;

        if (!string.IsNullOrEmpty(splitted[0]))
        {
            if (!float.TryParse(splitted[0], out firstPart))
            {
                return "Unable to resolve input of first part!";
            }
        }
        if (splitted.Length > 1)
        {
            if (!float.TryParse(splitted[1], out secondPart))
            {
                return "Unable to resolve input of second part!";
            }
        }

        float result = ResolveOperation(firstPart, secondPart, operation);
        return $"{result}";
    }

    private void SplitExpression(out string[] splitted, out char operation)
    {
        splitted = Regex.Split(Expression, RegexPattern);
        operation = Regex.Match(Expression, RegexPattern).Value.FirstOrDefault();
        foreach (var item in splitted)
        {
            Debug.Log($"Splitted: {item}\n");
        }
    }

    private float ResolveOperation(float firstPart, float secondPart, char operation)
    {
        switch (operation)
        {
            case '+':
                return firstPart + secondPart;
            case '-':
                return firstPart - secondPart;
            case '*':
                return firstPart * secondPart;
            case '/':
                return firstPart / secondPart;
            default:
                return 0.0f;
        }
    }

    private void ZeroOutLastPartOfExpression()
    {
        char operation = Regex.Match(Expression, RegexPattern).Value.FirstOrDefault();
        string[] splittedExpression = Expression.Split(operation);

        if (splittedExpression.Length == 1)
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

    private void AddSymbolToExpressionBasedOnLayout(string input)
    {
        if (!ActiveLayoutTransform.tag.Equals("StandartCalc"))
        {
            AddSymbolForConverter(input);
            return;
        }
        AddSymbolForStandartCalc(input);
    }

    private void AddSymbolForConverter(string input)
    {
        if(ActiveLayoutTransform.tag.Equals("VolumeConverter"))
        {
            AddSymbolToVolumeConverter(input);
        }
        else
        {
            AddSymbolToLengthConverter(input);
        }
    }

    private void AddSymbolToVolumeConverter(string input)
    {
        if (!Regex.IsMatch(Expression, @"[a-zA-Z!]+"))
        {
            if (Expression.Equals("0"))
            {
                Expression = $"{input}";
            }
            else
            {
                Expression = $"{Expression}{input}";
            }
        }
    }

    private void AddSymbolToLengthConverter(string input)
    {
        throw new NotImplementedException();
    }

    private void AddSymbolForStandartCalc(string input)
    {
        Debug.Log("Regex: " + Regex.IsMatch(Expression, @"[a-zA-Z!]+"));
        if (!Regex.IsMatch(Expression, @"[a-zA-Z!]+"))
        {
            if (Expression.Equals("0"))
            {
                if (!Regex.IsMatch(input, RegexPattern))
                {
                    Expression = $"{input}";
                    return;
                }
            }
            Expression = $"{Expression}{input}";
        }
    }

    private string PlusMinusValidation(string valStr)
    {
        if (valStr.Contains('-'))
        {
            return valStr;
        }
        return $"-{valStr}";
    }

    private void AddOperationToExpression(string input)
    {
        if (!Regex.IsMatch(Expression, RegexPattern))
        {
            Expression = $"{Expression}{input}";
        }
        else
        {
            return;
        }
    }
}
