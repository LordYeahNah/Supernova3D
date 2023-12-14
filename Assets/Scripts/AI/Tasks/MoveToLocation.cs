using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MoveToLocation : Task
{
    private readonly float STOPPING_DISTANCE = 2.0f;
    private readonly float POLL_TIME = 0.5f;
    private float _TimeSinceLastPoll = 0f;
    public bool _HasPolled = false;
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
                } else 
                {
                    if(!_HasPolled)
                    {
                        Poll(character, moveToLocation);
                    } else 
                    {
                        _TimeSinceLastPoll += 1 * Time.deltaTime;
                        if(_TimeSinceLastPoll > POLL_TIME)
                            Poll(character, moveToLocation);
                    }
                    return ETaskStatus.RUNNING;
                }

                
            }
        }

        return ETaskStatus.FAILURE;
    }

    private void Poll(BaseCharacter character, Vector3 moveToLocation)
    {
        if(character != null)
        {
            if(character is BaseColonist colonist)
            {
                colonist.Controller.SetMoveToLocation(moveToLocation);
                _HasPolled = true;
                _TimeSinceLastPoll = 0f;
            }
        }
    }
}