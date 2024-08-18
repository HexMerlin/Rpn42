
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonEnter : CalcButton
{
    public ButtonEnter(UnityButton unityButton) : base(unityButton)
    {
    }

    public bool AllowEnable(Rational leftOperand, Rational rightOperand) => !(leftOperand.IsInvalid && rightOperand.IsInvalid);

    public void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a, opc.InputEmpty);
}
    
