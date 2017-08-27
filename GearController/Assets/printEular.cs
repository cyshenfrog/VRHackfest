using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class printEular : MonoBehaviour
{
    private Text text;
    public Transform target;

    // Use this for initialization
    private void Start()
    {
        text = GetComponent<Text>();
        target = Camera.main.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        text.text = target.eulerAngles.ToString();
    }
}