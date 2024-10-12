using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonGenerator : AbstractButton
{
    private readonly bool retainOperand;
    public ButtonGenerator(UnityButton unityButton, bool retainOperand) : base(unityButton) 
    {
        this.retainOperand = retainOperand;
    }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => a.PadicGenerator(mc.OutputBase), retainOperand: retainOperand);
}
