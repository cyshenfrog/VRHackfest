using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchDogBehavior : MonoBehaviour
{
    public Transform NoticeTarget;
    public Transform AlertPos;
    public float AngryRate;

    private float FailValue = 100;
    private float WarningValue = 75;
    private float RecoverValue = 60;
    private float NoticeValue = 0;

    private bool SecondPhase;

    private Vector3 lastVRheadRot;
    private Vector3 delta;

    // Use this for initialization
    private void Start()
    {
        lastVRheadRot = NoticeTarget.eulerAngles;
    }

    // Update is called once per frame
    private void Update()
    {
        delta = NoticeTarget.eulerAngles - lastVRheadRot;
        NoticeValue += delta.magnitude * AngryRate;
        if (NoticeValue > FailValue)
        {
            Alert();
        }
        else if (NoticeValue > WarningValue)
        {
            Noticed();
        }
        else if (NoticeValue > RecoverValue)
        {
            Recover();
        }
        lastVRheadRot = NoticeTarget.eulerAngles;
    }

    private void Alert()
    {
    }

    private void Noticed()
    {
    }

    private void Recover()
    {
    }

    private void Fail()
    {
    }
}