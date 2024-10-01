using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRedo : AbstractButton
{
    public ButtonRedo(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(mc.CurrentChange.Next is not null);

    public override void Execute(ModelController mc) => mc.PerformRedo();
}