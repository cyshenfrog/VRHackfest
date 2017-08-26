using UnityEngine;

public class LookFollow : ObjectMovement
{
    public override Type MoveType
    {
        get { return Type.LookFollow; }
        set { }
    }

    public float sensitivity = 180;
    public float moveSpeed = 1;
    public float rotateSpeed = 1;
    private Rigidbody _rigidbody;
    public enum FollowType
    {
        Transform,
        Position
    }
    public virtual FollowType FollowingType
    {
        get; set;
    }
    public virtual Vector3 CurrentPosition { get; protected set; }
    public virtual bool ValidTarget { get; protected set; }

    protected override void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody)
        {
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }
        base.Start();
    }

    public virtual void Initialize(Transform follow)
    {
        base.Initialize();
    }

    public override void Toggle(bool on)
    {
        this.on = enabled = on && inited && ValidTarget;
    }

    private void Update ()
    {
        if (!ValidTarget)
        {
            enabled = false;
        }

        Quaternion lookRot = Quaternion.LookRotation(CurrentPosition - transform.position);
        float angle = Quaternion.Angle(transform.rotation, lookRot);
        if (angle >= 180)
        {
            angle -= 180;
        }
        else if (angle <= -180)
        {
            angle += 180;
        }
        if (Mathf.Abs(angle) > sensitivity)
        {
            Debug.LogFormat("abs angle > {0} : {1}", sensitivity, angle);
        }
        else
        {
            Quaternion q = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
            if (_rigidbody)
            {
                _rigidbody.MoveRotation(q);
                // or
                //Quaternion q = lookRot * Quaternion.Inverse(transform.rotation);
                //_rigidbody.AddTorque(q.x * 16, q.y * 16, q.z * 16, ForceMode.Force);

                _rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * moveSpeed);
                // or
                //_rigidbody.AddForce(transform.forward * moveSpeed, ForceMode.Force);
            }
            else
            {
                transform.rotation = q;
                transform.Translate(transform.forward * Time.deltaTime * moveSpeed);
            }
        }
    }

    protected override void OnDisable()
    {
        on = enabled = inited = false;
    }
}
