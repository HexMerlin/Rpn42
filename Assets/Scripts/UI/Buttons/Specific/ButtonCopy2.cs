using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonCopy2 : AbstractButton
{
    public ButtonCopy2(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN && !rightOperand.IsNaN);

    public override void Execute(OperationController opc) => opc.PerformCopy2();
}