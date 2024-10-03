#nullable enable
using System;
using System.Linq;
using System.Numerics;
using MathLib.Prime;
using UnityEngine;
using UnityEngine.UIElements;
using UnityButton = UnityEngine.UIElements.Button;

public partial class MainViewControl
{

    void Awake()
    {
        T Control<T>(string componentName) where T : VisualElement => uiDocument.rootVisualElement.Q<T>(componentName) ?? throw new NullReferenceException($"{componentName} not found in the UIDocument.");

        const string outputElementName = "output";
        const string inputElementName = "input";
        const string buttonGridName = "button-grid";
        const string numberDialogName = "number-dialog";

        VisualElement root = uiDocument.rootVisualElement;
        this.buttonGrid = Control<VisualElement>(buttonGridName);

        this.Buttons = new ButtonCollection(buttonGrid);

        this.ModelController = new();

        Primes.Prepare(OnPrimesInstanceCompleted); //start preparing the prime factoring capability in the background

        this.numberDialog = Control<NumberDialog>(numberDialogName);

        this.inputLabel = Control<Label>(inputElementName);

        this.output = Control<MultiColumnListView>(outputElementName);

        this.output.itemsSource = (System.Collections.IList)ModelController.OutputEntries;

        Debug.Assert(output.columns.Count == 3, $"Expected column count 3, but actual was {output.columns.Count}");

        static VisualElement makeCell() => new Label();
        for (int columnIndex = 0; columnIndex < output.columns.Count; columnIndex++)
        {
            output.columns[columnIndex].makeCell = makeCell;
        }

        this.output.columns[0].bindCell = (e, row) => ((Label) e).text = ModelController[row].ColumnData(0, this.ModelController.OutputFormat);
        this.output.columns[1].bindCell = (e, row) => ((Label) e).text = ModelController[row].ColumnData(1, this.ModelController.OutputFormat);
        this.output.columns[2].bindCell = (e, row) => ((Label) e).text = ModelController[row].ColumnData(2, this.ModelController.OutputFormat);

        this.output.makeNoneElement = () => new Label(""); //avoid message "List is empty"

        root.RegisterCallbackOnce<KeyDownEvent>(KeyDownEvent =>
        {
            if (KeyDownEvent.keyCode == KeyCode.Escape)
                Application.Quit();

        }, TrickleDown.TrickleDown);
        this.buttonGrid.RegisterCallback<ClickEvent>(OnButtonGridClick);
        this.buttonGrid.RegisterCallback<GeometryChangedEvent>(OnButtonGridGeometryChanged);

       
        this.numberDialog.ItemSelected += OnNumberDialogItemSelected;

        this.output.RegisterCallback<ClickEvent>(OnCellClick);
        
        this.numberDialog.Hide();
        LoadSavedData();
        RequestUIRefresh();

    }

    private void OnButtonGridClick(ClickEvent evt)
    {
        if (evt.target is not UnityButton unityButton)
            return;

        AbstractButton? button = AbstractButton.Button(unityButton);
        if (button is null) return; //button not assigned

        GuiEnable = false;

        button.Execute(ModelController);
        ModelController.SetUndoPoint();

        RequestUIRefresh();
        GuiEnable = true;

    }

    private void OnCellClick(ClickEvent evt)
    {
        if (evt.target is Label cellLabel)
        {
            BigInteger[] integers = StringParser.TokenizeDistinctIntegers(cellLabel.text);
            if (integers.Length == 0)
                return;
            else if (integers.Length == 1)
            {
                NumberEntry numberEntry = new NumberEntry(integers[0]);
                ModelController.PerformAddOutput(numberEntry, isUndoPoint: true);
                RequestUIRefresh();
            }
            else
            {
                GuiEnable = false;
                numberDialog.SetItems(integers.Select(i => i.ToString()));
                numberDialog.Show();
            }
        }
    }

    private void OnNumberDialogItemSelected(string selectedItem, bool cancelled)
    {
        if (cancelled)
        {
            GuiEnable = true;
            return;
        }
        NumberEntry numberEntry = new NumberEntry(BigInteger.Parse(selectedItem));
        ModelController.PerformAddOutput(numberEntry, isUndoPoint: true);
        GuiEnable = true;
        RequestUIRefresh();
    }


    public void Update()
    {
        if (uiRefreshRequested)
            PerformUIRefresh();
    }


    private void OnPrimesInstanceCompleted() => RequestUIRefresh();

    //private void OnApplicationPause(bool pauseStatus)
    //{
    //    if (pauseStatus) // Save data when the app is paused
    //        SaveData();
    //}

    //private void OnApplicationQuit() => SaveData(); // Save data when the app is about to quit

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // Save data when the app loses focus
            SaveData();
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
