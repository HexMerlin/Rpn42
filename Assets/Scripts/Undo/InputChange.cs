﻿public abstract class InputChange : Change
{
    
    public string Input { get; }

    public InputBuffer InputBuffer => ModelController.InputBuffer;

    public InputChange(ModelController modelController, string input) : base(modelController)
    {
        Input = input;
    }

}
