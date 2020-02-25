using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A node structure to be used when building a planning tree
public class LeafNode 
{
    public LeafNode parentNode;
    public float costSoFar;
    public Dictionary<string, object> state;
    public Action action;

    public LeafNode(LeafNode parentNode, float costSoFar, Dictionary<string, object> state, Action action)
    {
        this.parentNode = parentNode;
        this.costSoFar = costSoFar;
        this.state = state;
        this.action = action;
    }
}
