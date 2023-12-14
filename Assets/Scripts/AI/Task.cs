using System.Collections.Generic;
using UnityEngine;

public enum ETaskStatus
{
    SUCCESS = 0,
    FAILURE = 1,
    RUNNING = 2
}

public abstract class Task
{
    public BehaviorTree _Tree;
    public Task Parent;
    protected List<Task> _Children = new List<Task>();

    public Task(BehaviorTree tree)
    {
        _Tree = tree;
    }

    public Task(BehaviorTree tree, List<Task> children)
    {
        _Tree = tree;

        foreach(var child in children)
            _AttachChild(child);
    }

    protected void _AttachChild(Task child)
    {
        _Children.Add(child);
        child.Parent = this;
    }

    public abstract ETaskStatus RunTask();
}