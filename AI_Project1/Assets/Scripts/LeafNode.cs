using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafNode 
{
    public LeafNode parent;
    public float runningCost;
    public Dictionary<string, object> state;
    public Action action;

    public LeafNode(LeafNode parent, float runningCost, Dictionary<string, object> state, Action action)
    {
        this.parent = parent;
        this.runningCost = runningCost;
        this.state = state;
        this.action = action;
    }
}
