using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonReciprocal : AbstractButton
{
    public ButtonReciprocal(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a.Reciprocal);
}