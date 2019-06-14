using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMusicScoreRecordData
{
    // 譜面の座標
    public uint position;

    // ドラム。1ならばノートあり
    public uint drum;
    
    // スネア。1ならばノーとあり
    public uint snare;
    
    // ハイハット。１ならばノーとあり
    public uint hihat;

    // 譜面の座標の時刻
    public float time;

    // 判定が終わったらTrue
    public bool isDrumJudgeDone;
    public bool isHihatJudgeDone;
    public bool isSnareJudgeDone;
}