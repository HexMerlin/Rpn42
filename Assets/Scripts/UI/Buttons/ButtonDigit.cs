using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonDigit : AbstractButton
{
    public int Digit { get; }

    public ButtonDigit(UnityButton unityButton, int digit) : base(unityButton) 
    {
        this.Digit = digit;
    }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(Digit < mc.NumberBase);
    }

    public override void Execute(ModelController mc) => mc.PerformAddInput(Digit.ToString());
}

