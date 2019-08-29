using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteState
{
    Inactive,
    Moving,
    Judged,
}

public enum NoteSoundType
{
	drum = 0,
	snare,
	hihat
}

public enum JudgeType
{
	MISS,
	SAFE,
	GOOD,
	GREAT,
	PERFECT
}