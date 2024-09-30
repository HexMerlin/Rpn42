﻿using MathLib;
using MathLib.Prime;
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

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
    {
        if (IsEnabled)
            SetSelected(this.Base == opc.NumberFormat.Base);
    }

    public override void Execute(OperationController opc) => opc.NumberBase = this.Base;
}

