﻿using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonRepetendShiftLeft : AbstractButton
{
    public ButtonRepetendShiftLeft(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!rightOperand.IsNaN);

    public override void Execute(OperationController opc) { } // TODO Fix this //=> opc.PerformUnaryOperation((a) => a.NumeralSystem.RepetendShiftLeft());
}
