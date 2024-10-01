using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonToggle : AbstractButton
{
    public bool State { get; private set; }

    public string AltText { get; }

    public ButtonToggle(UnityButton unityButton, string altText) : base(unityButton)
    {
        this.AltText = altText;
        this.State = true;
    }

    public override void Execute(ModelController mc) => throw new System.NotImplementedException();
    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand) => throw new System.NotImplementedException();

    //public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    //{
    //    SetEnabled(Digit < opc.NumberBase);
    //}

    //public override void Execute(OperationController opc) => opc.PerformAddInput(Digit.ToString());
}

