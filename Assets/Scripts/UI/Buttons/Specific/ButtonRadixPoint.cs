using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRadixPoint : AbstractButton
{
    public ButtonRadixPoint(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(!opc.Input.Contains('.', System.StringComparison.InvariantCulture));
    }

    public override void Execute(OperationController opc) => opc.PerformAddInput(".");
    
}


