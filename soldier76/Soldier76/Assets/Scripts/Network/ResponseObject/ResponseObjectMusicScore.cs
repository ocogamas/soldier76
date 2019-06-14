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
            MasterMusicScoreRecordData data = new MasterMusicScoreRecordData();

        	string position = ResponseObjectManager.GetStringFromDictionary(dic, "position");
            data.position = uint.Parse(position);

            string drum = ResponseObjectManager.GetStringFromDictionary(dic, "drum");
            if (uint.TryParse(drum, out data.drum) == false)
            {
                data.drum = 0;
            }
            
            string snare = ResponseObjectManager.GetStringFromDictionary(dic, "snare");
            if (uint.TryParse(snare, out data.snare) == false)
            {
            	data.snare = 0;
            }
                       
            string hihat = ResponseObjectManager.GetStringFromDictionary(dic, "hihat");
            if (uint.TryParse(hihat, out data.hihat) == false)
            {
            	data.hihat = 0;
            }
            
            list.Add(data);
        }

        dataList.dataList = list;

        return dataList;
    }
}
