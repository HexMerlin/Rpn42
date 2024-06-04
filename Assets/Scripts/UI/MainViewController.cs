using System;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public partial class MainViewController : MonoBehaviour
{
    public UIDocument UIDocument;

    private readonly OperationController OperationController = new ();
 
    private Label input;

    private MultiColumnListView output;

    private VisualElement buttonGrid;

    private readonly Button[] numberFormatButtons = new Button[Enum.GetValues(typeof(Format)).Length];

    private Format numberFormat = Format.Normal;
    private Format NumberFormat
    {
        get => numberFormat;
        set
        {
            if (numberFormat != value)
                SetNumberFormat(value);
        }
    }

    void OnEnable()
    {
        Input.backButtonLeavesApp = true;
        const string outputElementName = "output";
        const string inputElementName = "input";
        const string buttonGridName = "button-grid";

        VisualElement root = UIDocument.rootVisualElement;
        this.input = root.Q<Label>(inputElementName) ?? throw new NullReferenceException($"{inputElementName} not found in the UIDocument.");

        this.output = root.Q<MultiColumnListView>(outputElementName) ?? throw new NullReferenceException("output not found in the UIDocument.");

        output.itemsSource = (System.Collections.IList) OperationController.OutputEntries;

        static VisualElement makeCell() => new Label();
        Debug.Assert(output.columns.Count == 3, $"Expected column count 3, but actual was {output.columns.Count}");

        for (int columnIndex = 0; columnIndex < 3; columnIndex++)
        {
            output.columns[columnIndex].makeCell = makeCell;
          
        }
        output.columns[0].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(0, NumberFormat);
        output.columns[1].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(1, NumberFormat);
        output.columns[2].bindCell = (e, row) => (e as Label).text = OperationController[row].ColumnData(2, NumberFormat);

        
        output.makeNoneElement = () => new Label(""); //avoid message "List is empty"
  
        buttonGrid = root.Q<VisualElement>(buttonGridName) ?? throw new NullReferenceException($"{buttonGridName} not found in the UIDocument.");
        
        buttonGrid.RegisterCallback<ClickEvent>(OnButtonGridClick);
        buttonGrid.RegisterCallback<GeometryChangedEvent>(OnButtonGridGeometryChanged);

        foreach (Format format in Enum.GetValues(typeof(Format)))
        {
            numberFormatButtons[(int)format] = format switch
            {
                Format.Normal => buttonGrid.Q<Button>("button-format-normal"),
                Format.Bin => buttonGrid.Q<Button>("button-format-bin"),
                Format.BalBin => buttonGrid.Q<Button>("button-format-balbin"),
                Format.RotationsBin => buttonGrid.Q<Button>("button-format-rotbin"),
                Format.RotationsBalBin => buttonGrid.Q<Button>("button-format-rotbalbin"),
                Format.Partition => buttonGrid.Q<Button>("button-format-partition"),
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unknown format")
            };
            if (numberFormatButtons[(int)format] == null)
                Debug.LogWarning($"Button for format {format} not found");
        }
        
        SetNumberFormat(Format.Normal); //call method (instead of property) to force update of the buttons
      
        ClearInput();
        ClearOutput();

        RefreshOutput(true);

    }

    private void RefreshOutput(bool makeCompleteRebuild = false)
    {
       
        if (makeCompleteRebuild)
        {
       
            output.Rebuild();

        }
        else
            output.RefreshItems();

        if (output.itemsSource.Count > 0)
        {
            output.ScrollToItem(output.itemsSource.Count - 1);
            AssertColumnWidths();
        }
    }



    private string InputText
    {
        get => input.text;
        set => this.input.text = value;
    }

    private bool OutputEmpty => OperationController.OutputIsEmpty;
    private bool InputEmpty => string.IsNullOrEmpty(InputText);

    private int AvailableOperands => OperationController.OutputCount + (InputEmpty ? 0 : 1);


    private void SetNumberFormat(Format format)
    {
        numberFormat = format;
      
        const string selectedClass = "selected";

        for (int i = 0; i < numberFormatButtons.Length; i++)
        {
            Button button = numberFormatButtons[i];
            if (i == (int)format)
                button.AddToClassList(selectedClass);
            else
                button.RemoveFromClassList(selectedClass);
        }

        RefreshOutput();
    }

    void ClearInput() => InputText = string.Empty;

    void ClearOutput()
    {
        OperationController.ClearOutput();
        RefreshOutput();
    }

    void ClearLast()
    {
        if (OutputEmpty)
            return;
        OperationController.RemoveLastOutput();
        RefreshOutput();
    }

    private void OnButtonGridClick(ClickEvent evt)
    {
        if (evt.target is not Button button)
            return;


        // Check if a number-format button was clicked
        int numberFormatButtonIndex = Array.IndexOf(numberFormatButtons, button);
        if (numberFormatButtonIndex >= 0)
        {
            NumberFormat = (Format)numberFormatButtonIndex;
            return;
        }
        SuspendUserInterface();

        const string buttonPrefix = "button-";

        string buttonValue = button.name.Length > buttonPrefix.Length ? button.name[buttonPrefix.Length..] : button.name;

        switch (buttonValue)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                InputText += buttonValue;
                break;
            case "enter":
                PushNumber();
                break;
            case "back-drop":
                if (InputEmpty)
                    ClearLast();
                else
                    InputText = InputText.Remove(InputText.Length - 1);
                break;
            case "swap":
                //implement this
                break;
            case "neg":
                PerformUnaryOperation((a) => -a);        
                break;
            case "reciprocal":
                PerformUnaryOperation((a) => a.Reciprocal);
                break;
            case "square":
                PerformUnaryOperation((a) => a * a);
                break;
            case "sum":
                PerformBinaryOperation((a, b) => a + b);
                break;
            case "diff":
                PerformBinaryOperation((a, b) => a - b);
                break;
            case "prod":
                PerformBinaryOperation((a, b) => a * b);
                break;
            case "quotient":
                PerformBinaryOperation((a, b) => a / b);
                break;
            case "clear":
                ClearInput();
                ClearOutput();
                break;
            case "mod":
                PerformBinaryOperation((a, b) => a % b);
                break;
            case "div-ones":
                PerformUnaryOperation((a) => a.DivideByMersenneCeiling());
                break;
            default:
                Debug.LogWarning($"Unhandled button: {button.name}");
                break;
        }
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



    private void AssertColumnWidths()
    {
        if (OperationController.OutputIsEmpty)
            return;

        for (int column = 0; column < output.columns.Count; column++)
        {
            int maxCharCount = 8;
            for (int row = 0; row < OperationController.OutputCount; row++)
            {
                NumberEntry entry = OperationController[row];
                int charCount = entry.ColumnData(column, NumberFormat).Length;
                if (charCount > maxCharCount)
                {
                    maxCharCount = charCount;
                }
            }
            output.columns[column].width = maxCharCount * 24;
        }
    }

    private void OnDisable()
    {
        if (buttonGrid != null)
        {
            buttonGrid.UnregisterCallback<GeometryChangedEvent>(OnButtonGridGeometryChanged);

            buttonGrid.UnregisterCallback<ClickEvent>(OnButtonGridClick);
        }
    }
    public void SuspendUserInterface()
    {
        input.SetEnabled(false);
        output.SetEnabled(false);
        buttonGrid.SetEnabled(false);
    }

    public void ResumeUserInterface()
    {
        input.SetEnabled(true);
        output.SetEnabled(true);
        buttonGrid.SetEnabled(true);
    }
}
