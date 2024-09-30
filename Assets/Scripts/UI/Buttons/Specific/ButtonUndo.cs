using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonUndo : AbstractButton
{
    public ButtonUndo(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(opc.CurrentChange is not NoChange);

    public override void Execute(OperationController opc) => opc.PerformUndo();
}
