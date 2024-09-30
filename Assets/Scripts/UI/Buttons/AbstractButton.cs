using System;
using MathLib;
using UnityEngine.UIElements;
using UnityButton = UnityEngine.UIElements.Button;

public abstract class AbstractButton : IEquatable<AbstractButton>
{
    private const string selectedKeyword = "selected";

    public string Name => UnityButton.name;

    public string Text { get; }

    public string DisabledText { get; set; }

    public UnityButton UnityButton { get; }

    public AbstractButton(UnityButton unityButton)
    {
        this.UnityButton = unityButton;
        this.Text = unityButton.text;
        this.DisabledText = this.Text;
        unityButton.userData = this;
    }

    public static AbstractButton Button(UnityButton unityButton) => unityButton.userData as AbstractButton;

    public bool IsEnabled => UnityButton.enabledSelf;


    public abstract void UpdateEnabledStatus(OperationController opc, Q leftOperand, Q rightOperand);

    public abstract void Execute(OperationController opc);

    protected void SetEnabled(bool enabled)
    {
        if (enabled == IsEnabled) return;
        UnityButton.SetEnabled(enabled);
        UnityButton.pickingMode = enabled ? PickingMode.Position : PickingMode.Ignore;
        UnityButton.text = enabled ? Text : DisabledText;
    }

    protected void SetSelected(bool selected)
    {
        if (selected) UnityButton.AddToClassList(selectedKeyword);
        else UnityButton.RemoveFromClassList(selectedKeyword);
    }

    public bool Equals(AbstractButton other) => Name == other.Name;  

    public override int GetHashCode() => Name.GetHashCode();
   
 
}


