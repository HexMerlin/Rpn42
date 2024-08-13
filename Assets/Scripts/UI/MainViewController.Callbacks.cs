﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public partial class MainViewController
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

        this.OperationController = new(buttonGrid);

        Primes.Prepare(OnPrimesInstanceCompleted); //start preparing the prime factoring capability in the background

        this.numberDialog = Control<NumberDialog>(numberDialogName);

        this.inputLabel = Control<Label>(inputElementName);

        this.output = Control<MultiColumnListView>(outputElementName);

        this.output.itemsSource = (System.Collections.IList)OperationController.OutputEntries;

        Debug.Assert(output.columns.Count == 3, $"Expected column count 3, but actual was {output.columns.Count}");

        static VisualElement makeCell() => new Label();
        for (int columnIndex = 0; columnIndex < output.columns.Count; columnIndex++)
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

       
        this.numberDialog.ItemSelected += OnNumberDialogItemSelected;

        this.output.RegisterCallback<ClickEvent>(OnCellClick);
        
        this.numberDialog.Hide();
        LoadSavedData();
        DemandUIRefresh();

    }

 
    private void OnCellClick(ClickEvent evt)
    {
        if (evt.target is Label cellLabel)
        {
            BigInteger[] integers = Tokenizer.TokenizeDistinctIntegers(cellLabel.text);
            if (integers.Length == 0)
                return;
            else if (integers.Length == 1)
            {
                //Debug.Log("Cell clicked: " + integers[0]);
                NumberEntry numberEntry = new NumberEntry(integers[0]);
                OperationController.AddOutput(numberEntry, isUndoPoint: true);
                DemandUIRefresh();
            }
            else
            {
                //Debug.Log($"Multiple cell value clicked: {string.Join(", ", integers)}");
                GuiEnable = false;
                numberDialog.SetItems(integers.Select(i => i.ToString()));
                numberDialog.Show();
            }
        }
    }

    private void OnNumberDialogItemSelected(string selectedItem, bool cancelled)
    {
        Debug.Log("User selected: " + selectedItem);

        if (cancelled)
        {
            GuiEnable = true;
            return;
        }
        NumberEntry numberEntry = new NumberEntry(BigInteger.Parse(selectedItem));
        OperationController.AddOutput(numberEntry, isUndoPoint: true);
        GuiEnable = true;
        DemandUIRefresh();
    }


    public void Update()
    {
        if (uiRefreshDemanded)
            PerformUIRefresh();
    }


    private void OnPrimesInstanceCompleted() => DemandUIRefresh();

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

      
        DemandUIRefresh();
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