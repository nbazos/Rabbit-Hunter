using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_FSMState
{
    // the method the delegate is referencing
    void Update(FSM fsm, GameObject gameObject);
}
