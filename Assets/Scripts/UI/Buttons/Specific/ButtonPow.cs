using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonPow : AbstractButton
{
    public ButtonPow(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN && rightOperand.TryCastToInt32(out int _));

    public override void Execute(ModelController mc) => mc.PerformBinaryOperation((a, b) => a.Pow(b));
}