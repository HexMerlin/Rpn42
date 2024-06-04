
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.InputSystem;


public class OperationController
{
    private List<NumberEntry> outputEntries;

    public IReadOnlyList<NumberEntry> OutputEntries => outputEntries;

    public OperationController()
    {
        outputEntries = new List<NumberEntry>();

    }

    public NumberEntry this[int index] => outputEntries[index];

    public int OutputCount => outputEntries.Count;

    public bool OutputIsEmpty => outputEntries.Count == 0;
  

    public NumberEntry LastOutput => outputEntries[^1];

    public NumberEntry SecondLastOutput => outputEntries[^2];

    public void AddLastOutput(NumberEntry entry)
    {
        outputEntries.Add(entry);
    }

    public void CopyLastOutput()
    {
        outputEntries.Add(outputEntries[^1]);
    }

    public void RemoveLastOutput()
    {
        outputEntries.RemoveAt(outputEntries.Count - 1);
    }

    public void ClearOutput()
    {
        outputEntries.Clear();
    }
}

//internal class UndoManager
//{
//    private static UndoManager instance;
//    private Stack<IUndoable> undoStack;
//    private Stack<IUndoable> redoStack;

//    private UndoManager()
//    {
//        undoStack = new Stack<IUndoable>();
//        redoStack = new Stack<IUndoable>();
//    }

//    public static UndoManager Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = new UndoManager();
//            }
//            return instance;
//        }
//    }

//    public void AddUndo(IUndoable undoable)
//    {
//        undoStack.Push(undoable);
//        redoStack.Clear();
//    }

//    public void Undo()
//    {
//        if (undoStack.Count > 0)
//        {
//            IUndoable undoable = undoStack.Pop();
//            undoable.Undo();
//            redoStack.Push(undoable);
//        }
//    }

//    public void Redo()
//    {
//        if (redoStack.Count > 0)
//        {
//            IUndoable undoable = redoStack.Pop();
//            undoable.Redo();
//            undoStack.Push(undoable);
//        }
//    }
//}

