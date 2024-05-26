using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public delegate Status TickM(int index);
    public TickM ProcessMethodM;
    public int index;

    public Leaf() { }

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public Leaf(string n, int index, TickM pm)
    {
        name = n;
        ProcessMethodM = pm;
        this.index = index;
    }

    public Leaf(string n, Tick pm, int order)
    {
        name = n;
        ProcessMethod = pm;
        priorityOrder = order;
    }

    public override Status Process()
    {
        if(ProcessMethod != null)
            return ProcessMethod();

        else if (ProcessMethodM != null)
            return ProcessMethodM(index);

        return Status.FAILURE;
    }
}
