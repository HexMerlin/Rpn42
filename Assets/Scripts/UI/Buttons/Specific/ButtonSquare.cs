using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonSquare : AbstractButton
{
    public ButtonSquare(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => a.Square());
}

