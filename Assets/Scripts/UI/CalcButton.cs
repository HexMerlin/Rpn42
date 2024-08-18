using System;
using UnityEngine.UIElements;
using UnityButton = UnityEngine.UIElements.Button;

public class CalcButton : IEquatable<CalcButton>
{
    private const string selectedKeyword = "selected";
    private const string modeButtonClass = "mode-button";

    public string Name => UnityButton.name;

    public string Text { get; }

    public string DisabledText { get; set; }

    public UnityButton UnityButton { get; }

    public CalcButton(UnityButton unityButton)
    {
        this.UnityButton = unityButton;
        this.Text = UnityButton.text;
        this.DisabledText = this.Text;
    }
    public bool IsEnabled() => UnityButton.enabledSelf;

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


