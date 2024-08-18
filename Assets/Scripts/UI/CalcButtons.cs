#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class CalcButtons
{
    //declare all buttons with their UXML names
    public const string Zero = "button-0";
    public const string One = "button-1";
    public const string Two = "button-2";
    public const string Three = "button-3";
    public const string Four = "button-4";
    public const string Five = "button-5";
    public const string Six = "button-6";
    public const string Seven = "button-7";
    public const string Eight = "button-8";
    public const string Nine = "button-9";
    public const string Enter = "button-enter";
    public const string BackDrop = "button-back-drop";
    public const string Copy2 = "button-copy-2";
    public const string Neg = "button-neg";
    public const string Reciprocal = "button-reciprocal";
    public const string Square = "button-square";
    public const string Power = "button-pow";
    public const string Sum = "button-sum";
    public const string Diff = "button-diff";
    public const string Prod = "button-prod";
    public const string Quotient = "button-quotient";
    public const string Clear = "button-clear";
    public const string Mod = "button-mod";
    public const string DivOnes = "button-div-ones";
    public const string Undo = "button-undo";
    public const string Redo = "button-redo";
    public const string RepFactor = "button-rep-factor";
    public const string AsRepetend = "button-as-repetend";
    public const string AsBalanced = "button-as-bal";
    public const string RepShiftLeft = "button-rep-shift-left";
    public const string RepShiftRight = "button-rep-shift-right";
    public const string FormatNormal = "button-format-normal";
    public const string FormatBin = "button-format-bin";
    public const string FormatRepetend = "button-format-repetend";
    public const string FormatRotationsBin = "button-format-rotbin";
    public const string FormatFactor = "button-format-factor";
    public const string FormatPartition = "button-format-partition";
    public const string FormatPeriod = "button-format-period";

    private readonly Dictionary<string, CalcButton> allButtons;

    public readonly (CalcButton Button, Format NumberFormat)[] ModeButtons;

    public readonly CalcButton ButtonFormatRepetend;
    public readonly CalcButton ButtonFormatFactor;
    public readonly CalcButton ButtonFormatPeriod;

    public CalcButtons(VisualElement buttonGrid)
    {
        this.allButtons = new Dictionary<string, CalcButton>();

        AddAll(
            Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Enter,
            BackDrop, Copy2, Neg, Reciprocal, Square, Power, Sum, Diff, Prod, Quotient,
            Clear, Mod, DivOnes, Undo, Redo, AsRepetend, AsBalanced, RepShiftLeft, RepShiftRight, RepFactor,
            FormatNormal, FormatBin, FormatRepetend, FormatRotationsBin, FormatFactor, FormatPeriod, FormatPartition
        );

       
        void AddAll(params string[] buttonNames)
        {
            foreach (string buttonName in buttonNames)
                allButtons.Add(buttonName, new CalcButton(buttonName, buttonGrid));
        }

        this.ModeButtons = new (CalcButton, Format)[]
        {
            (this[FormatNormal], Format.Normal),
            (this[FormatBin], Format.Bin),
            (this[FormatRepetend], Format.Repetend),
            (this[FormatRotationsBin], Format.RotationsBin),
            (this[FormatFactor], Format.Factor),
            (this[FormatPeriod], Format.Period),
            (this[FormatPartition], Format.Partition)
        };

        this.ButtonFormatFactor = this[FormatFactor];
        this.ButtonFormatRepetend = this[FormatRepetend];
        this.ButtonFormatPeriod = this[FormatPeriod];
        this.ButtonFormatFactor.DisabledText = "Pending…";
        this.ButtonFormatRepetend.DisabledText = "Pending…";
        this.ButtonFormatPeriod.DisabledText = "Pending…";
    }

    public CalcButton this[string buttonName] =>
       TryGetButton(buttonName) ?? throw new ArgumentException($"Unknown button '{buttonName}'");

    public CalcButton? TryGetButton(string buttonName) 
        => allButtons.TryGetValue(buttonName, out CalcButton button) ? button : null;

    public Format? IsNumberFormatButton(CalcButton calcButton) 
    {       
        foreach ((CalcButton button, Format numberFormat) in ModeButtons)
        {
            if (button == calcButton)
                return numberFormat;
        }
        return null;
    }

}

