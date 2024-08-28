using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRedo : ButtonBase
{
    public ButtonRedo(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(opc.CurrentChange.Next is not null);

    public override void Execute(OperationController opc) => opc.PerformRedo();
}