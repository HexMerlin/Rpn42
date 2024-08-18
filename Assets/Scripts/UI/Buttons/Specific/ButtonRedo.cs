using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRedo : ButtonBase
{
    public ButtonRedo(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(opc.CurrentChange.Next is not null);

    public override void Execute(OperationController opc) => opc.PerformRedo();
}