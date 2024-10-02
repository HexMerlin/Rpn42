using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRadixPoint : AbstractButton
{
    public ButtonRadixPoint(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(!mc.InputBuffer.ContainsRadixPoint());
    }

    public override void Execute(ModelController mc) => mc.PerformAddInput(".");
    
}


