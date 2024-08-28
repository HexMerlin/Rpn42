using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonDivMersenne : ButtonBase
{
    public ButtonDivMersenne(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a.DivideByNextMersenneNumber(mustBeCoprime: false));
}