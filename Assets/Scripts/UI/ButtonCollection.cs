#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using MathLib;
using UnityEngine.UIElements;
using UnityButton = UnityEngine.UIElements.Button;

public class ButtonCollection : IEnumerable<AbstractButton>
{

    private readonly IReadOnlyList<AbstractButton> buttons;

    public ButtonCollection(VisualElement buttonGrid)
    {
        UnityButton UnityButton(string buttonName) => buttonGrid.Q<UnityButton>(buttonName) ?? throw new ArgumentNullException($"Cannot find a button {buttonName}");

        this.buttons = new AbstractButton[] {

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
            new ButtonRepetendShiftLeft(UnityButton("button-rep-shift-left")),
            new ButtonRepetendShiftRight(UnityButton("button-rep-shift-right")),

            new ButtonMode(UnityButton("button-format-normal"), Mode.Normal, false),
            new ButtonMode(UnityButton("button-format-periodic"), Mode.Periodic, false),
            new ButtonMode(UnityButton("button-format-padic"), Mode.PAdic, false),
            new ButtonMode(UnityButton("button-format-rot"), Mode.Rotations, false),
            new ButtonMode(UnityButton("button-format-factor"), Mode.Factorization, true),
            new ButtonMode(UnityButton("button-format-repetend"), Mode.Repetend, true),
            new ButtonMode(UnityButton("button-format-period"), Mode.Period, true),

            new ButtonBase(UnityButton("button-base-2"), 2),
            new ButtonBase(UnityButton("button-base-3"), 3),
            new ButtonBase(UnityButton("button-base-5"), 5),
            new ButtonBase(UnityButton("button-base-10"), 10),
  
        };

    }

    public void UpdateButtons(OperationController opc)
    {
        (Q leftOperand, Q rightOperand) = opc.PeekOperands();

        foreach (AbstractButton button in buttons)
            button.UpdateEnabledStatus(opc, leftOperand, rightOperand);
    }

    public IEnumerator<AbstractButton> GetEnumerator() => this.buttons.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}



