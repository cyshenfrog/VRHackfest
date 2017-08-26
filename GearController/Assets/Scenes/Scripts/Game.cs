using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game _instance;
    public static Game Instance
    {
        get
        {
            return _instance;
        }
    }

    private ControlManager _controller;

    public enum State
    {
        Intro,
        Gameplay,
    }
    public State state;

    public GameObject canvas;
    public Button startButton;

    public InteractiveObject[] items;

    public Transform gearVRControllerAnchor;
    public Transform daydreamControllerAnchor;
    public Transform controllerAnchor;
    public Transform reticle;
    public float distanceForwardController = 1;
    public Transform leverPlane;
    public Vector3 leverPlaneNormal;
    public Transform lever1, lever2;
    public Transform lever1End, lever2End;
    public Transform itemAttachedToLever1, itemAttachedToLever2;
    public Transform currentItemRaycastHit;
    public Transform currentRaycastHit;

    public const string CORRECT_ITEM = "CorrectItem";
    public const string WRONG_ITEM = "WrongItem";

    public InteractiveObject interactiveSample;
    public Text info;
    public LineRenderer line;

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
    }

    private IEnumerator Start ()
    {
        while (ControlManager.Instance == null)
        {
            yield return null;
        }
        _controller = ControlManager.Instance;
        _controller.SetupCanvas(canvas);
        if (_controller.control == ControlManager.Control.GearVR)
        {
            controllerAnchor = gearVRControllerAnchor;
        }
        else if (_controller.control == ControlManager.Control.Daydream)
        {
            controllerAnchor = daydreamControllerAnchor;
        }

        leverPlaneNormal = leverPlane.forward;

        interactiveSample.onPointerClick = (go) => { info.text = "Click"; };
        interactiveSample.onPointerExit = (go) => { info.text = "Exit"; };
    }

    private void Update ()
    {
        if (_controller == null)
        {
            return;
        }

        ChooseItem();
    }

    public void OnStartButtonClicked()
    {
        startButton.gameObject.SetActive(false);
    }

    private void ChooseItem()
    {
        Ray ray;
        if (_controller.control == ControlManager.Control.Daydream)
        {
            ray = new Ray(controllerAnchor.position, reticle.position - controllerAnchor.position);
            line.SetPosition(0, controllerAnchor.position);
            line.SetPosition(1, reticle.position);
        }
        else
        {
            ray = new Ray(controllerAnchor.position, controllerAnchor.forward);
            line.SetPosition(0, controllerAnchor.position);
            line.SetPosition(1, controllerAnchor.position + controllerAnchor.forward * distanceForwardController);
        }
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag(WRONG_ITEM) || hit.collider.CompareTag(CORRECT_ITEM))
            {
                currentItemRaycastHit = hit.collider.transform;
            }
            else
            {
                currentItemRaycastHit = null;
            }
        }
        else
        {
            currentItemRaycastHit = currentRaycastHit = null;
        }

        Quaternion q = Quaternion.LookRotation(-controllerAnchor.forward);
        Vector3 original = -controllerAnchor.forward;
        Vector3 project = original - (Vector3.Dot(original, leverPlaneNormal) / leverPlaneNormal.sqrMagnitude) * leverPlaneNormal;
        lever1.rotation = lever2.rotation = Quaternion.LookRotation(project);

        if (_controller.SecondaryButtonDown)
        {
            //info.text = "SecondaryButtonDown";
            if (itemAttachedToLever1 == null)
            {
                if (currentItemRaycastHit != null)
                {
                    info.text = currentItemRaycastHit.name;
                    currentItemRaycastHit.SetParent(lever1End, true);
                    currentItemRaycastHit.localPosition = Vector3.zero;
                    itemAttachedToLever1 = currentItemRaycastHit;
                }
                else
                {
                    info.text = "?????";
                }
            }
            else
            {
                info.text = "Drop " + itemAttachedToLever1.name;
                itemAttachedToLever1.SetParent(null);
                itemAttachedToLever1.position = itemAttachedToLever1.GetComponent<InteractiveObject>().originalPosition;
                itemAttachedToLever1.rotation = itemAttachedToLever1.GetComponent<InteractiveObject>().originalRotation;
                itemAttachedToLever1 = null;
            }
        }

        if (itemAttachedToLever1 != null)
        {
            itemAttachedToLever1.rotation = controllerAnchor.rotation;
        }
        if (itemAttachedToLever2 != null)
        {
            itemAttachedToLever2.rotation = controllerAnchor.rotation;
        }
    }

    private void Unlock()
    {

    }
}
