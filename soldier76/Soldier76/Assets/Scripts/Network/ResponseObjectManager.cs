using System.Collections.Generic;

public class ResponseObjectManager 
{
    public static string GetStringFromDictionary(Dictionary<string, object> dic, string key)
    {
        //Debug.Log_blue ("GetStringFromDictionary > key = " + key);
        Dictionary<string, object> elementDic = (Dictionary<string, object>)dic ["gsx$" + key];
        string result = (string)elementDic ["$t"];
        return result;
    }
}
