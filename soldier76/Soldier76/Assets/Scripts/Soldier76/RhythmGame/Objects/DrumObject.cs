using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumObject : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private GameObject bodyObject;
    
    public GameObject BodyObject() 
    {
    	return this.bodyObject;
    }

    private System.Action onTouchDown;

    public void RegisterCallbackOnTouchDown(System.Action onTouchDown)
    {
        this.onTouchDown = onTouchDown;
    }

    public void OnTouchDown()
    {
        this.onTouchDown?.Invoke();
        
        this.animator.Play("DrumAnimation", 0, 0);
    }


}
