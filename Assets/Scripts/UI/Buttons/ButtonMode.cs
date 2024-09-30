using MathLib;
using MathLib.Prime;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonMode : AbstractButton
{
    public Mode NumberMode {  get; }

    public bool RequirePrimes { get; }

    public ButtonMode(UnityButton unityButton, Mode numberMode, bool requirePrimes) : base(unityButton)
    {
        this.NumberMode = numberMode;
        this.RequirePrimes = requirePrimes;
        if (requirePrimes)
            this.DisabledText = "Pending…";
    }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(!this.RequirePrimes || Primes.IsReady);
        if (IsEnabled)
            SetSelected(this.NumberMode == opc.NumberFormat.Mode);
    }

    public override void Execute(OperationController opc)
    {
        opc.NumberMode = this.NumberMode;
    }
}

