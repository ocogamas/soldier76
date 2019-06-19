using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    // 担当するボタン
    private DrumObject drumObject;

    private NoteState noteState;


    #region Public

    public void Setup()
    {
        this.noteState = NoteState.Inactive;


    }

    public void ExecUpdate()
    {
        
    }

    #endregion // Public



}
