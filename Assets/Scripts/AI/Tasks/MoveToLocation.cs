using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MoveToLocation : Task
{
    private readonly float STOPPING_DISTANCE = 1.0f;
    public MoveToLocation(BehaviorTree tree) : base(tree)
    {
    }

    public override ETaskStatus RunTask()
    {
        if(_Tree == null)
            return ETaskStatus.FAILURE;

        Blackboard board = _Tree.BlackboardRef;                     // Get reference to the blackboard

        if(board != null)
        {
            BaseCharacter character = board.GetValue<BaseCharacter>(GeneralKeys.SELF);
            if(character != null)
            {
                Vector3 moveToLocation = board.GetValue<Vector3>(GeneralKeys.MOVE_TO_LOCATION);
                Vector3 currentLocation = character.CharacterTransform.position;

                float distance = Vector3.Distance(moveToLocation, currentLocation);
                if(distance < STOPPING_DISTANCE)
                {
                    character.StopMovement();
                    return ETaskStatus.SUCCESS;
                }

                return ETaskStatus.RUNNING;
            }
        }

        return ETaskStatus.FAILURE;
    }
}