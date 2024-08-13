using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

[UxmlElement]
public partial class NumberDialog : VisualElement
{
    private List<Button> itemButtons = new List<Button>();

    public string SelectedItem { get; private set; }

    public event System.Action<string, bool> ItemSelected;

    private const string CancelString = "Cancel";

    public NumberDialog()
    {
        // Set the dialog background color with transparency
        style.backgroundColor = new StyleColor(new Color(2 / 255f, 0 / 255f, 47 / 255f, 0.7f));

        // Apply default padding and alignments
        style.paddingTop = 20;
        style.paddingBottom = 20;
        style.paddingLeft = 20;
        style.paddingRight = 20;

        // Set default items
        SetItems(new[] { "1000", "2000", "3000", "4000" });
    }

    // Method to populate the dialog with a list of items
    public void SetItems(IEnumerable<string> items)
    {
        itemButtons.Clear();
        Clear(); // Clear any previous children

        Label label = new Label("Copy item to stack");
        label.style.maxHeight = 80;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        Add(label);


        // Dynamically create and add a button for each item
        foreach (string item in items.Append(CancelString))
        {
            var button = new Button(() => OnItemButtonClicked(item, item.Equals(CancelString)))
            {
                text = item
            };
            ConfigureButtonStyles(button);
            Add(button);
            itemButtons.Add(button);
        }

    }

    private void ConfigureButtonStyles(Button button)
    {
        // Let the button size itself based on its content
        button.style.flexGrow = 1;
        button.style.flexShrink = 0;
        button.style.marginTop = 10;
        button.style.marginBottom = 10;
        button.style.backgroundColor = new StyleColor(new Color(0.05f, 0.05f, 0.05f)); // Almost black
        button.style.color = new StyleColor(Color.yellow); // Fully yellow text
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        button.style.fontSize = 40; // Set a larger font size for visibility

        // Simulate rounded corners using individual radius properties
        button.style.borderTopLeftRadius = 10;
        button.style.borderTopRightRadius = 10;
        button.style.borderBottomLeftRadius = 10;
        button.style.borderBottomRightRadius = 10;

        // Center the text horizontally and vertically
        button.style.justifyContent = Justify.Center;
        button.style.alignItems = Align.Center;
        button.style.paddingLeft = 20;
        button.style.paddingRight = 20;
        button.style.maxHeight = 80;
    }

    private void OnItemButtonClicked(string clickedString, bool cancelled)
    {
        SelectedItem = clickedString;
        Hide(); // Automatically hide the dialog when an item is selected
        ItemSelected?.Invoke(SelectedItem, SelectedItem.Equals(CancelString));
    }

    public void Show() => style.display = DisplayStyle.Flex;

    public void Hide() => style.display = DisplayStyle.None;

}
