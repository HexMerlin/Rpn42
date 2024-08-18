using UnityButton = UnityEngine.UIElements.Button;

public class ButtonPow : ButtonBase
{
    public ButtonPow(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(!leftOperand.IsInvalid && rightOperand.TryCastToInt32(out int _));

    public override void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a.Pow(b));
}