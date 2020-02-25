using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planner
{
    // Return a queue of planned actions for the desired goal
    public Queue<Action> Plan(GameObject agent, List<Action> possibleActions, Dictionary<string, object> stateOfWorld, Dictionary<string, object> goal)
    {
        // Reset each possible actions variables for accurate planning 
        foreach (Action a in possibleActions)
        {
            a.ResetVariables();
        }

        List<Action> functionalActions = new List<Action>();
        
        // Check the procedural preconditions of the possible actions and add actions that can be performed 
        foreach(Action a in possibleActions)
        {
            if (a.CheckProceduralPrecondition(agent)) 
            {
                functionalActions.Add(a);
            }
        }

        List<LeafNode> leaves = new List<LeafNode>();

        LeafNode startingNode = new LeafNode(null, 0, stateOfWorld, null);
        
        // Start processing different action plans to reach the goal  
        bool success = BuildSolutionTree(startingNode, leaves, functionalActions, goal);

        // If no plan is found 
        if (!success)
        {
            return null;
        }

        // Get the cheapest LeafNode
        LeafNode cheapest = null;
        leaves.Sort((A, B) => A.costSoFar.CompareTo(B.costSoFar));
        cheapest = leaves[0];

        // Correctly order a list with sequential actions, working nodes through their parents
        List<Action> orderedResult = new List<Action>();
        LeafNode node = cheapest;
        while (node != null)
        {
            if (node.action != null)
            {
                orderedResult.Insert(0, node.action);
            }
            // Move to parent LeafNode
            node = node.parentNode;
        }
        
        // Transfer the ordered list to a queue
        Queue<Action> plannedActions = new Queue<Action>();
        foreach (Action a in orderedResult)
        {
            plannedActions.Enqueue(a);
        }

        // Return the queue of planned actions to reach the goal
        return plannedActions;
    }

    // Returns true if a solution is found and fills the List<LeafNode> branch routes
    private bool BuildSolutionTree(LeafNode parent, List<LeafNode> leaves, List<Action> functionalActions, Dictionary<string, object> goal)
    {
        bool solutionFound = false;

        // Iterate through actions from this LeafNode and see if we can use it here
        foreach (Action action in functionalActions)
        {

            // Check if the action's preconditions match the parent's state
            if (CheckState(action.actionPreconditions, parent.state))
            {

                // Add on the action's postconditions or effects
                Dictionary<string, object> currentState = ApplyChangesToCurrentState(parent.state, action.actionEffects);
                LeafNode LeafNode = new LeafNode(parent, parent.costSoFar + action.cost, currentState, action);

                // Check for state equivalence with the goal state
                if (CheckState(goal, currentState))
                {
                    leaves.Add(LeafNode);
                    solutionFound = true;
                }
                // Continue the tree to see if remaining actions can reach the goal
                else
                {
                    List<Action> remainingActions = RemainingActions(functionalActions, action);
                    bool found = BuildSolutionTree(LeafNode, leaves, remainingActions, goal);
                    if (found)
                    {
                        solutionFound = true;
                    }
                }
            }
        }

        return solutionFound;
    }

    // Return a list of remaining actions except for the invalid one passed in
    private List<Action> RemainingActions(List<Action> actions, Action invalidAction)
    {
        List<Action> remainingActions = new List<Action>();
        foreach (Action a in actions)
        {
            if (!a.Equals(invalidAction))
            {
                remainingActions.Add(a);
            }
        }
        return remainingActions;
    }

    // Check state equivalence
    private bool CheckState(Dictionary<string, object> testState, Dictionary<string, object> state)
    {
        bool equivalence = true;

        foreach (KeyValuePair<string, object> t in testState)
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
                equivalence = false;
            }
        }

        return equivalence;
    }

    // Apply changes to the current state and return the updated state
    private Dictionary<string, object> ApplyChangesToCurrentState(Dictionary<string, object> currentState, Dictionary<string, object> changedState)
    {
        Dictionary<string, object> updatedState = new Dictionary<string, object>();
        
        // Transfer the the contents of current state 
        foreach (KeyValuePair<string, object> current in currentState)
        {
            updatedState.Add(current.Key, current.Value);
        }

        // Update the key-value pair according to the changes
        foreach (KeyValuePair<string, object> change in changedState)
        {
            updatedState[change.Key] = change.Value;
        }

        return updatedState;
    }
}
