using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonMod : ButtonBase
{
    public ButtonMod(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsInvalid && !rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a % b);
}