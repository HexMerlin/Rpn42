
using Newtonsoft.Json;


public class SavedData
{
 
    public NumberEntry[] numberEntries;

    public string input;

    public SavedData()
    {
        this.numberEntries = new NumberEntry[0];
        this.input = string.Empty;
       
    }
}
