
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonBackDrop : ButtonBase
{
    public ButtonBackDrop(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(!leftOperand.IsInvalid || !rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformBackDrop();
}
