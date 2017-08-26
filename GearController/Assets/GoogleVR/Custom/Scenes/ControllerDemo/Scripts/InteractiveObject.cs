using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Action onPointerEnter;
    public Action onPointerExit;
    public Action onPointerClick;
    public ControllerDemo main;

    public void OnPointerEnter()
    {
        if (onPointerEnter != null)
        {
            onPointerEnter();
        }
        //main.info.text = "Cube OnPointerEnter";
        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void OnPointerExit()
    {
        if (onPointerExit != null)
        {
            onPointerExit();
        }
        //main.info.text = "Cube OnPointerExit";
        //transform.localScale = Vector3.one;
    }

    public void OnPointerClick()
    {
        if (onPointerClick != null)
        {
            onPointerClick();
        }
        //main.info.text = "Cube OnPointerClick";
        //transform.localScale = Vector3.one * 1.5f;
    }
}
