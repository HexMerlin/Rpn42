using UnityButton = UnityEngine.UIElements.Button;

public class ButtonClear : ButtonBase
{
    public ButtonClear(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand) { }

    public override void Execute(OperationController opc) => opc.PerformClear();
}