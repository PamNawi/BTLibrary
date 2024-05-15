using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        this.name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        return children[currentChild].Process();
    }

    public void PrintTree()
    {
        string s = "";
        s += "Behaviour Tree:\t" + this.name + "\n";
        s += ChildrenNames();
        Debug.Log(s);
    }
}