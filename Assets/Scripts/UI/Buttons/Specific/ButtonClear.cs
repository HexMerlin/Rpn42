using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonClear : AbstractButton
{
    public ButtonClear(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand) { }

    public override void Execute(ModelController mc) => mc.PerformClear();
}