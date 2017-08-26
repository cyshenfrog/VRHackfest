using UnityEngine;

public class LookFollowPosition : LookFollow
{
    public Vector3 targetPosition;
    public override FollowType FollowingType
    {
        get
        {
            return FollowType.Position;
        }
    }
    public override bool ValidTarget
    {
        get
        {
            return true;
        }
        protected set { }
    }
    public override Vector3 CurrentPosition
    {
        get
        {
            return targetPosition;
        }
        protected set { }
    }

    public override void Initialize(Transform follow)
    {
        targetPosition = follow.position;
        base.Initialize(follow);
    }
}
