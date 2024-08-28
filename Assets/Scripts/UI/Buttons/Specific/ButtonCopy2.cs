using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonCopy2 : ButtonBase
{
    public ButtonCopy2(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsInvalid && !rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformCopy2();
}