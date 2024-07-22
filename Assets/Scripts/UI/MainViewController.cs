using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainViewController : MonoBehaviour
{
    public UIDocument UIDocument;

    private OperationController OperationController;

    private Label inputLabel;

    private MultiColumnListView output;

    private VisualElement buttonGrid;

    private CalcButtons calcButtons;

     private Format numberFormat = Format.Normal;
   
    private Format NumberFormat
    {
        get => numberFormat;
        set
        {
            if (numberFormat != value) SetNumberFormat(value);
        }
    }

    void OnEnable()
    {
        const string outputElementName = "output";
        const string inputElementName = "input";
        const string buttonGridName = "button-grid";

        this.OperationController = new(OnInputUpdate, OnOutputUpdate);

        VisualElement root = UIDocument.rootVisualElement;

        root.RegisterCallbackOnce<KeyDownEvent>(KeyDownEvent =>
        {
            if (KeyDownEvent.keyCode == KeyCode.Escape) 
                Application.Quit();

        }, TrickleDown.TrickleDown);


        this.inputLabel = root.Q<Label>(inputElementName) ?? throw new NullReferenceException($"{inputElementName} not found in the UIDocument.");

        this.output = root.Q<MultiColumnListView>(outputElementName) ?? throw new NullReferenceException("output not found in the UIDocument.");

        this.output.itemsSource = (System.Collections.IList) OperationController.OutputEntries;

        static VisualElement makeCell() => new Label();
        Debug.Assert(output.columns.Count == 3, $"Expected column count 3, but actual was {output.columns.Count}");

        for (int columnIndex = 0; columnIndex < 3; columnIndex++)
        {
            output.columns[columnIndex].makeCell = makeCell;
          
        }
        this.output.columns[0].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(0, NumberFormat);
        this.output.columns[1].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(1, NumberFormat);
        this.output.columns[2].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(2, NumberFormat);
       
        this.output.makeNoneElement = () => new Label(""); //avoid message "List is empty"
       
        this.buttonGrid = root.Q<VisualElement>(buttonGridName) ?? throw new NullReferenceException($"{buttonGridName} not found in the UIDocument.");

        this.buttonGrid.RegisterCallback<ClickEvent>(OnButtonGridClick);
        this.buttonGrid.RegisterCallback<GeometryChangedEvent>(OnButtonGridGeometryChanged);

        this.calcButtons = new CalcButtons(buttonGrid);
   
        SetNumberFormat(Format.Normal); //call method (instead of property) to force update of the buttons

        OnInputUpdate(string.Empty);
        OnOutputUpdate();

    }


    private void OnInputUpdate(string text) => this.inputLabel.text = text;


    private void OnOutputUpdate()
    {
        AssertColumnWidths();

        this.output.Rebuild();   

        if (this.output.itemsSource.Count > 0)
            this.output.ScrollToItem(this.output.itemsSource.Count - 1);
        
        void AssertColumnWidths()
        {
            for (int column = 0; column < this.output.columns.Count; column++)
            {
                int maxCharCount = this.output.columns[column].title.Length * 2;
           
                for (int row = 0; row < this.OperationController.OutputCount; row++)
                {
                    NumberEntry entry = this.OperationController[row];
                    int charCount = entry.ColumnData(column, NumberFormat).Length;
                    if (charCount > maxCharCount)
                    {
                        maxCharCount = charCount;
                    }
                }
                this.output.columns[column].width = maxCharCount * 24;
            }
        }
    }

    private void SetNumberFormat(Format format)
    {
       calcButtons.GetNumberFormatButton(this.numberFormat).Deselect(); //deselect previous selected button

        this.numberFormat = format;

        calcButtons.GetNumberFormatButton(this.numberFormat).Select(); //select new button

        SetColumnTitles();

        OnOutputUpdate();
    }

    private void OnButtonGridClick(ClickEvent evt)
    {
        if (evt.target is not Button unityButton)
            return;

        if (calcButtons.TryGetButton(unityButton.name) is not CalcButton button)
        {
            Debug.LogWarning($"Unhandled button: {unityButton.name}");
            return;
        }

        SuspendUserInterface();

        if (calcButtons.IsNumberFormatButton(button.Name) is Format numberFormat)
            NumberFormat = numberFormat;
        
        else
            OperationController.InputButtonPressed(button);
        
        ResumeUserInterface();
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

    private void SetColumnTitles()
    {
        this.output.columns[0].title = "Fraction";
        this.output.columns[1].title = "Attr";
        this.output.columns[2].title = this.NumberFormat switch
        {
            Format.Normal => "Decimal",
            Format.Bin => "Binary",
            Format.BalBin => "Bal Binary",
            Format.RotationsBin => "Rotations",
            Format.RotationsBalBin => "Rotations",
            Format.Partition => "Partitions",
            _ => throw new ArgumentException($"Unhandled format '{NumberFormat}'"),
        };
            

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

    private void SuspendUserInterface() => SetUserInterfaceEnabled(false);

    private void ResumeUserInterface() => SetUserInterfaceEnabled(true);

    private void SetUserInterfaceEnabled(bool enabled)
    {
        this.inputLabel.SetEnabled(enabled);
        this.output.SetEnabled(enabled);
        this.buttonGrid.SetEnabled(enabled);
    }

}
