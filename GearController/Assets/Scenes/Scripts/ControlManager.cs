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
    public bool IsTouching
    {
        get
        {
            if (control == Control.GearVR)
            {
                return (OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad) != Vector2.zero);
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
                return OVRInput.Get(OVRInput.Touch.PrimaryTouchpad);
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
                return OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.TouchDown;
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
                return OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad);
            }
            else if (control == Control.Daydream)
            {
                return GvrControllerInput.TouchUp;
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
                return OVRInput.GetUp(OVRInput.RawButton.Back);
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
