#nullable enable
using System;
using System.Collections.Generic;
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
    public const string Swap = "button-swap";
    public const string Neg = "button-neg";
    public const string Reciprocal = "button-reciprocal";
    public const string Square = "button-square";
    public const string Sum = "button-sum";
    public const string Diff = "button-diff";
    public const string Prod = "button-prod";
    public const string Quotient = "button-quotient";
    public const string Clear = "button-clear";
    public const string Mod = "button-mod";
    public const string DivOnes = "button-div-ones";
    public const string Undo = "button-undo";
    public const string Redo = "button-redo";
    public const string FormatNormal = "button-format-normal";
    public const string FormatBin = "button-format-bin";
    public const string FormatBalBin = "button-format-balbin";
    public const string FormatRotationsBin = "button-format-rotbin";
    public const string FormatRotationsBalBin = "button-format-rotbalbin";
    public const string FormatPartition = "button-format-partition";

    private readonly Dictionary<string, CalcButton> allButtons;

    public CalcButtons(VisualElement buttonGrid)
    {
        this.allButtons = new Dictionary<string, CalcButton>();

        AddAll(
            Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Enter,
            BackDrop, Swap, Neg, Reciprocal, Square, Sum, Diff, Prod, Quotient,
            Clear, Mod, DivOnes, Undo, Redo,
            FormatNormal, FormatBin, FormatBalBin, FormatRotationsBin, FormatRotationsBalBin, FormatPartition
        );

        void AddAll(params string[] buttonNames) {
            foreach (string buttonName in buttonNames)
                allButtons.Add(buttonName, new CalcButton(buttonName, buttonGrid));
        }
    }

    public CalcButton this[string buttonName] =>
       TryGetButton(buttonName) ?? throw new ArgumentException($"Unknown button '{buttonName}'");

    public CalcButton? TryGetButton(string buttonName) 
        => allButtons.TryGetValue(buttonName, out CalcButton button) ? button : null;

    public CalcButton GetNumberFormatButton(Format format) => format switch
    {
        Format.Normal => this[FormatNormal],
        Format.Bin => this[FormatBin],
        Format.BalBin => this[FormatBalBin],
        Format.RotationsBin => this[FormatRotationsBin],
        Format.RotationsBalBin => this[FormatRotationsBalBin],
        Format.Partition => this[FormatPartition],
        _ => throw new ArgumentException($"Button for format {format} not found")
    };


    public Format? IsNumberFormatButton(string buttonName) => buttonName switch
    {
        FormatNormal => Format.Normal,
        FormatBin => Format.Bin,
        FormatBalBin => Format.BalBin,
        FormatRotationsBin => Format.RotationsBin,
        FormatRotationsBalBin => Format.RotationsBalBin,
        FormatPartition => Format.Partition,
        _ => null,
    };

}

