
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonDigit : ButtonBase
{
    public ButtonDigit(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand) { }

    public override void Execute(OperationController opc) => opc.PerformAddInput(Text); //note: text on digit buttons maps verbatim to input strings
}

