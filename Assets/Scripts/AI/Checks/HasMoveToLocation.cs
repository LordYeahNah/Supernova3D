using System.Collections.Generic;
using UnityEngine;

public class HasMoveToLocation : Task
{
    public HasMoveToLocation(BehaviorTree tree) : base(tree)
    {
    }

    public override ETaskStatus RunTask()
    {
        // Validate tree and blackboard
        if(_Tree == null || _Tree.BlackboardRef == null)
            return ETaskStatus.FAILURE;

        Vector3 moveToLocation = _Tree.BlackboardRef.GetValue<Vector3>(GeneralKeys.MOVE_TO_LOCATION);
        if(moveToLocation != Vector3.zero)
            return ETaskStatus.SUCCESS;

        return ETaskStatus.FAILURE;
    }
}