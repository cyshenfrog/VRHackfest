using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public bool inited = false;
    public bool on;
    public enum Type
    {
        LookFollow
    }
    public virtual Type MoveType { get; set; }

    protected virtual void Start()
    {
        //enabled = false;
    }

    public virtual void Initialize()
    {
        inited = true;
    }

    public virtual void Toggle(bool on)
    { }

    protected virtual void OnDisable()
    {
        inited = false;
    }
}
