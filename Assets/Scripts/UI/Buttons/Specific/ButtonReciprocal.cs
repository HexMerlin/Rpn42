using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonReciprocal : AbstractButton
{
    public ButtonReciprocal(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => a.Reciprocal);
}