using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

/// <summary>
/// Denotes a button that changes the current number base.
/// </summary>
public class ButtonBase : AbstractButton
{
    public int Base { get; }

    public bool IsInputBase { get; }

    public ButtonBase(UnityButton unityButton, bool isInputBase, int numberBase) : base(unityButton)
    {
        this.IsInputBase = isInputBase;
        this.Base = numberBase;
    }

    public override void UpdateEnabledStatus(ModelController mc, Q leftOperand, Q rightOperand)
    {
        if (IsEnabled)
        {
            SetSelected(this.Base == (this.IsInputBase ? mc.InputBase : mc.OutputBase));    
        }
    }

    public override void Execute(ModelController mc)
    {
        if (this.IsInputBase)
            mc.InputBase = this.Base;
        else
            mc.OutputBase = this.Base;
    }
}

