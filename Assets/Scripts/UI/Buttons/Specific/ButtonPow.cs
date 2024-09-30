using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonPow : ButtonBase
{
    public ButtonPow(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN && rightOperand.TryCastToInt32(out int _));

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a.Pow(b));
}