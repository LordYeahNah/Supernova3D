using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter
{
    public Transform CharacterTransform;                               // Store reference to the characters transform
    protected Blackboard _BlackboardRef;
    protected BehaviorTree _Tree;

    protected virtual void OnUpdate()
    {
        if(_Tree != null)
            _Tree.OnUpdate();
    }

    protected virtual void InitializeAI()
    {
        _BlackboardRef = new Blackboard();
        _BlackboardRef.SetValue<BaseCharacter>(GeneralKeys.SELF, this);
        _BlackboardRef.SetValue(GeneralKeys.MOVE_TO_LOCATION, Vector3.zero);
        _BlackboardRef.SetValue<BaseRoom>(GeneralKeys.WORKING_ROOM, null);
        _BlackboardRef.SetValue<BaseCharacter>(GeneralKeys.TARGET, null);
    }

    /// <summary>
    /// Stops the character from moving anymore
    /// </summary>
    public virtual void StopMovement()
    {
        _BlackboardRef.SetValue(GeneralKeys.MOVE_TO_LOCATION, Vector3.zero);
    }

    /// <summary>
    /// Sets the location for the character to move
    /// </summary>
    /// <param name="location">Location to move to</param>
    public void SetLocation(Vector3 location)
    {
        _BlackboardRef.SetValue(GeneralKeys.MOVE_TO_LOCATION, location);
    }
}