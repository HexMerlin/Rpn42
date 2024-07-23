
using Newtonsoft.Json;


public class SavedData
{
    public Format numberFormat;

    public NumberEntry[] numberEntries;

    public string input;

    public SavedData()
    {
        this.numberFormat = Format.Normal;
        this.numberEntries = new NumberEntry[0];
        this.input = string.Empty;
       
    }
}
