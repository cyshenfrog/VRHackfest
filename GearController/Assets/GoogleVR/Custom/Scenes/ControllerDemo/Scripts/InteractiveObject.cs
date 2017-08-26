using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Action<GameObject> onPointerEnter;
    public Action<GameObject> onPointerExit;
    public Action<GameObject> onPointerClick;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void OnPointerEnter(GameObject go)
    {
        if (onPointerEnter != null)
        {
            onPointerEnter(go);
        }
    }

    public void OnPointerExit(GameObject go)
    {
        if (onPointerExit != null)
        {
            onPointerExit(go);
        }
    }

    public void OnPointerClick(GameObject go)
    {
        if (onPointerClick != null)
        {
            onPointerClick(go);
        }
    }
}
