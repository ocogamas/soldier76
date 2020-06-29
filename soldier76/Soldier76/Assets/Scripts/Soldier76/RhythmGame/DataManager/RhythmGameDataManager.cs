using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameDataManager
{
    public static MasterStageRecordData masterStageRecordData;

    // プレイしようとしている１つのステージのデータ
    public static MasterMusicScoreRecordDataList musicScoreRecordDataList;

    // 全譜面データ
    // 譜面の名前をキーにして譜面を格納する
    public static Dictionary<string, MasterMusicScoreRecordDataList> musicScoreDictionary = new Dictionary<string, MasterMusicScoreRecordDataList>();


    public static MasterStageRecordDataList masterStageRecordDataList;


    public static bool isPracticeMode;
}
