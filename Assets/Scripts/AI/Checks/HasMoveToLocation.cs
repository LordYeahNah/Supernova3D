using System.Collections.Generic;
using UnityEngine;

public class HasMoveToLocation : Task
{
    private MoveToLocation _MoveTask;
    public HasMoveToLocation(BehaviorTree tree, MoveToLocation moveTask) : base(tree)
    {
        _MoveTask = moveTask;
    }

    public override ETaskStatus RunTask()
    {
        // Validate tree and blackboard
        if(_Tree == null || _Tree.BlackboardRef == null)
            return ETaskStatus.FAILURE;

        Vector3 moveToLocation = _Tree.BlackboardRef.GetValue<Vector3>(GeneralKeys.MOVE_TO_LOCATION);
        if(moveToLocation != Vector3.zero)
            return ETaskStatus.SUCCESS;

        if(_MoveTask != null)
            _MoveTask._HasPolled = false;
            
        return ETaskStatus.FAILURE;
    }
}