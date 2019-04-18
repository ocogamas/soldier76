using System.Collections;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    // スプレッドシート自体のID
    private const string SPREAD_SHEET_ID = "1NP-doRS-1pZrS5zKuKM-V61bSkAPvXz308tQbo875qI";

    // メインシートのキー
    // メインシートは１つのスプレッドシートに１つだけ存在する一番偉いシートのこと
    // スプレッドシートの仕様で、メインシートのキーはod6固定である。
    private const string SPREAD_SHEET_MAIN_KEY = "od6";

    // SPREAD_SHEET_IDと、シートのキーを与えることで、
    // 指定したシートのJSONを得るURL
    private const string SPREAD_SHEET_URL = "https://spreadsheets.google.com/feeds/list/{0}/{1}/public/values?alt=json";

    // サブシートのリストを得るためのJSONのURL
    private const string SPREAD_SHEET_SUB_SHEET_INFO_URL = "https://spreadsheets.google.com/feeds/worksheets/{0}/public/basic";

    public string GetSpreadSheetURLWithKey(string key)
    {
        return string.Format(SPREAD_SHEET_URL, SPREAD_SHEET_ID, key);
    }

    public string RequestMainSpreadSheet()
    {
        string url = GetSpreadSheetURLWithKey(SPREAD_SHEET_MAIN_KEY);
        return Request(url, "メインシート");
    }

    public string Request(string url, string title)
    {
        WebRequest request = WebRequest.Create(url);
        if (request != null)
        {
            request.Method = "GET";
        }
        else
        {
            Debug.LogError("requestがnull");
            return "";
        }

        WebResponse response = null;

        try
        {
            response = request.GetResponse();
        }
        catch (Exception e)
        {
            response = null;
            Debug.LogError("通信エラー。 title = " + title + " の通信が失敗しました");
        }

        string text = "";
        if (response != null)
        {
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
            text = streamReader.ReadToEnd();

            streamReader.Close();
            stream.Close();
        }

        return text;
    }
}
