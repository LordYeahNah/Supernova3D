using System.Collections.Generic;
using UnityEngine;

public class ColonistBT : BehaviorTree
{
    private MoveToLocation _GeneralMoveTask;

    public ColonistBT(Blackboard blackboardRef) : base(blackboardRef)
    {
        
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _GeneralMoveTask = new MoveToLocation(this);
    }

    protected override Task CreateTree()
    {
        return new Selector(this, new List<Task>
        {
            new Sequence(this, new List<Task>
            {
                new HasMoveToLocation(this, _GeneralMoveTask),
                _GeneralMoveTask
            }) 
        });
    }
}