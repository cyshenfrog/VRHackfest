using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFollowTest : MonoBehaviour
{
    public Transform target;
    public LookFollowPosition lookFollowTransform;
    public float moveSpeed = 1;
    public float rotateSpeed = 1;
    public float sensitivity = 30;
    private GameObject _follower;

    private void Start ()
    {
        _follower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _follower.transform.position = transform.position;
        Rigidbody rigidbody =_follower.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        lookFollowTransform = _follower.AddComponent<LookFollowPosition>();
        lookFollowTransform.moveSpeed = moveSpeed;
        lookFollowTransform.rotateSpeed = rotateSpeed;
        lookFollowTransform.sensitivity = sensitivity;
    }

    private void Update ()
    {
        if (lookFollowTransform.inited)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lookFollowTransform.Toggle(true);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                lookFollowTransform.Toggle(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!this.target)
                {
                    GameObject target = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    target.transform.position = transform.position + transform.forward * Random.Range(10, 20) + transform.right * Random.Range(10, 20) + transform.up * Random.Range(5, 10);
                    lookFollowTransform.Initialize(target.transform);
                }
                else
                {
                    lookFollowTransform.Initialize(this.target);
                }
            }
        }
    }
}
