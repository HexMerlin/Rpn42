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

            new ButtonDigit(UnityButton("button-0"), 0),
            new ButtonDigit(UnityButton("button-1"), 1),
            new ButtonDigit(UnityButton("button-2"), 2),
            new ButtonDigit(UnityButton("button-3"), 3),
            new ButtonDigit(UnityButton("button-4"), 4),
            new ButtonDigit(UnityButton("button-5"), 5),
            new ButtonDigit(UnityButton("button-6"), 6),
            new ButtonDigit(UnityButton("button-7"), 7),
            new ButtonDigit(UnityButton("button-8"), 8),
            new ButtonDigit(UnityButton("button-9"), 9),
            new ButtonRadixPoint(UnityButton("button-radix-point")),
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
            new ButtonGenerator(UnityButton("button-generator-retain"), retainOperand: true),
            new ButtonGenerator(UnityButton("button-generator"), retainOperand: false),
            new ButtonDivMersenne(UnityButton("button-div-ones")),
            new ButtonUndo(UnityButton("button-undo")),
            new ButtonRedo(UnityButton("button-redo")),
            new ButtonRepFactor(UnityButton("button-rep-factor")),
            new ButtonAsRepetend(UnityButton("button-as-repetend")),
            new ButtonRepetendShiftLeft(UnityButton("button-rep-shift-left")),
            new ButtonRepetendShiftRight(UnityButton("button-rep-shift-right")),

            new ButtonMode(UnityButton("button-mode-normal"), Mode.Normal, false),
            new ButtonMode(UnityButton("button-mode-periodic"), Mode.Periodic, false),
            new ButtonMode(UnityButton("button-mode-rot"), Mode.Rotations, false),
            new ButtonMode(UnityButton("button-mode-factor"), Mode.Factorization, true),
            new ButtonMode(UnityButton("button-mode-repetend"), Mode.Repetend, true),
            new ButtonMode(UnityButton("button-mode-period"), Mode.Period, true),

            new ButtonBase(UnityButton("button-out-base-2"), isInputBase: false, 2),
            new ButtonBase(UnityButton("button-out-base-3"), isInputBase: false, 3),
            new ButtonBase(UnityButton("button-out-base-5"), isInputBase: false, 5),
            new ButtonBase(UnityButton("button-out-base-7"), isInputBase: false, 7),
            new ButtonBase(UnityButton("button-out-base-10"), isInputBase: false, 10),

            new ButtonBase(UnityButton("button-in-base-2"), isInputBase: true, 2),
            new ButtonBase(UnityButton("button-in-base-3"), isInputBase: true, 3),
            new ButtonBase(UnityButton("button-in-base-5"), isInputBase: true, 5),
            new ButtonBase(UnityButton("button-in-base-7"), isInputBase: true, 7),
            new ButtonBase(UnityButton("button-in-base-10"), isInputBase: true, 10),
  
        };

    }

    public void UpdateButtons(ModelController mc)
    {
        (Q leftOperand, Q rightOperand) = mc.PeekOperands();

        foreach (AbstractButton button in buttons)
            button.UpdateEnabledStatus(mc, leftOperand, rightOperand);
    }

    public IEnumerator<AbstractButton> GetEnumerator() => this.buttons.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}



