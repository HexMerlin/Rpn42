using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonClear : AbstractButton
{
    public ButtonClear(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand) { }

    public override void Execute(OperationController opc) => opc.PerformClear();
}