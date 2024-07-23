using System;
using System.Diagnostics;
using UnityEngine.UIElements;


public class CalcButton : IEquatable<CalcButton>
{
    private const string selectedKeyword = "selected";


    public string Name { get; }

    public UnityEngine.UIElements.Button UnityButton { get; }

    public CalcButton(string name, VisualElement buttonGrid)
    {
        this.Name = name;
        this.UnityButton = buttonGrid.Q<Button>(name);
        Debug.Assert(UnityButton != null);
    }

    public void SetSelected(bool selected)
    {
        if (selected) UnityButton.AddToClassList(selectedKeyword);
        else UnityButton.RemoveFromClassList(selectedKeyword);
    }

    public bool Equals(CalcButton other) => Name == other.Name;  

    public override int GetHashCode() => Name.GetHashCode();
 
}


