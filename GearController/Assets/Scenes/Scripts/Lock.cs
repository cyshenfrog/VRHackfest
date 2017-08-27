using System;
using DG.Tweening;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public int scaleLength = 4;
    public Transform[] rings;
    public Transform[] answers;
    public Quaternion[] originalRotations;
    public int current;
    public bool done;
    public bool solved;
    public bool transition;
    public Action<GameObject> onUnlocked;
    public bool isRight;
    public bool isTotalRight = true;
    public int id;
    public AudioSource slide;
    public AudioSource confirm;

    private void Awake()
    {
        originalRotations = new Quaternion[rings.Length];
        for (int i = 0; i < rings.Length; i++)
        {
            originalRotations[i] = rings[i].rotation;
        }
    }

	private void Start ()
    {
        
    }

    public void Reset()
    {
        current = 0;
        isTotalRight = true;
        done = isRight = solved = false;
        InteractiveObject interactiveObject = gameObject.GetComponent<InteractiveObject>();
        transform.SetParent(Game.Instance.environment, true);
        transform.position = interactiveObject.originalPosition;
        transform.rotation = interactiveObject.originalRotation;
        for (int i = 0; i < rings.Length; i++)
        {
            rings[i].gameObject.SetActive(true);
            rings[i].rotation = originalRotations[i];
        }
        if (Game.Instance.itemAttachedToLever1 == transform)
        {
            Game.Instance.currentLock = Game.Instance.itemAttachedToLever1 = null;
        }
        Game.Instance.itemAttachedToLever2 = null;
    }

    private void Update ()
    {
		if (Input.GetKeyDown(KeyCode.A))
        {
            Slide(0.25f);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Slide(-0.25f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Confirm();
        }
    }

    public void Slide(float delta)
    {
        if (solved || done || transition)
        {
            return;
        }
        slide.Play();
        if (delta > 0.125f)
        {
            RotateTransition(360 / scaleLength);
        }
        else if (delta < -0.125)
        {
            RotateTransition(-360 / scaleLength);
        }
    }

    private void RotateTransition(float angle)
    {
        Game.Instance.info.text = current.ToString();
        transition = true;
        Quaternion before = rings[current].localRotation;
        rings[current].Rotate(Vector3.forward, angle, Space.Self);
        Quaternion after = rings[current].localRotation;
        rings[current].localRotation = before;
        rings[current].DOLocalRotate(after.eulerAngles, 0.125f).OnComplete(() =>
        {
            isRight = Mathf.Abs(rings[current].localRotation.eulerAngles.z - answers[current].localRotation.eulerAngles.z) < 0.0078125f;
            transition = false;
        });
    }

    public void Confirm()
    {
        if (!transition)
        {
            confirm.Play();
            isRight = Mathf.Abs(rings[current].localRotation.eulerAngles.z - answers[current].localRotation.eulerAngles.z) < 0.0078125f;
            isTotalRight = isTotalRight && isRight;
            rings[current].localRotation = answers[current].localRotation;
            current++;
            Game.Instance.info.text = current.ToString();
            if (current >= rings.Length)
            {
                current = 0;
                if (onUnlocked != null)
                {
                    onUnlocked(gameObject);
                }
                done = true;
                solved = isTotalRight;
            }
            else
            {
                //rings[current - 1].gameObject.SetActive(false);
            }
        }
    }
}
