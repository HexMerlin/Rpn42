﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public partial class MainViewController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
 
    private OperationController OperationController;

    private Label inputLabel;

    private MultiColumnListView output;

    private VisualElement buttonGrid;

    private NumberDialog numberDialog;
  
    private volatile bool uiRefreshDemanded;
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
        SavedData savedData = PersistenceManager.LoadData();
     
        this.OperationController.ReadFrom(savedData);
        DemandUIRefresh();
    }


    private void SaveData()
    {
        SavedData savedData = new SavedData();
    
        this.OperationController.WriteTo(savedData);
        PersistenceManager.SaveData(savedData);
    }

    public void DemandUIRefresh()
    {
        uiRefreshDemanded = true;
    }

    private void PerformUIRefresh()
    {
        
        lock (uiRefreshLock)
        {
            bool storedGuiEnableState = GuiEnable;
            
            GuiEnable = false;
            this.inputLabel.text = OperationController.Input;

            for (int column = 0; column < this.output.columns.Count; column++)
            {
                string columnTitle = NumberEntry.ColumnTitle(column, this.OperationController.NumberFormat);
                this.output.columns[column].title = columnTitle;

                int maxCharCount = columnTitle.Length * 2;

                for (int row = 0; row < this.OperationController.OutputCount; row++)
                {
                    NumberEntry entry = this.OperationController[row];
                    int charCount = entry.ColumnData(column, OperationController.NumberFormat).Length;
                    if (charCount > maxCharCount)
                    {
                        maxCharCount = charCount;
                    }
                }
                this.output.columns[column].width = maxCharCount * 24;
            }

            this.output.RefreshItems();

            this.OperationController.CalcButtons.ButtonFormatFactor.SetEnabled(Primes.IsReady);
            this.OperationController.CalcButtons.ButtonFormatRepetend.SetEnabled(Primes.IsReady);
                      

            GuiEnable = storedGuiEnableState;

            if (this.OperationController.OutputCount > 0)
                this.output.schedule.Execute(() => this.output.ScrollToItem(this.OperationController.OutputCount - 1));
            
            uiRefreshDemanded = false;
        }
    }

}
