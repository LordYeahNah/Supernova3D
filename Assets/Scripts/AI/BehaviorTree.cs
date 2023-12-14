using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree
{
    private Task _RootTask;
    private Blackboard _BlackboardRef;
    public Blackboard BlackboardRef => _BlackboardRef;

    public BehaviorTree(Blackboard blackboardRef)
    {
        _BlackboardRef = blackboardRef;
        _RootTask = CreateTree();
    }

    public void OnUpdate()
    {
        if(_RootTask == null)
            return;

        _RootTask.RunTask();
    }

    protected abstract Task CreateTree();
}