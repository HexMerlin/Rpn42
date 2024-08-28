using UnityButton = UnityEngine.UIElements.Button;

public class ButtonNeg : ButtonBase
{
    public ButtonNeg(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => -a);
}
