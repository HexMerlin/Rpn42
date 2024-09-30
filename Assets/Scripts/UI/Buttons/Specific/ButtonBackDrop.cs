using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonBackDrop : ButtonBase
{
    public ButtonBackDrop(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN || !rightOperand.IsNaN);

    public override void Execute(OperationController opc) => opc.PerformBackDrop();
}
