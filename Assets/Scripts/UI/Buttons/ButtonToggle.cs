using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonToggle : AbstractButton
{
    //public int Digit { get; }

    public ButtonToggle(UnityButton unityButton) : base(unityButton)
    {
        this.UnityButton.userData = "kalle";
    }

    public override void Execute(OperationController opc) => throw new System.NotImplementedException();
    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand) => throw new System.NotImplementedException();

    //public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    //{
    //    SetEnabled(Digit < opc.NumberBase);
    //}

    //public override void Execute(OperationController opc) => opc.PerformAddInput(Digit.ToString());
}

