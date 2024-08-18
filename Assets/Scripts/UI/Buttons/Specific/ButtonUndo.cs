using UnityButton = UnityEngine.UIElements.Button;

public class ButtonUndo : ButtonBase
{
    public ButtonUndo(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(opc.CurrentChange is not NoChange);

    public override void Execute(OperationController opc) => opc.PerformUndo();
}
