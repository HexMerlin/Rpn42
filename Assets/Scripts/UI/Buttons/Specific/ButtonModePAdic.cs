using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonModePAdic : ButtonMode
{
    public ButtonModePAdic(UnityButton unityButton) : base(unityButton, Mode.PAdic, false) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    {
        base.UpdateEnabledStatus(opc, leftOperand, rightOperand);

        SetEnabled(opc.NumberBase != 10);
    }

}
