using System;
using System.Diagnostics;
using UnityEngine.UIElements;


public class CalcButton : IEquatable<CalcButton>
{
    private const string selectedKeyword = "selected";
    private const string modeButtonClass = "mode-button";

    public string Name { get; }

    public string Text { get; }

    public string DisabledText { get; set; }

    public UnityEngine.UIElements.Button UnityButton { get; }

    public CalcButton(string name, VisualElement buttonGrid)
    {
        this.Name = name;
        this.UnityButton = buttonGrid.Q<Button>(name);
        this.Text = UnityButton.text;
        this.DisabledText = this.Text;
        Debug.Assert(UnityButton != null);
    }

    public void SetEnabled(bool enabled)
    {
        UnityButton.SetEnabled(enabled);
        UnityButton.pickingMode = enabled ? PickingMode.Position : PickingMode.Ignore;
        UnityButton.text = enabled ? Text : DisabledText;
    }

    public void SetSelected(bool selected)
    {
        if (selected) UnityButton.AddToClassList(selectedKeyword);
        else UnityButton.RemoveFromClassList(selectedKeyword);
    }

    public bool IsModeButton => UnityButton.ClassListContains(modeButtonClass);

    public bool Equals(CalcButton other) => Name == other.Name;  

    public override int GetHashCode() => Name.GetHashCode();
 
}


