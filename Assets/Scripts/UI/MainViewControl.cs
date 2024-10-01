using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public partial class MainViewControl : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private ButtonCollection Buttons;

    private ModelController ModelController;

    private Label inputLabel;

    private MultiColumnListView output;

    private VisualElement buttonGrid;

    private NumberDialog numberDialog;
  
    private volatile bool uiRefreshRequested;
    private readonly object uiRefreshLock = new object();

    private bool _guiEnabled = true;

    private bool GuiEnable
    {
        set
        {
            if (_guiEnabled == value)
                return;
            this._guiEnabled = value;
            this.output.SetEnabled(value);
            this.inputLabel.SetEnabled(value);
            this.buttonGrid.SetEnabled(value);

        }
        get => _guiEnabled;
    }

    private void LoadSavedData()
    {
        this.ModelController.LoadSavedData();
        RequestUIRefresh();
    }


    private void SaveData() => this.ModelController.SaveData();

    public void RequestUIRefresh() => this.uiRefreshRequested = true;

    private void PerformUIRefresh()
    {
        
        lock (uiRefreshLock)
        {
            bool storedGuiEnableState = GuiEnable;
            
            GuiEnable = false;
            this.inputLabel.text = ModelController.Input;

            for (int column = 0; column < this.output.columns.Count; column++)
            {
                string columnTitle = NumberEntry.ColumnTitle(column, this.ModelController.NumberFormat);
                this.output.columns[column].title = columnTitle;

                int maxCharCount = columnTitle.Length * 2;

                for (int row = 0; row < this.ModelController.OutputCount; row++)
                {
                    NumberEntry entry = this.ModelController[row];
                    int charCount = entry.ColumnData(column, ModelController.NumberFormat).Length;
                    if (charCount > maxCharCount)
                    {
                        maxCharCount = charCount;
                    }
                }
                this.output.columns[column].width = maxCharCount * 24;
            }

            this.output.RefreshItems();

            this.Buttons.UpdateButtons(ModelController);

         
            GuiEnable = storedGuiEnableState;
            
            this.output.selectedIndex = -1;
            if (this.ModelController.OutputCount > 0)
                this.output.schedule.Execute(() => this.output.ScrollToItem(this.ModelController.OutputCount - 1));
            
            uiRefreshRequested = false;
        }
    }

}
