using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonDiff : AbstractButton
{
    public ButtonDiff(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN && !rightOperand.IsNaN);

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a - b);
}