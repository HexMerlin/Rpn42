using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonBackDrop : AbstractButton
{
    public ButtonBackDrop(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN || !rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformBackDrop();
}
