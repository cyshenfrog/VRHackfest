using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    private static ControlManager _instance;
    public static ControlManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public enum Control
    {
        GearVR,
        Daydream,
    }
    public Control control;

    public GameObject gearVR;
    public GameObject daydream;

    #region Input Data
    public Quaternion Rotation
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.GetLocalControllerRotation(OVRInput.GetActiveController());
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.Orientation;
            }
            else
            {
                return Quaternion.identity;
            }
        }
    }
    private bool _isTouchDown;
    public bool IsTouchDown
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad); ;
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.TouchDown;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsTouching
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.Get(OVRInput.Touch.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.IsTouching;
            }
            else
            {
                return false;
            }
        }
    }

    public bool TouchpadButton
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.Get(OVRInput.Button.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.ClickButton;
            }
            return false;
        }
    }

    public bool TouchpadButtonDown
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.ClickButtonDown;
            }
            return false;
        }
    }

    public bool TouchpadButtonUp
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.ClickButtonUp;
            }
            return false;
        }
    }

    public Vector2 TouchPos
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.TouchPosCentered;
            }
            return Vector2.zero;
        }
    }

    public bool SecondaryButton
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.Get(OVRInput.RawButton.Back);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.AppButton;
            }
            return false;
        }
    }

    public bool SecondaryButtonDown
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.GetDown(OVRInput.RawButton.Back);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.AppButtonDown;
            }
            return false;
        }
    }

    public bool SecondaryButtonUp
    {
        get
        {
            if (control == Control.GearVR)
            {
                return OVRInput.GetUp(OVRInput.RawButton.Back);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.AppButtonUp;
            }
            return false;
        }
    }
    #endregion

    #region Event
    public Action onTouchButtonDown;
    public Action<Vector2> touchButtonEvent;
    public Action onSecondaryButtonDown;
    public Action isTouchingEvent;
    #endregion

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance.gameObject != gameObject)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

	private void Start ()
    {
		if (control == Control.GearVR)
        {
            gearVR.SetActive(true);
            daydream.SetActive(false);
        }
        else if (control == Control.Daydream)
        {
            gearVR.SetActive(false);
            daydream.SetActive(true);
        }
        //OVRTouchpad.TouchHandler += HandleTouchHandler;
    }
	
	private void Update ()
    {
        if (!TouchpadButtonDown || !SecondaryButtonDown)
        {
            if (TouchpadButtonDown)
            {
                if (onTouchButtonDown != null)
                {
                    onTouchButtonDown();
                }
            }
            if (SecondaryButtonDown)
            {
                if (onSecondaryButtonDown != null)
                {
                    onSecondaryButtonDown();
                }
            }
        }
        if (IsTouching)
        {
            if (touchButtonEvent != null)
            {
                touchButtonEvent(TouchPos);
            }
        }
	}

    private void HandleTouchHandler(object sender, EventArgs e)
    {
        OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)e;
        /*if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap)
        {
            // TAP!
        }*/
        if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap ||
            touchArgs.TouchType == OVRTouchpad.TouchEvent.Right ||
            touchArgs.TouchType == OVRTouchpad.TouchEvent.Left ||
            touchArgs.TouchType == OVRTouchpad.TouchEvent.Down)
        {
            // Down!
            _isTouchDown = true;
        }
        else
        {
            _isTouchDown = false;
        }
        /*if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Up)
        {
            // UP!
        }
        if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Right)
        {
            // Right!
        }
        if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Left)
        {
            // Left!
        }*/
    }

    public void SetupCanvas(GameObject canvas)
    {
        if (control == Control.GearVR)
        {
            canvas.AddComponent<GraphicRaycaster>();
        }
        if (control == Control.Daydream)
        {
            canvas.AddComponent<GvrPointerGraphicRaycaster>();
        }
    }
}
