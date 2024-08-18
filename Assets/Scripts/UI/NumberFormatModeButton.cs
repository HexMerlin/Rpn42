using System;
using System.Diagnostics;
using UnityEngine.UIElements;
using UnityButton = UnityEngine.UIElements.Button;

public class NumberFormatModeButton : CalcButton
{
    public Format NumberFormat {  get; }

    public NumberFormatModeButton(UnityButton unityButton, Format numberFormat) : base(unityButton)
    {
        this.NumberFormat = numberFormat;
    }
}
