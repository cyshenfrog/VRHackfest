using UnityEngine;

public class LookFollowTransform : LookFollow
{
    public Transform follow;
    public override FollowType FollowingType
    {
        get
        {
            return FollowType.Transform;
        }
    }
    public override bool ValidTarget
    {
        get
        {
            return (follow != null) && follow.gameObject.activeInHierarchy;
        }
        protected set { }
    }
    public override Vector3 CurrentPosition
    {
        get
        {
            return follow.position;
        }
        protected set { }
    }

    public override void Initialize(Transform follow)
    {
        this.follow = follow;
        base.Initialize(follow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        follow = null;
    }
}
