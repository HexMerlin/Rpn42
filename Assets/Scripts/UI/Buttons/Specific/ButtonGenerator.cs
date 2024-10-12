using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonGenerator : AbstractButton
{
    private readonly bool replaceOperand;
    public ButtonGenerator(UnityButton unityButton, bool replaceOperand) : base(unityButton) 
    {
        this.replaceOperand = replaceOperand;
    }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => a.PadicGenerator(mc.OutputBase), replaceOperand: replaceOperand);
}
