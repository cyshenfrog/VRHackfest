using DG.Tweening;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public Transform[] rings;
    public Transform[] answers;
    public float[] yDegrees;
    public int current;
    public bool solved;
    public bool transition;

    private void Awake()
    {
        yDegrees = new float[rings.Length];
    }

	private void Start ()
    {
        current = 0;
    }

    private void Update ()
    {
		if (Input.GetKeyDown(KeyCode.A))
        {
            Slide(0.125f);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Slide(-0.125f);
        }
    }

    public void Slide(float delta)
    {
        if (solved || transition)
        {
            return;
        }
        int len = rings.Length;
        if (delta > 0.0625f)
        {
            RotateTransition(360 / len);
        }
        else if (delta < -0.0625)
        {
            RotateTransition(-360 / len);
        }
    }

    private void RotateTransition(float angle)
    {
        transition = true;
        Quaternion before = rings[current].localRotation;
        rings[current].Rotate(Vector3.forward, angle, Space.Self);
        Quaternion after = rings[current].localRotation;
        rings[current].localRotation = before;
        rings[current].DOLocalRotate(after.eulerAngles, 0.125f).OnComplete(() =>
        {
            if (Mathf.Abs(rings[current].localRotation.eulerAngles.z - answers[current].localRotation.eulerAngles.z) < 0.0078125f)
            {
                rings[current].localRotation = answers[current].localRotation;
                current++;
                if (current >= rings.Length)
                {
                    current = 0;
                    transform.localScale = transform.localScale * 1.125f;
                    solved = true;
                }
            }
            transition = false;
        });
    }
}
