using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    private bool Adjusted = false;
    private float Cd = 0.5f;
    public GameObject PassUI;
    public GameObject AdjustUI;
    public GameObject GoodlukUI;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Adjusted)
        {
            if ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || GvrControllerInput.HomeButtonState || Input.GetMouseButton(0)))
            {
                Cd -= Time.deltaTime;
                if (Cd < 0)
                {
                    Adjusted = true;
                    PassUI.SetActive(true);
                    AdjustUI.SetActive(false);
                }
            }
            else
            {
                Cd = 0.5f;
            }
        }
        else
        {
            if (ControlManager.Instance.TouchpadButtonDown || Input.GetMouseButtonDown(0))
            {
                GoodlukUI.SetActive(true);
                PassUI.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}