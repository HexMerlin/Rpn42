#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using MathLib;
using UnityEngine.UIElements;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonCollection : IEnumerable<ButtonBase>
{

    private readonly IReadOnlyList<ButtonBase> buttons;

    public ButtonCollection(VisualElement buttonGrid)
    {
        UnityButton UnityButton(string buttonName) => buttonGrid.Q<UnityButton>(buttonName) ?? throw new ArgumentNullException($"Cannot find a button {buttonName}");

        this.buttons = new ButtonBase[] {

            new ButtonDigit(UnityButton("button-0")),
            new ButtonDigit(UnityButton("button-1")),
            new ButtonDigit(UnityButton("button-2")),
            new ButtonDigit(UnityButton("button-3")),
            new ButtonDigit(UnityButton("button-4")),
            new ButtonDigit(UnityButton("button-5")),
            new ButtonDigit(UnityButton("button-6")),
            new ButtonDigit(UnityButton("button-7")),
            new ButtonDigit(UnityButton("button-8")),
            new ButtonDigit(UnityButton("button-9")),
            new ButtonEnter(UnityButton("button-enter")),
            new ButtonQuotient(UnityButton("button-quotient")),
            new ButtonBackDrop(UnityButton("button-back-drop")),
            new ButtonCopy2(UnityButton("button-copy-2")),
            new ButtonNeg(UnityButton("button-neg")),
            new ButtonReciprocal(UnityButton("button-reciprocal")),
            new ButtonSquare(UnityButton("button-square")),
            new ButtonPow(UnityButton("button-pow")),
            new ButtonSum(UnityButton("button-sum")),
            new ButtonDiff(UnityButton("button-diff")),
            new ButtonProd(UnityButton("button-prod")),
            new ButtonClear(UnityButton("button-clear")),
            new ButtonMod(UnityButton("button-mod")),
            new ButtonDivMersenne(UnityButton("button-div-ones")),
            new ButtonUndo(UnityButton("button-undo")),
            new ButtonRedo(UnityButton("button-redo")),
            new ButtonRepFactor(UnityButton("button-rep-factor")),
            new ButtonAsRepetend(UnityButton("button-as-repetend")),
            new ButtonAsBalanced(UnityButton("button-as-bal")),
            new ButtonRepetendShiftLeft(UnityButton("button-rep-shift-left")),
            new ButtonRepetendShiftRight(UnityButton("button-rep-shift-right")),

            new ButtonNumberFormatMode(UnityButton("button-format-normal"), Format.Normal, false),
            new ButtonNumberFormatMode(UnityButton("button-format-bin"), Format.Bin, false),
            new ButtonNumberFormatMode(UnityButton("button-format-repetend"), Format.Repetend, true),
            new ButtonNumberFormatMode(UnityButton("button-format-rotbin"), Format.RotationsBin, false),
            new ButtonNumberFormatMode(UnityButton("button-format-factor"), Format.Factor, true),
            new ButtonNumberFormatMode(UnityButton("button-format-period"), Format.Period, true),
            new ButtonNumberFormatMode(UnityButton("button-format-partition"), Format.Partition, false), 
        };

    }

    public void UpdateButtons(OperationController opc)
    {
        (Q leftOperand, Q rightOperand) = opc.PeekOperands();

        foreach (ButtonBase button in buttons)
            button.UpdateEnabledStatus(opc, leftOperand, rightOperand);
    }

    public IEnumerator<ButtonBase> GetEnumerator() => this.buttons.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}



