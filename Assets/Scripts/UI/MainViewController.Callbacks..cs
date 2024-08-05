
using System;
using UnityEngine;
using UnityEngine.UIElements;

public partial class MainViewController
{

    void OnAwake()
    {

    }
    void OnEnable()
    {
        const string outputElementName = "output";
        const string inputElementName = "input";
        const string buttonGridName = "button-grid";

        VisualElement root = UIDocument.rootVisualElement;

        this.buttonGrid = root.Q<VisualElement>(buttonGridName) ?? throw new NullReferenceException($"{buttonGridName} not found in the UIDocument.");

        this.OperationController = new(buttonGrid);

        this.inputLabel = root.Q<Label>(inputElementName) ?? throw new NullReferenceException($"{inputElementName} not found in the UIDocument.");

        this.output = root.Q<MultiColumnListView>(outputElementName) ?? throw new NullReferenceException("output not found in the UIDocument.");
        this.output.itemsSource = (System.Collections.IList)OperationController.OutputEntries;

        Debug.Assert(output.columns.Count == 3, $"Expected column count 3, but actual was {output.columns.Count}");
        
        static VisualElement makeCell() => new Label();
        for (int columnIndex = 0; columnIndex < 3; columnIndex++)
        {
            output.columns[columnIndex].makeCell = makeCell;

        }

        this.output.columns[0].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(0, this.OperationController.NumberFormat);
        this.output.columns[1].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(1, this.OperationController.NumberFormat);
        this.output.columns[2].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(2, this.OperationController.NumberFormat);

        this.output.makeNoneElement = () => new Label(""); //avoid message "List is empty"

       
        root.RegisterCallbackOnce<KeyDownEvent>(KeyDownEvent =>
        {
            if (KeyDownEvent.keyCode == KeyCode.Escape)
                Application.Quit();

        }, TrickleDown.TrickleDown);
        this.buttonGrid.RegisterCallback<ClickEvent>(OnButtonGridClick);
        this.buttonGrid.RegisterCallback<GeometryChangedEvent>(OnButtonGridGeometryChanged);
 
        LoadSavedData();
        RefreshGUI();
        

    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // Save data when the app is paused
            SaveData();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // Save data when the app loses focus
            SaveData();
    }

    private void OnApplicationQuit() => SaveData(); // Save data when the app is about to quit

    private void OnButtonGridClick(ClickEvent evt)
    {
        if (evt.target is not Button unityButton)
            return;

        if (OperationController.CalcButtons.TryGetButton(unityButton.name) is not CalcButton button)
        {
            Debug.LogWarning($"Unhandled button: {unityButton.name}");
            return;
        }

        GuiEnable = false;

        OperationController.InputButtonPressed(button);

        RefreshGUI();
        GuiEnable = true;
    }

    private void OnButtonGridGeometryChanged(GeometryChangedEvent evt)
    {
        var height = buttonGrid.resolvedStyle.height;
        var width = buttonGrid.resolvedStyle.width;
        float ratio = 4f;

        if (height * ratio > width)
        {
            buttonGrid.style.height = width / ratio;
        }

    }

    private void OnDisable()
    {
        if (buttonGrid != null)
        {
            buttonGrid.UnregisterCallback<GeometryChangedEvent>(OnButtonGridGeometryChanged);

            buttonGrid.UnregisterCallback<ClickEvent>(OnButtonGridClick);

        }
        if (output != null)
        {
            output.columns[0].bindCell = (e, row) => { };
            output.columns[1].bindCell = (e, row) => { };
            output.columns[2].bindCell = (e, row) => { };
        }
    }
}
