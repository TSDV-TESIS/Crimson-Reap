using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;
using UnityEngine.UIElements;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Selector", category: "Flow", id: "14cb321ef07234324cf7f6424864a93c")]
public partial class SelectorSequence : Composite
{
    [CreateProperty] int m_CurrentChild;
    
    protected override Status OnStart()
    {
        return StartSequence();
    }

    protected override Status OnUpdate()
    {
        Node currentChild = Children[m_CurrentChild];
        Status childStatus = currentChild.CurrentStatus;

        if (childStatus == Status.Failure)
        {
            return StartChild(++m_CurrentChild);
        }

        if (childStatus == Status.Success)
        {
            Debug.Log(currentChild);
            Debug.Log(currentChild.CurrentStatus);
            ResetStatus();
            CurrentStatus = Status.Running;
            return StartSequence();
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    protected Status StartChild(int childIndex)
    {
        if (m_CurrentChild >= Children.Count)
        {
            return Status.Failure;
        }
        var childStatus = StartNode(Children[childIndex]);

        return childStatus switch
        {
            Status.Success => Status.Success,
            Status.Running => Status.Waiting,
            Status.Failure => Status.Running,
            _ => childStatus
        };
    }

    private Status StartSequence()
    {
        m_CurrentChild = 0;
        return StartChild(m_CurrentChild);
    }
}

