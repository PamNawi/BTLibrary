using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE };
    public Status status;

    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public string ChildrenNames()
    {
        string s = "";
        if (children.Count == 0)
        {
            return name + ": []\n";
        }
        else
        {
            s = name + ":\t[";
            foreach(Node c in children)
            {
                s += c.name+ ",";
            }
            s = s.Remove(s.Length - 1);
            s += "]\n";
            foreach (Node c in children)
            {
                s += c.ChildrenNames();
            }
            return s;
        }
    }
}
