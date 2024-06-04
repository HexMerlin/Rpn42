using System;
using UnityEngine;

public partial class MainViewController
{


    bool PushNumber()
    {
        if (InputEmpty)
        {
            if (OperationController.OutputIsEmpty)
                return true; //no number to push
            
            OperationController.CopyLastOutput();

            RefreshOutput();
            return true;
        }

        Rational rational = new Rational(InputText);
        if (rational.IsInvalid)
        {
            Debug.LogWarning($"Failed to parse {InputText}");
            return false;
        }
        NumberEntry entry = new NumberEntry(rational);
        OperationController.AddLastOutput(entry);
        ClearInput();
        RefreshOutput();
        return true;
    }

    void PerformUnaryOperation(Func<Rational, Rational> operation)
    {
        if (AvailableOperands < 1)
            return; //need a number to perform operation: abort operation

        Rational operand;

        if (InputEmpty)
        {
            operand = OperationController.LastOutput.Rational;
        }
        else
        {
            operand = new Rational(InputText);
            if (operand.IsInvalid)
            {
                Debug.LogWarning($"Failed to parse {InputText}");
                return;
            }
        }

        Rational result = operation(operand);
        if (result.IsInvalid)
        {
            Debug.LogWarning($"Invalid operation");
            return;
        }

        if (InputEmpty)
            OperationController.RemoveLastOutput();
        else
            ClearInput();

        OperationController.AddLastOutput(new NumberEntry(result));
        RefreshOutput();


    }

    void PerformBinaryOperation(Func<Rational, Rational, Rational> operation)
    {
        if (AvailableOperands < 2)
            return; //need two numbers to perform operation: abort operation

        Rational operandA, operandB;

        if (InputEmpty)
        {
            operandA = OperationController.SecondLastOutput.Rational;
            operandB = OperationController.LastOutput.Rational;
        }
        else
        {
            operandA = OperationController.LastOutput.Rational;
            operandB = new Rational(InputText);
            if (operandB.IsInvalid)
            {
                Debug.LogWarning($"Failed to parse {InputText}");
                return;
            }
        }

        Rational result = operation(operandA, operandB);
        if (result.IsInvalid)
        {
            Debug.LogWarning($"Invalid operation");
            return;
        }
        OperationController.RemoveLastOutput();
     
        if (InputEmpty)
            OperationController.RemoveLastOutput();
        else
            ClearInput();

        OperationController.AddLastOutput(new NumberEntry(result));
        RefreshOutput();


    }

}

