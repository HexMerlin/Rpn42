using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonSum : AbstractButton
{
    public ButtonSum(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN && !rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformBinaryOperation((a, b) => a + b);
}
