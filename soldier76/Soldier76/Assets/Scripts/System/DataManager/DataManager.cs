
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager 
{
    public static string UPDATE_INFO      = "ui";   // 更新情報
    public static string MUSIC_SCORE_DATA = "msd";  // 譜面データ
    public static string PLAY_RECORD_DATA = "prd";  // プレイ記録データ

    public static void Save<T>(string fileName, T target)
    {
        Debug.Log_blue ("保存します。" + fileName);
        string path = getPath (fileName);
        string json = JsonFx.Json.JsonWriter.Serialize (target);

        File.WriteAllText (path, json);
    }

    public static T Load<T>(string fileName)
    {
        Debug.Log_blue ("読み込みます。" + fileName);
        string path = getPath (fileName);
        if (File.Exists (path) == false)
        {
            return default (T);
        }
        string json = File.ReadAllText (path);

        T result = JsonFx.Json.JsonReader.Deserialize<T> (json);
        return result;
    }

    public static bool Delete(string fileName)
    {
        string path = getPath (fileName);
        if (File.Exists (path))
        {
            File.Delete (path);
            return true;
        }
        return false;           
    }


    private static string getPath(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;

        Debug.Log_blue ("getPath > path = " + path);
        return path;
    }
}
