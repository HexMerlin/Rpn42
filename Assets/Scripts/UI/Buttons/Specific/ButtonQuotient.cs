using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonQuotient : ButtonBase
{
    public ButtonQuotient(UnityButton unityButton) : base(unityButton) {}

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand) 
        => SetEnabled(!leftOperand.IsNaN && !rightOperand.IsNaN && !rightOperand.IsZero);

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a / b);
}
