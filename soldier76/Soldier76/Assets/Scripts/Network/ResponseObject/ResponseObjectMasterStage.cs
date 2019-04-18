using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseObjectMasterStage
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
    public MasterStageRecordDataList GetDataList()
    {
        MasterStageRecordDataList dataList = new MasterStageRecordDataList();

        List<MasterStageRecordData> list = new List<MasterStageRecordData>();

        foreach (Dictionary<string, object> dic in this.entry)
        {
            string stageName  = ResponseObjectManager.GetStringFromDictionary(dic, "stagename");
            string bpm        = ResponseObjectManager.GetStringFromDictionary(dic, "bpm");
            string version    = ResponseObjectManager.GetStringFromDictionary(dic, "version");

            MasterStageRecordData data = new MasterStageRecordData();
            data.stageName = stageName;
            data.bpm       = bpm;
            data.version   = version;
            list.Add(data);

            Debug.Log_lime("更新確認データ抽出　stageName=" + stageName + ", bpm = " + bpm + ", version = " + version);
        }

        dataList.dataList = list;

        return dataList;
    }
}
