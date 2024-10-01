using MathLib;
using System.Numerics;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonDivMersenne : AbstractButton
{
    public ButtonDivMersenne(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(ModelController mc) => mc.PerformUnaryOperation((a) => QExtensions.DivideByNextMersenneNumber(a, mustBeCoprime: false));


  
}