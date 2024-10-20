using MathLib;
using UnityButton = UnityEngine.UIElements.Button;


public class ButtonToTuple : AbstractButton
{
    public ButtonToTuple(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
      => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformToTuple();
}
