using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSelector : Node
{
    Node[] nodeArray;
    bool shuffled = false;

    public RSelector(string n)
    {
        name = n;
    }

    // Método para embaralhar (shuffle) os nós
    public void ShuffleNodes()
    {
        nodeArray = children.ToArray();
        int n = nodeArray.Length;
        System.Random rng = new System.Random();
        while (n > 1)
        {
            int k = rng.Next(n--);
            Node temp = nodeArray[n];
            nodeArray[n] = nodeArray[k];
            nodeArray[k] = temp;
        }
        children = new List<Node>(nodeArray);
    }

    public override Node.Status Process()
    {
        if (!shuffled)
        {
            ShuffleNodes();
            shuffled = true;
        }
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCESS)
        {
            shuffled = false;
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            shuffled = false;
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
