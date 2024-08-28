using UnityEngine.Rendering;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonNumberFormatMode : ButtonBase
{
    public Format NumberFormat {  get; }

    public bool RequirePrimes { get; }

    public ButtonNumberFormatMode(UnityButton unityButton, Format numberFormat, bool requirePrimes) : base(unityButton)
    {
        this.NumberFormat = numberFormat;
        this.RequirePrimes = requirePrimes;
        this.DisabledText = "Pending…";
    }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(!this.RequirePrimes || Primes.IsReady);
        if (IsEnabled)
            SetSelected(this.NumberFormat == opc.NumberFormat);
    }

    public override void Execute(OperationController opc) => opc.NumberFormat = this.NumberFormat;
}

