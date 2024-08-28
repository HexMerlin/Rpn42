using UnityButton = UnityEngine.UIElements.Button;

public class ButtonSum : ButtonBase
{
    public ButtonSum(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsInvalid && !rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a + b);
}
