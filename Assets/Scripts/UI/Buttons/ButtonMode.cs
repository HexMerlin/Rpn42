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

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
    {
        SetEnabled(!this.RequirePrimes || Primes.IsReady);
        if (IsEnabled)
            SetSelected(this.NumberMode == mc.NumberMode);
    }

    public override void Execute(ModelController mc)
    {
        mc.NumberMode = this.NumberMode;
    }
}

