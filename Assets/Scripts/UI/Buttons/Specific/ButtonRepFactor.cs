using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRepFactor : ButtonBase
{
    public ButtonRepFactor(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(!rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => Rational.FindUnitFractionWithRepetendFactor(a));
}