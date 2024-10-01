using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonQuotient : AbstractButton
{
    public ButtonQuotient(UnityButton unityButton) : base(unityButton) {}

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand) 
        => SetEnabled(!leftOperand.IsNaN && !rightOperand.IsNaN && !rightOperand.IsZero);

    public override void Execute(ModelController mc) => mc.PerformBinaryOperation((a, b) => a / b);
}
