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
    private int optionVolumeSelectedFrom = 0;
    private int optionVolumeSelectedTo = 0;
    private int optionLengthSelectedFrom = 0;
    private int optionLengthSelectedTo = 0;

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
            else if (ActiveLayoutTransform.tag.Equals("VolumeConverter"))
            {
                VolumeFrom.text = Expression;
                var volumeTo = GetTMPComponent("VolumeTo");
                var volumeFromDropDown = GetDropDownBasedOnName("VolumeFromDropDown");
                var volumeToDropDown = GetDropDownBasedOnName("VolumeToDropDown");
                volumeTo.text = ConvertVolumeFromTo(volumeFromDropDown, volumeToDropDown);
            }
            else if (ActiveLayoutTransform.tag.Equals("LengthConverter"))
            {
                LengthFrom.text = Expression;
                var lengthTo = GetTMPComponent("LengthTo");
                var lengthFromDropDown = GetDropDownBasedOnName("LengthFromDropDown");
                var lengthToDropDown = GetDropDownBasedOnName("LengthToDropDown");
                lengthTo.text = ConvertLengthFromTo(lengthFromDropDown, lengthToDropDown);
            }
        }

        SetButtonsBehaviour(GetButtons(ActiveLayoutTransform));
    }


    #region LayoutBehaviour
    private Transform GetCurrentActiveLayout()
    {
        Transform active = default;
        foreach (var item in transform.GetComponentsInChildren<Transform>())
        {
            if (item.name.Contains("Foreground"))
            {
                if (item.gameObject.activeSelf)
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
                Expression = "0";
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

    private void AddSymbolToExpressionBasedOnLayout(string input)
    {
        if (!ActiveLayoutTransform.tag.Equals("StandartCalc"))
        {
            AddSymbolToConverter(input);
            return;
        }
        AddSymbolForStandartCalc(input);
    }
    //private void AddSymbolForConverter(string input)
    //{
    //    if (ActiveLayoutTransform.tag.Equals("VolumeConverter"))
    //    {
    //        AddSymbolToConverter(input);
    //    }
    //    else
    //    {
    //        AddSymbolToLengthConverter(input);
    //    }
    //}

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

    private TMPro.TextMeshProUGUI GetTMPComponent(string name)
    {
        return ActiveLayoutTransform.GetComponentsInChildren<Transform>().Where(x => x.name.Equals(name)).First().GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    private Dropdown GetDropDownBasedOnName(string name)
    {
        return ActiveLayoutTransform.GetComponentsInChildren<Transform>().Where(x => x.name.Equals(name)).First().GetComponentInChildren<Dropdown>();
    }

    private void GetSelectedOption(Dropdown targetDropdown)
    {
        if(GetCurrentActiveLayout().name.Contains("Volume"))
        {
            targetDropdown.onValueChanged.RemoveAllListeners();
            targetDropdown.onValueChanged
                .AddListener(
                                delegate
                                {
                                    VolumeDropDownValueHandler(targetDropdown);
                                });
        }
        if(GetCurrentActiveLayout().name.Contains("Length"))
        {
            targetDropdown.onValueChanged.RemoveAllListeners();
            targetDropdown.onValueChanged
                .AddListener(
                                delegate
                                {
                                    LengthDropDownValueHandler(targetDropdown);
                                });
        }
    }

    private void AddSymbolToConverter(string input)
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
    #endregion

    #region StandartCalc
    private string PlusMinusProcessing()
    {
        string[] splitted;
        char operation;
        SplitExpression(out splitted, out operation);

        if (splitted.Length > 1)
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
    #endregion

    #region VolumeConverter


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
    private string ConvertVolumeFromTo(Dropdown volumeFromDropDown, Dropdown volumeToDropDown)
    {
        float result = 0;
        float expressionPart = 0;
        GetSelectedOption(volumeFromDropDown);
        GetSelectedOption(volumeToDropDown);


        if (float.TryParse(Expression, out expressionPart))
        {
            //Cubic cm
            if (optionVolumeSelectedFrom.Equals(0))
            {
                switch (optionVolumeSelectedTo)
                {
                    case 0:
                        result = expressionPart * GlobalVars.CubicCmToPints;
                        break;
                    case 1:
                        result = expressionPart;
                        break;
                    case 2:
                        result = expressionPart * GlobalVars.CubicCmToLitres;
                        break;
                    default:
                        result = 0.0f;
                        break;
                }
            }
            //Litres
            else if (optionVolumeSelectedFrom.Equals(1))
            {
                switch (optionVolumeSelectedTo)
                {
                    case 0:
                        result = expressionPart / GlobalVars.PintsToLitres;
                        break;
                    case 1:
                        result = expressionPart / GlobalVars.CubicCmToLitres;
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
            else if (optionVolumeSelectedFrom.Equals(2))
            {
                switch (optionVolumeSelectedTo)
                {
                    case 0:
                        result = expressionPart;
                        break;
                    case 1:
                        result = expressionPart / GlobalVars.CubicCmToPints;
                        break;
                    case 2:
                        result = expressionPart * GlobalVars.PintsToLitres;
                        break;
                    default:
                        result = 0.0f;
                        break;
                }
            }
        }

        return result.ToString();
    }

    private void VolumeDropDownValueHandler(Dropdown target)
    {
        if (target.Equals(GetDropDownBasedOnName("VolumeFromDropDown")))
        {
            optionVolumeSelectedFrom = target.value;
        }
        if (target.Equals(GetDropDownBasedOnName("VolumeToDropDown")))
        {
            optionVolumeSelectedTo = target.value;
        }
    }

    #endregion

    #region LengthConverter

    private string ConvertLengthFromTo(Dropdown lengthFromDropDown, Dropdown lengthToDropDown)
    {
        float result = 0;
        float expressionPart = 0;
        GetSelectedOption(lengthFromDropDown);
        GetSelectedOption(lengthToDropDown);

        if(float.TryParse(Expression, out expressionPart))
        {
            //Cm
            if(optionLengthSelectedFrom.Equals(0))
            {
                switch (optionLengthSelectedTo)
                {
                    case 0:
                        result = expressionPart * GlobalVars.CmToInches;
                        break;
                    case 1:
                        result = expressionPart * GlobalVars.CmToMeters;
                        break;
                    case 2:
                        result = expressionPart * GlobalVars.CmToFeet;
                        break;
                    case 3:
                        result = expressionPart * GlobalVars.CmToYard;
                        break;
                    case 4:
                        result = expressionPart;
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
            //Meters
            else if (optionLengthSelectedFrom.Equals(1))
            {
                float metersToCm = expressionPart / GlobalVars.CmToMeters;
                switch (optionLengthSelectedTo)
                {
                    case 0:
                        result = metersToCm * GlobalVars.CmToInches;
                        break;
                    case 1:
                        result = expressionPart;
                        break;
                    case 2:
                        result = metersToCm * GlobalVars.CmToFeet;
                        break;
                    case 3:
                        result = metersToCm * GlobalVars.CmToYard;
                        break;
                    case 4:
                        result = metersToCm;
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
            //Inches
            else if (optionLengthSelectedFrom.Equals(2))
            {
                switch (optionLengthSelectedTo)
                {
                    case 0:
                        result = expressionPart;
                        break;
                    case 1:
                        result = (expressionPart / GlobalVars.CmToInches) * GlobalVars.CmToMeters;
                        break;
                    case 2:
                        result = expressionPart * GlobalVars.InchesToFeet;
                        break;
                    case 3:
                        result = expressionPart * GlobalVars.InchesToYard;
                        break;
                    case 4:
                        result = expressionPart / GlobalVars.CmToInches;
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
            //Feet
            else if (optionLengthSelectedFrom.Equals(3))
            {
                switch (optionLengthSelectedTo)
                {
                    case 0:
                        result = expressionPart / GlobalVars.InchesToFeet;
                        break;
                    case 1:
                        result = (expressionPart / GlobalVars.CmToFeet) * GlobalVars.CmToMeters;
                        break;
                    case 2:
                        result = expressionPart;
                        break;
                    case 3:
                        result = expressionPart * GlobalVars.FeetToYard;
                        break;
                    case 4:
                        result = expressionPart / GlobalVars.CmToFeet;
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
            //Yards
            else if (optionLengthSelectedFrom.Equals(4))
            {
                switch (optionLengthSelectedTo)
                {
                    case 0:
                        result = expressionPart / GlobalVars.InchesToYard;
                        break;
                    case 1:
                        result = (expressionPart / GlobalVars.CmToYard) * GlobalVars.CmToMeters;
                        break;
                    case 2:
                        result = expressionPart / GlobalVars.FeetToYard;
                        break;
                    case 3:
                        result = expressionPart;
                        break;
                    case 4:
                        result = expressionPart / GlobalVars.CmToYard;
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
        }

        return result.ToString();
    }

    private void LengthDropDownValueHandler(Dropdown target)
    {
        if (target.Equals(GetDropDownBasedOnName("LengthFromDropDown")))
        {
            optionLengthSelectedFrom = target.value;
        }
        if (target.Equals(GetDropDownBasedOnName("LengthToDropDown")))
        {
            optionLengthSelectedTo = target.value;
        }
    }

    #endregion
    private void OnDisable()
    {
        foreach (var btn in buttonsInternalPuroposeVar)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}