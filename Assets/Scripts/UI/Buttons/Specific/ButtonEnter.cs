﻿using MathLib;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonEnter : AbstractButton
{
    public ButtonEnter(UnityButton unityButton) : base(unityButton) {}

    public override void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand)
        => SetEnabled(!leftOperand.IsNaN || !rightOperand.IsNaN);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a, opc.InputEmpty);
}
