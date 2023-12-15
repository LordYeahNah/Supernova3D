using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter
{
    // === Colonist Details === //
    protected string _FirstName;
    protected string _LastName;
    protected ESex _Sex;

    public string FirstName => _FirstName;
    public string LastName => _LastName;
    public string Name => $"{_FirstName} {_LastName}";
    public ESex Sex => _Sex;

    // === Health Settings === //
    protected float _CurrentHealth;
    public float CurrentHealth => _CurrentHealth;

    // === Components === //
    public Transform CharacterTransform;                               // Store reference to the characters transform
    protected Blackboard _BlackboardRef;
    protected BehaviorTree _Tree;

    public virtual void OnInitialize()
    {
        InitializeAI();
    }

    /// <summary>
    /// Called each frame
    /// </summary>
    public virtual void OnUpdate()
    {
        if(_Tree != null)
            _Tree.OnUpdate();
    }

    /// <summary>
    /// Initialize the AI
    /// </summary>
    protected virtual void InitializeAI()
    {
        // Create Blackboard properties 
        _BlackboardRef = new Blackboard();
        _BlackboardRef.SetValue<BaseCharacter>(GeneralKeys.SELF, this);
        _BlackboardRef.SetValue(GeneralKeys.MOVE_TO_LOCATION, Vector3.zero);
        _BlackboardRef.SetValue<BaseRoom>(GeneralKeys.WORKING_ROOM, null);
        _BlackboardRef.SetValue<BaseCharacter>(GeneralKeys.TARGET, null);

        // Override methods to create BT
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