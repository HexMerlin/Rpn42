using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonUndo : AbstractButton
{
    public ButtonUndo(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(mc.CurrentChange is not NoChange);

    public override void Execute(ModelController mc) => mc.PerformUndo();
}
