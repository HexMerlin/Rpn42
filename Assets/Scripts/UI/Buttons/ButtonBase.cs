using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

/// <summary>
/// Denotes a button that changes the current number base.
/// </summary>
public class ButtonBase : AbstractButton
{
    public int Base { get; }

    public ButtonBase(UnityButton unityButton, int numberBase) : base(unityButton)
    {
        this.Base = numberBase;
    }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
    {
        if (IsEnabled)
            SetSelected(this.Base == mc.NumberFormat.Base);
    }

    public override void Execute(ModelController mc)
    {
        mc.NumberBase = this.Base;
    }
}

