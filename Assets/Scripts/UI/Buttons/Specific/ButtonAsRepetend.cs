﻿using UnityButton = UnityEngine.UIElements.Button;

public class ButtonAsRepetend : ButtonBase
{
    public ButtonAsRepetend(UnityButton unityButton) : base(unityButton) { }

    public override void UpdateEnabledStatus(OperationController opc, Rational leftOperand, Rational rightOperand)
        => SetEnabled(!rightOperand.IsInvalid);

    public override void Execute(OperationController opc) => opc.PerformUnaryOperation((a) => a.DivideByNextMersenneNumber(mustBeCoprime: true));
}