using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delegates;
using UnityEngine.XR;

public class InputHandler : MonoBehaviour
{
    private static InputHandler instance;

    /// <summary>
    /// Event to handle single swipes Vector order is "Start position","Direction"
    /// </summary>
    public event Action<Swipe> OnSingleSwipe;
    public event Action<Swipe,Swipe> OnDualSwipe;
    public event Action<Vector2> OnTap;
    public event Action<Vector2,Vector2> OnDualTap;
    public event Action<Vector2> OnSingleHold;
    public event Action<Vector2,Vector2> OnDualHold;
    
    
    
    
    
    #region SwipeProperties
    private Swipe firstSwipe;
    private Swipe secondSwipe;
    #endregion

    private void Awake()
    {
        firstSwipe = new Swipe();
        secondSwipe = new Swipe();
        GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouch();
    }

    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            TouchPhase ph = t.phase;

            switch (ph)
            {
                case TouchPhase.Began:
                    firstSwipe.startPos = t.position;
                    if(Input.touchCount == 1)
                        OnTap?.Invoke(t.position);
                    break;
                case TouchPhase.Ended:
                    firstSwipe.endPos = firstSwipe.startPos + t.deltaPosition;
                    if(Input.touchCount == 1)
                        OnSingleSwipe?.Invoke(firstSwipe);
                    break;
                case TouchPhase.Moved:
                    firstSwipe.endPos = firstSwipe.startPos + t.deltaPosition;
                    if(Input.touchCount == 1)
                        OnSingleHold?.Invoke(t.position);
                    break;
            }
            
            if (Input.touchCount > 1)
            {
                Touch t1 = Input.GetTouch(1);
                TouchPhase ph1 = t.phase;
                
                switch (ph1)
                {
                    case TouchPhase.Began:
                        secondSwipe.startPos = t.position;
                        OnDualTap?.Invoke(t.position,t1.position);
                        break;
                    case TouchPhase.Ended:
                        secondSwipe.endPos = secondSwipe.startPos + t1.deltaPosition;
                        OnDualSwipe?.Invoke(firstSwipe,secondSwipe);
                        break;
                    case TouchPhase.Moved:
                        secondSwipe.endPos = secondSwipe.startPos + t1.deltaPosition;
                        OnDualHold?.Invoke(t.position,t1.position);
                        break;
                }
            }
        }
    }

    void GetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public static InputHandler Instance => instance;
}
