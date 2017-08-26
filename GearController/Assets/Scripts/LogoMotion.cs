using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMotion : MonoBehaviour
{
    private RectTransform trans;
    public Transform cam;
    private float yAxisModified;

    // Use this for initialization
    private void Start()
    {
        trans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        yAxisModified = cam.eulerAngles.y > 180 ? cam.eulerAngles.y - 360 : cam.eulerAngles.y;
        yAxisModified *= (478 / 180);
        trans.anchoredPosition = new Vector2(-yAxisModified, 0);
    }
}