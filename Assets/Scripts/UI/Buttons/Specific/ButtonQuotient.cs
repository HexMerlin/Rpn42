using UnityButton = UnityEngine.UIElements.Button;

public class ButtonQuotient : ButtonBase
{
    public ButtonQuotient(UnityButton unityButton) : base(unityButton) {}

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand) 
        => SetEnabled(!leftOperand.IsInvalid && !rightOperand.IsInvalid && !rightOperand.IsZero);

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a / b);
}
