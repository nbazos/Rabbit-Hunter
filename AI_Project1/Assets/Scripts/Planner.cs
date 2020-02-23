﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planner
{
    public Queue<Action> Plan(GameObject agent, List<Action> possibleActions, Dictionary<string, object> stateOfWorld, Dictionary<string, object> goal)
    {
        foreach (Action a in possibleActions)
        {
            a.ResetVariables();
        }

        List<Action> functionalActions = new List<Action>();
        
        foreach(Action a in possibleActions)
        {
            if (a.CheckProceduralPrecondition(agent)) 
            {
                functionalActions.Add(a);
            }
        }

        List<LeafNode> leaves = new List<LeafNode>();

        LeafNode start = new LeafNode(null, 0, stateOfWorld, null);
        
        bool success = BuildGraph(start, leaves, functionalActions, goal);

        if (!success)
        {
            return null;
        }

        // get the cheapest leaf
        LeafNode cheapest = null;
        leaves.Sort((A, B) => A.runningCost.CompareTo(B.runningCost));
        cheapest = leaves[0];

        // get its LeafNode and work back through the parents
        List<Action> result = new List<Action>();
        LeafNode n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action); // insert the action in the front
            }
            n = n.parent;
        }
        // we now have this action list in correct order

        Queue<Action> queue = new Queue<Action>();
        foreach (Action a in result)
        {
            queue.Enqueue(a);
        }

        // hooray we have a plan!
        return queue;
    }

    /**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
    private bool BuildGraph(LeafNode parent, List<LeafNode> leaves, List<Action> functionalActions, Dictionary<string, object> goal)
    {
        bool foundOne = false;

        // go through each action available at this LeafNode and see if we can use it here
        foreach (Action action in functionalActions)
        {

            // if the parent state has the conditions for this action's preconditions, we can use it here
            if (InState(action.actionPreconditions, parent.state))
            {

                // apply the action's effects to the parent state
                Dictionary<string, object> currentState = PopulateState(parent.state, action.actionEffects);
                LeafNode LeafNode = new LeafNode(parent, parent.runningCost + action.cost, currentState, action);

                if (InState(goal, currentState))
                {
                    // we found a solution!
                    leaves.Add(LeafNode);
                    foundOne = true;
                }
                else
                {
                    // not at a solution yet, so test all the remaining actions and branch out the tree
                    List<Action> subset = ActionSubset(functionalActions, action);
                    bool found = BuildGraph(LeafNode, leaves, subset, goal);
                    if (found)
                    {
                        foundOne = true;
                    }
                }
            }
        }

        return foundOne;
    }

    /**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
    private List<Action> ActionSubset(List<Action> actions, Action removeMe)
    {
        List<Action> subset = new List<Action>();
        foreach (Action a in actions)
        {
            if (!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }
        return subset;
    }

    /**
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
    private bool InState(Dictionary<string, object> test, Dictionary<string, object> state)
    {
        bool allMatch = true;
        foreach (KeyValuePair<string, object> t in test)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                allMatch = false;
            }
        }

        return allMatch;
    }

    /**
	 * Apply the stateChange to the currentState
	 */
    private Dictionary<string, object> PopulateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        // copy the KVPs over as new objects
        foreach (KeyValuePair<string, object> s in currentState)
        {
            state.Add(s.Key, s.Value);
        }

        foreach (KeyValuePair<string, object> change in stateChange)
        {
            // if the key exists in the current state, update the Value
            bool exists = false;

            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(change))
                {
                    exists = true;
                    break;
                }
            }

            // ? 
            if (exists)
            {
                state[change.Key] = change.Value;
            }
            // if it does not exist in the current state, add it
            else
            {
                state[change.Key] = change.Value;
            }
        }
        return state;
    }
}
