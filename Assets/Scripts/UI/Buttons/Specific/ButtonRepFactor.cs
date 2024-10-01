using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRepFactor : AbstractButton
{
    public ButtonRepFactor(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    //TODO: Fix this!
    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => a);// RationalNumerals.FindUnitFractionWithRepetendFactor(a));
}