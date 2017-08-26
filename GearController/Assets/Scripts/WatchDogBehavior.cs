using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class WatchDogBehavior : MonoBehaviour
{
    public Transform NoticeTarget;
    public Transform NoticePos;
    public Transform AlertPos;
    public Transform[] WalkAroundTarget;
    public float AngryRate;
    public Animator anim;
    public Text debugtext;

    private enum AnimList
    {
        Idle,
        Walk,
        Atk,
        LookAround,
        Run,
        Angry
    }

    private float FailValue = 200;
    private float WarningValue = 120;
    private float RecoverValue = 80;
    private float NoticeValue = 0;

    private bool suspicion = false;
    private bool toggleTarget = false;
    private bool SecondPhase = false;
    private bool CanAccumulate = true;
    private Vector3 posBeforeWarning;
    private Vector3 lastVRheadRot;
    private float degreeOffset;
    private int turnLevel;

    // Use this for initialization
    private void Start()
    {
        WalkAround();
        //Move(Vector3.forward * 10, 0, 2, () => { });
        lastVRheadRot = NoticeTarget.eulerAngles;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!CanAccumulate) return;

        degreeOffset = NoticeTarget.eulerAngles.y > 180 ? Mathf.Abs(NoticeTarget.eulerAngles.y - 360) : NoticeTarget.eulerAngles.y;
        turnLevel = Mathf.CeilToInt(degreeOffset / 45);
        switch (turnLevel)
        {
            case 1:
                NoticeValue -= 0.5f * AngryRate;
                break;

            case 2:
                NoticeValue += 0.2f * AngryRate;
                break;

            case 3:
                NoticeValue += 0.5f * AngryRate;
                break;

            case 4:
                NoticeValue += 0.9f * AngryRate;
                break;

            default:

                break;
        }

        if (NoticeValue > FailValue)
        {
            Alert();
        }
        else if (NoticeValue > WarningValue && !suspicion)
        {
            Noticed();
        }
        else if (NoticeValue < RecoverValue && suspicion)
        {
            Recover();
        }

        if (NoticeValue < 0)
            NoticeValue = 0;

        debugtext.text = NoticeValue.ToString();
        lastVRheadRot = NoticeTarget.eulerAngles;
    }

    private void Alert()
    {
        posBeforeWarning = transform.position;
        CanAccumulate = false;

        Move(AlertPos.position, 0, 2, () =>
         {
             TriggerAnim(AnimList.Atk);
             Recover();
         });
        NoticeValue = 0;
    }

    private void Noticed()
    {
        suspicion = true;
        posBeforeWarning = transform.position;
        Move(NoticePos.position, 0, 1, () =>
        {
            TriggerAnim(AnimList.Idle);
        });
    }

    private void WalkAround()
    {
        Move(WalkAroundTarget[toggleTarget ? 0 : 1].position, 0, 1, () => { WalkAround(); });
        toggleTarget = !toggleTarget;
    }

    private void Recover()
    {
        suspicion = false;
        CanAccumulate = true;
        Move(posBeforeWarning, 2, 1, () =>
        {
            WalkAround();
        });
    }

    private void Fail()
    {
    }

    private void TriggerAnim(AnimList state)
    {
        switch (state)
        {
            case AnimList.Idle:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                break;

            case AnimList.Walk:
                anim.SetBool("Walk", true);
                break;

            case AnimList.Atk:
                anim.SetTrigger("Attack");
                break;

            case AnimList.LookAround:
                anim.SetTrigger("Look");
                break;

            case AnimList.Angry:
                anim.SetTrigger("Angry");
                break;

            case AnimList.Run:
                anim.SetBool("Run", true);
                break;

            default:
                break;
        }
    }

    private void Move(Vector3 pos, float delay, float speed, Action onFinish)
    {
        transform.DOKill();
        TriggerAnim(AnimList.Walk);
        transform.DOLookAt(pos, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(delay);
        transform.DOMove(pos, (transform.position - pos).magnitude / speed)
            .SetEase(Ease.Linear)
            .SetDelay(delay)
            .OnComplete(() => { onFinish(); });
    }
}