using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public ControlManager controller;

    public enum State
    {
        Intro,
        Gameplay,
    }

    public State state;

    public GameObject canvas;
    public Button startButton;

    public InteractiveObject[] items;

    #region Pick Item

    public Transform gearVRControllerAnchor;
    public Transform daydreamControllerAnchor;
    public Transform controllerAnchor;
    public Transform reticle;
    public float distanceForwardController = 1;
    public Transform environment;
    public Transform leverPlane;
    public Vector3 leverPlaneNormal;
    public Transform lever1, lever2;
    public Transform lever1End, lever2End;
    public Transform itemAttachedToLever1, itemAttachedToLever2;
    public Transform currentItemRaycastHit;
    public Transform currentRaycastHit;
    public Transform currentLock;
    public AudioSource attractItem;

    public float dropHeight1, dropHeight2;
    public AudioSource bigDrop, smallDrop;

    #endregion Pick Item

    #region Unlock Key

    public float lastX;

    #endregion Unlock Key

    #region Unlock Treasure

    public Treasure treasure;
    public Transform knife;

    #endregion Unlock Treasure

    #region Cut Rope

    public bool firstGetKnife = true;
    public Quaternion lastKnifeRotation;
    public float cutTime = 1;
    public float cutTimer;
    public Transform rope;
    public GameObject victory;

    #endregion Cut Rope

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

    private IEnumerator Start()
    {
        while (ControlManager.Instance == null)
        {
            yield return null;
        }
        controller = ControlManager.Instance;
        controller.SetupCanvas(canvas);
        if (controller.control == ControlManager.Control.GearVR)
        {
            controllerAnchor = gearVRControllerAnchor;
        }
        else if (controller.control == ControlManager.Control.Daydream)
        {
            controllerAnchor = daydreamControllerAnchor;
        }

        leverPlaneNormal = leverPlane.forward;

        items[2].GetComponent<Lock>().onUnlocked = items[3].GetComponent<Lock>().onUnlocked = OnUnlockItem;
        treasure.unlockedEvent = UnlockedTreasureEvent;

        interactiveSample.onPointerClick = (go) => { info.text = "Click"; };
        interactiveSample.onPointerExit = (go) => { info.text = "Exit"; };
    }

    private void Update()
    {
        if (controller == null)
        {
            return;
        }

        if (!treasure.unlocked)
        {
            ChooseItemProcess();
            if (currentLock != null)
            {
                UnlockProcess(currentLock.GetComponent<Lock>());
            }
        }
        else
        {
            if (firstGetKnife)
            {
                firstGetKnife = false;
                lever1.rotation = lever2.rotation = controllerAnchor.rotation * Quaternion.Euler(0, 180, 0);
                lastKnifeRotation = knife.rotation;
                cutTimer = 0;
            }
            else
            {
                float dot = Vector3.Dot(knife.forward, Vector3.right) / knife.forward.magnitude;
                float angle = Quaternion.Angle(knife.rotation, lastKnifeRotation);
                if (angle > 180)
                {
                    angle -= 180;
                }
                else if (angle < -180)
                {
                    angle += 180;
                }
                angle = Mathf.Abs(angle);
                info.text = dot + " " + angle;
                if (true || ((dot >= 0.75 || dot <= -0.75) && angle >= 3))
                {
                    cutTimer += Time.deltaTime;
                    if (cutTimer >= cutTime)
                    {
                        //rope.gameObject.SetActive(false);
                        victory.SetActive(true);
                        WatchDogBehavior.instance.Gameover = true;
                    }
                    else
                    {
                        info.text += " " + cutTimer.ToString();
                    }
                }
                lastKnifeRotation = knife.rotation;
                lever1.rotation = lever2.rotation = controllerAnchor.rotation * Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void OnStartButtonClicked()
    {
        startButton.gameObject.SetActive(false);
    }

    private void ChooseItemProcess()
    {
        Ray ray;
        if (controller.control == ControlManager.Control.Daydream)
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

        if (controller.SecondaryButtonDown)
        {
            if (currentItemRaycastHit != null)
            {
                info.text = currentItemRaycastHit.name;
            }
            //info.text = "SecondaryButtonDown";
            if (itemAttachedToLever1 == null)
            {
                if (currentItemRaycastHit != null && itemAttachedToLever2 == null)
                {
                    attractItem.Play();
                    info.text = currentItemRaycastHit.name;
                    currentItemRaycastHit.SetParent(lever1End, true);
                    currentItemRaycastHit.localPosition = Vector3.zero;
                    itemAttachedToLever1 = currentItemRaycastHit;
                    if (itemAttachedToLever1.CompareTag(CORRECT_ITEM))
                    {
                        currentLock = itemAttachedToLever1;
                    }
                }
                else
                {
                    info.text = "?????";
                }
            }
            else
            {
                info.text = "Drop " + itemAttachedToLever1.name;
                if (itemAttachedToLever1.position.y >= dropHeight1)
                {
                    bigDrop.Play();
                    if (SceneManager.GetActiveScene().name == "test")
                    {
                        WatchDogBehavior.instance.AngryRate += 20;
                    }
                }
                else if (itemAttachedToLever1.position.y >= dropHeight2)
                {
                    smallDrop.Play();
                }
                itemAttachedToLever1.SetParent(null);
                itemAttachedToLever1.position = itemAttachedToLever1.GetComponent<InteractiveObject>().originalPosition;
                itemAttachedToLever1.rotation = itemAttachedToLever1.GetComponent<InteractiveObject>().originalRotation;
                currentLock = itemAttachedToLever1 = null;
            }
        }

        if (itemAttachedToLever1 != null)
        {
            itemAttachedToLever1.rotation = controllerAnchor.rotation;
            if (itemAttachedToLever1.position.y >= dropHeight2)
            {
                WatchDogBehavior.instance.OverShorder = true;
            }
            else
            {
                WatchDogBehavior.instance.OverShorder = false;
            }
        }
        if (itemAttachedToLever2 != null)
        {
            itemAttachedToLever2.rotation = controllerAnchor.rotation;
            if (itemAttachedToLever2.position.y >= dropHeight2)
            {
                WatchDogBehavior.instance.OverShorder = true;
            }
            else
            {
                WatchDogBehavior.instance.OverShorder = false;
            }
        }
    }

    private void UnlockProcess(Lock @lock)
    {
        if (controller.IsTouchDown)
        {
            lastX = controller.TouchPos.x;
        }
        if (controller.IsTouching)
        {
            float delta = controller.TouchPos.x - lastX;
            lastX = controller.TouchPos.x;
            @lock.Slide(delta);
            if (@lock.done)
            {
                currentLock = null;
            }
        }
        if (controller.TouchpadButtonDown)
        {
            @lock.Confirm();
        }
    }

    private void OnUnlockItem(GameObject go)
    {
        go.transform.SetParent(lever2End, true);
        go.transform.localPosition = Vector3.zero;
        itemAttachedToLever2 = go.transform;
        if (itemAttachedToLever2 == itemAttachedToLever1)
        {
            itemAttachedToLever1 = null;
        }
    }

    private void UnlockedTreasureEvent()
    {
        if (itemAttachedToLever2 != null)
        {
            itemAttachedToLever2.gameObject.SetActive(false);
        }
        itemAttachedToLever2 = null;

        knife.gameObject.SetActive(true);
        knife.SetParent(lever1End, true);
        knife.localPosition = Vector3.zero;

        WatchDogBehavior.instance.SecondPhase = true;
    }
}