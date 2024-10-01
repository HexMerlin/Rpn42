using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonAsRepetend : AbstractButton
{
    public ButtonAsRepetend(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => QExtensions.DivideByNextMersenneNumber(a, mustBeCoprime: true));
}
