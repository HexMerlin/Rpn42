using UnityButton = UnityEngine.UIElements.Button;

public class ButtonSquare : ButtonBase
{
    public ButtonSquare(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(!rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a.Square());
}

