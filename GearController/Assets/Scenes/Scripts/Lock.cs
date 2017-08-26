using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public Transform[] rings;
    public float[] targetDegrees;
    public float[] yDegrees;
    public int current;
    public bool solved;

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
		
	}

    public void Slide(float delta)
    {
        if (solved)
        {
            return;
        }
        int len = rings.Length;
        if (delta > 0.0625f)
        {
            if (len == 4)
            {
                rings[current].Rotate(Vector3.up, 90, Space.Self);
                if (rings[current].localEulerAngles.y == targetDegrees[current])
                {
                    current++;
                    if (current >= rings.Length)
                    {
                        current = 0;
                        solved = true;
                    }
                }
            }
        }
        else if (delta < -0.0625)
        {
            if (len == 4)
            {
                rings[current].Rotate(Vector3.up, -90, Space.Self);
                if (rings[current].localEulerAngles.y == targetDegrees[current])
                {
                    current++;
                    if (current >= rings.Length)
                    {
                        current = 0;
                        solved = true;
                    }
                }
            }
        }
    }
}
