using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafNode 
{
    public LeafNode parent;
    public float runningCost;
    public List<KeyValuePair<string, object>> state;
    public Action action;

    public LeafNode(LeafNode parent, float runningCost, List<KeyValuePair<string, object>> state, Action action)
    {
        this.parent = parent;
        this.runningCost = runningCost;
        this.state = state;
        this.action = action;
    }
}
