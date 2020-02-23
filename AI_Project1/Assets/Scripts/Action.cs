using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public Dictionary<string, object> actionPreconditions;
    public Dictionary<string, object> actionEffects;

    // Action cost
    public float cost = 1.0f;
    private bool inRange = false;
    public GameObject target;

    public Action()
    {
        actionPreconditions = new Dictionary<string, object>();
        actionEffects = new Dictionary<string, object>();
    }

    public void ResetVariables()
    {
        target = null;
        Reset();
    }

    public abstract void Reset();

    public abstract bool ActionCompleted();

    public abstract bool CheckProceduralPrecondition(GameObject agent);

    public abstract bool DoAction(GameObject agent);

    public void AddActionPrecondition(string key, object value)
    {
        actionPreconditions.Add(key, value);
    }

    public void RemoveActionPrecondition(string key)
    {
        actionPreconditions.Remove(key);
    }

    public void AddActionEffect(string key, object value)
    {
        actionEffects.Add(key, value);
    }

    public void RemoveActionEffect(string key)
    {
        actionEffects.Remove(key);
    }

    public abstract bool IsRangeBased();


    // Make a property?
    public void setInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    public bool WithinRange()
    {
        return inRange;
    }
}
