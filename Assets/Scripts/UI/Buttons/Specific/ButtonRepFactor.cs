using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRepFactor : ButtonBase
{
    public ButtonRepFactor(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => Q.FindUnitFractionWithRepetendFactor(a));
}