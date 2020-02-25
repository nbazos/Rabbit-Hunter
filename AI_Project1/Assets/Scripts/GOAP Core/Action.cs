using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract parent class that all created actions will inherit
public abstract class Action : MonoBehaviour
{
    public Dictionary<string, object> actionPreconditions;
    public Dictionary<string, object> actionEffects;

    // Action attributes
    public float cost = 1.0f;
    private bool inRange = false;
    public GameObject target;

    public Action()
    {
        actionPreconditions = new Dictionary<string, object>();
        actionEffects = new Dictionary<string, object>();
    }

    // Reset this action's variables
    public void ResetVariables()
    {
        target = null;
        Reset();
    }

    // Reset the child's action specific variables
    public abstract void Reset();

    // Return boolean for this action to represent if it has been completed or not
    public abstract bool ActionCompleted();

    // Complete the necessary steps so this action can be done
    public abstract bool CheckProceduralPrecondition(GameObject agent);

    // Do the action itself (if range is required for this action then you are already at the necessary location)
    public abstract bool DoAction(GameObject agent);

    // Add a precondition to the dictionary
    public void AddActionPrecondition(string key, object value)
    {
        actionPreconditions.Add(key, value);
    }

    // Remove a precondition from the dictionary
    public void RemoveActionPrecondition(string key)
    {
        actionPreconditions.Remove(key);
    }

    // Add an effect to the dictionary
    public void AddActionEffect(string key, object value)
    {
        actionEffects.Add(key, value);
    }

    // Remove an effect from the dictionary
    public void RemoveActionEffect(string key)
    {
        actionEffects.Remove(key);
    }

    // Is the child aciton range based?
    public abstract bool IsRangeBased();

    // Set "in rnage" property of the action
    public void SetInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    // Return the boolean for "In Range"
    public bool WithinRange()
    {
        return inRange;
    }
}
