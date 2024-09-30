using MathLib;
using MathLib.Prime;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonDigit : AbstractButton
{
    public int Digit { get; }

    public ButtonDigit(UnityButton unityButton, int digit) : base(unityButton) 
    {
        this.Digit = digit;
    }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(Digit < opc.NumberBase);
    }

    public override void Execute(OperationController opc) => opc.PerformAddInput(Digit.ToString());
}

