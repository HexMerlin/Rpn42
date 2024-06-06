//using System;
//using UnityEngine;

//public partial class MainViewController
//{


//    //bool PushNumber()
//    //{
//    //    if (OperationController.InputEmpty)
//    //    {
//    //        if (OperationController.OutputIsEmpty)
//    //            return true; //no number to push
            
//    //        OperationController.CopyLastOutput();

//    //        RefreshOutput();
//    //        return true;
//    //    }

//    //    Rational rational = new Rational(OperationController.Input);
//    //    if (rational.IsInvalid)
//    //    {
//    //        Debug.LogWarning($"Failed to parse {OperationController.Input}");
//    //        return false;
//    //    }
//    //    NumberEntry entry = new NumberEntry(rational);
//    //    OperationController.AddLastOutput(entry);
//    //    ClearInput();
//    //    RefreshOutput();
//    //    return true;
//    //}

//    //void PerformUnaryOperation(Func<Rational, Rational> operation)
//    //{
//    //    if (AvailableOperands < 1)
//    //        return; //need a number to perform operation: abort operation

//    //    Rational operand;

//    //    if (OperationController.InputEmpty)
//    //    {
//    //        operand = OperationController.LastOutput.Rational;
//    //    }
//    //    else
//    //    {
//    //        operand = new Rational(OperationController.Input);
//    //        if (operand.IsInvalid)
//    //        {
//    //            Debug.LogWarning($"Failed to parse {OperationController.Input}");
//    //            return;
//    //        }
//    //    }

//    //    Rational result = operation(operand);
//    //    if (result.IsInvalid)
//    //    {
//    //        Debug.LogWarning($"Invalid operation");
//    //        return;
//    //    }

//    //    if (OperationController.InputEmpty)
//    //        OperationController.RemoveLastOutput();
//    //    else
//    //        ClearInput();

//    //    OperationController.AddLastOutput(new NumberEntry(result));
//    //    RefreshOutput();


//    //}

//    //void PerformBinaryOperation(Func<Rational, Rational, Rational> operation)
//    //{
//    //    if (AvailableOperands < 2)
//    //        return; //need two numbers to perform operation: abort operation

//    //    Rational operandA, operandB;

//    //    if (OperationController.InputEmpty)
//    //    {
//    //        operandA = OperationController.SecondLastOutput.Rational;
//    //        operandB = OperationController.LastOutput.Rational;
//    //    }
//    //    else
//    //    {
//    //        operandA = OperationController.LastOutput.Rational;
//    //        operandB = new Rational(OperationController.Input);
//    //        if (operandB.IsInvalid)
//    //        {
//    //            Debug.LogWarning($"Failed to parse {OperationController.Input}");
//    //            return;
//    //        }
//    //    }

//    //    Rational result = operation(operandA, operandB);
//    //    if (result.IsInvalid)
//    //    {
//    //        Debug.LogWarning($"Invalid operation");
//    //        return;
//    //    }
//    //    OperationController.RemoveLastOutput();
     
//    //    if (OperationController.InputEmpty)
//    //        OperationController.RemoveLastOutput();
//    //    else
//    //        ClearInput();

//    //    OperationController.AddLastOutput(new NumberEntry(result));
//    //    RefreshOutput();


//    //}

//}

