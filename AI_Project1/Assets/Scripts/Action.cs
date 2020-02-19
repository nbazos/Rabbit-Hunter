using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public List<KeyValuePair<string, object>> actionPreconditions;
    public List<KeyValuePair<string, object>> actionEffects;

    // Action cost
    public float cost = 1.0f;
    private bool inRange = false;
    public GameObject target;

    public Action()
    {
        actionPreconditions = new List<KeyValuePair<string, object>>();
        actionEffects = new List<KeyValuePair<string, object>>();
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
        actionPreconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemoveActionPrecondition(string key)
    {
        int indexToRemove = 0;

        for (int i = 0; i < actionPreconditions.Count; i++)
        {
            if(actionPreconditions[i].Key.Equals(key))
            {
                indexToRemove = i;
            }
        }

        actionPreconditions.RemoveAt(indexToRemove);
    }

    public void AddActionEffect(string key, object value)
    {
        actionEffects.Add(new KeyValuePair<string, object>());
    }

    public void RemoveActionEffect(string key)
    {
        int indexToRemove = 0;

        for (int i = 0; i < actionEffects.Count; i++)
        {
            if (actionEffects[i].Key.Equals(key))
            {
                indexToRemove = i;
            }
        }

        actionPreconditions.RemoveAt(indexToRemove);
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
