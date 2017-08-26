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

    public float distanceForwardController = 1;
    public Transform lever1, lever2;

    public InteractiveObject interactiveSample;
    public Text info;

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

        items[0].onPointerClick = OnItemPointerClick;

        interactiveSample.onPointerClick = (go) => { info.text = "Click"; };
        interactiveSample.onPointerExit = (go) => { info.text = "Exit"; };
    }

    private void Update ()
    {
        if (_controller == null)
        {
            return;
        }
        Vector3 euler = _controller.Rotation.eulerAngles;
        lever1.rotation = Quaternion.Euler(euler.x, 0, euler.z);
    }

    public void OnStartButtonClicked()
    {
        startButton.gameObject.SetActive(false);
    }

    public void OnItemPointerEnter(GameObject go)
    {

    }

    public void OnItemPointerClick(GameObject go)
    {
        go.transform.SetParent(lever1.Find("End"), false);
    }
}
