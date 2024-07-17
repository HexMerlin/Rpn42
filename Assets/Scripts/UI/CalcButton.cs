using System.Diagnostics;
using UnityEngine.UIElements;


public class CalcButton
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

    public void Select() => UnityButton.AddToClassList(selectedKeyword);

    public void Deselect() => UnityButton.RemoveFromClassList(selectedKeyword);

}


