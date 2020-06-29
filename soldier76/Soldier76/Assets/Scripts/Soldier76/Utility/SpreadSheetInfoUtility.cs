using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class SpreadSheetInfoUtility
{

    // 全てのSpreadSheetの情報のDictionaryを返す
    // 名前をキーにして、SpreadSheetIdを得るDictionaryを返す
    public static Dictionary<string, string> GetSpreadSheetInfoDictionary(NetworkManager networkManager)
    {
       
        Dictionary<string, string> spreadSheetInfoDictionary = new Dictionary<string, string>();

        string subSheetList = networkManager.RequestSubSpreadSheetList();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(subSheetList));

        XmlNodeList entryList = xmlDoc.GetElementsByTagName("entry");

        foreach (XmlNode node in entryList)
        {
            string title = "";
            string url = "";

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "id")
                {
                    url = childNode.InnerText;
                }
                else if (childNode.Name == "title")
                {
                    title = childNode.InnerText;
                }
            }


            if (!string.IsNullOrEmpty(title))
            {
                string sheetId = getSheetId(url); 
                spreadSheetInfoDictionary.Add(title, sheetId);

                //Debug.Log_cyan("GetSpreadSheetInfoDictionary > 辞書に登録 > title = " + title + ", sheetId = " + sheetId, null );
            }

        }

        return spreadSheetInfoDictionary;
    }

    private static string getSheetId(string url)
    {
        const string basicUrl = "https://spreadsheets.google.com/feeds/worksheets/1NP-doRS-1pZrS5zKuKM-V61bSkAPvXz308tQbo875qI/public/basic/";

        return url.Replace(basicUrl, "");
    }

}
