using UnityButton = UnityEngine.UIElements.Button;

public class ButtonQuotient : CalcButton
{
    public ButtonQuotient(UnityButton unityButton) : base(unityButton)
    {
    }

    public bool AllowEnable(Rational leftOperand, Rational rightOperand)
    {
        if (leftOperand.IsInvalid || rightOperand.IsInvalid) return false; 
        if (rightOperand.IsZero) return false;
        return true;
    }

    public void Execute(OperationController opc) => opc.PerformBinaryOperation((a, b) => a / b);
}
