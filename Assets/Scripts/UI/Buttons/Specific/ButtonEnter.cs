
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonEnter : ButtonBase
{
    public ButtonEnter(UnityButton unityButton) : base(unityButton) {}

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(!leftOperand.IsInvalid || !rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a, opc.InputEmpty);
}
