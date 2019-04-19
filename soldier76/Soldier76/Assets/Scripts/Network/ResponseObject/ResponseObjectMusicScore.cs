using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseObjectMusicScore
{
    public string encoding;
    public Dictionary<string, object> feed;


    private Dictionary<string, object>[] entry;

    // feedからentryを抜き出して保持する
    public void SetupEntry()
    {
        foreach (string key in feed.Keys)
        {
            if (key == "entry")
            {
                this.entry = (Dictionary<string, object>[])feed[key];
            }
        }
    }

    // entry内のデータをわかりやすい形式のデータに変換して返却する
    public MasterMusicScoreRecordDataList GetDataList()
    {
        MasterMusicScoreRecordDataList dataList = new MasterMusicScoreRecordDataList();

        List<MasterMusicScoreRecordData> list = new List<MasterMusicScoreRecordData>();

        foreach (Dictionary<string, object> dic in this.entry)
        {
            string position = ResponseObjectManager.GetStringFromDictionary(dic, "position");
            string drum = ResponseObjectManager.GetStringFromDictionary(dic, "drum");

            MasterMusicScoreRecordData data = new MasterMusicScoreRecordData();
            data.position = uint.Parse(position);
            if (uint.TryParse(drum, out data.drum) == false)
            {
                data.drum = 0;
            }
            list.Add(data);

            Debug.Log_lime("更新確認データ抽出　position=" + position + ", drum = " + drum);
        }

        dataList.dataList = list;

        return dataList;
    }
}
