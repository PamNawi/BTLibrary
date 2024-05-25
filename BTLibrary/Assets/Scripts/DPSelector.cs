using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSelector : PSelector
{
    public DPSelector(string n) : base(n)
    {
        name = n;
    }

    public override Node.Status Process()
    {
        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING) 
            return Status.RUNNING;

        if (childStatus == Status.SUCCESS)
        {
            children[currentChild].priorityOrder = 1;
            currentChild = 0;
            OrderNodes();
            return Status.SUCCESS;
        }

        children[currentChild].priorityOrder += 2;
        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            OrderNodes();
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
