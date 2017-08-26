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
    public Transform WallPos;
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
    private bool GoChecking = false;
    private bool toggleTarget = false;
    private bool secondPhase = false;

    public bool SecondPhase
    {
        get
        {
            return secondPhase;
        }
        set
        {
            secondPhase = value;
            if (!secondPhase)
            {
                OnSecondPhase();
            }
        }
    }

    public bool OverShorder = false;
    private bool CanAccumulate = true;
    private Vector3 posBeforeWarning;
    private Vector3 lastVRheadRot;
    private Vector3 delta;
    private float degreeOffset;
    private int turnLevel;
    private float LookingCd = 10;
    public static WatchDogBehavior instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        WalkAround();
        Invoke("OnSecondPhase", 3);
        //Move(Vector3.forward * 10, 0, 2, () => { });
        lastVRheadRot = NoticeTarget.eulerAngles;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!CanAccumulate) return;

        if (secondPhase)
        {
            if (!suspicion && !GoChecking)
            {
                if (LookingCd > 0)
                {
                    LookingCd -= Time.deltaTime;
                }
                else
                {
                    LookingCd = UnityEngine.Random.Range(8, 13);
                    TriggerAnim(AnimList.LookAround);
                }
            }

            delta = NoticeTarget.eulerAngles - lastVRheadRot;
            NoticeValue += delta.magnitude * AngryRate;

            lastVRheadRot = NoticeTarget.eulerAngles;
        }
        else
        {
            degreeOffset = NoticeTarget.eulerAngles.y > 180 ? Mathf.Abs(NoticeTarget.eulerAngles.y - 360) : NoticeTarget.eulerAngles.y;
            turnLevel = Mathf.CeilToInt(degreeOffset / 15);
            switch (turnLevel)
            {
                case 0:
                    NoticeValue -= 0.05f * AngryRate;
                    break;

                case 1:
                    NoticeValue -= 0.05f * AngryRate;
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
                    NoticeValue += 0.9f * AngryRate;
                    break;
            }

            if (OverShorder)
            {
                NoticeValue += 0.1f * AngryRate;
            }
        }

        if (NoticeValue > FailValue)
        {
            Alert();
        }
        else if (NoticeValue > WarningValue && !suspicion && !GoChecking)
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
    }

    private void Alert()
    {
        posBeforeWarning = transform.position;
        CanAccumulate = false;

        Move(AlertPos.position, 0, 2.5f, () =>
         {
             TriggerAnim(AnimList.Atk);
             Recover();
         });
        NoticeValue = 0;
    }

    private void Noticed()
    {
        GoChecking = true;
        posBeforeWarning = transform.position;
        if (secondPhase)
        {
            transform.DOKill();
            transform.DOLookAt(NoticeTarget.position, 0.5f)
                .OnComplete(() => { suspicion = true; });
        }
        else
        {
            Move(NoticePos.position, 0, 1.5f, () =>
            {
                suspicion = true;
                transform.DOLookAt(NoticeTarget.position, 0.5f);
                TriggerAnim(AnimList.Idle);
            });
        }
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
        GoChecking = false;
        Move(posBeforeWarning, 2, 1, () =>
        {
            if (secondPhase)
            {
                TriggerAnim(AnimList.Idle);
                transform.DOLookAt(WallPos.position, 0.5f);
            }
            else
                WalkAround();
        });
    }

    public void OnSecondPhase()
    {
        secondPhase = true;
        Move(NoticePos.position, 0, 2, () =>
        {
            transform.DOLookAt(NoticeTarget.position, 0.5f).OnComplete(() =>
            {
                TriggerAnim(AnimList.Idle);
                transform.DOLookAt(WallPos.position, 1f).SetDelay(2f);
            });
            TriggerAnim(AnimList.Angry);
        });
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
        transform.DOLookAt(pos, 0.5f)
            .OnStart(() =>
            {
                TriggerAnim(AnimList.Walk);
            })
            .SetEase(Ease.Linear)
            .SetDelay(delay);
        transform.DOMove(pos, (transform.position - pos).magnitude / speed)
            .SetEase(Ease.Linear)
            .SetDelay(delay)
            .OnComplete(() => { onFinish(); });
    }
}