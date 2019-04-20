using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMusicScoreRecordData
{
    // 譜面の座標
    public uint position;

    // ドラム。1ならばノートあり
    public uint drum;

    // 譜面の座標の時刻
    public float time;
}