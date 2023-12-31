using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Targeter targeter = null;
    
    private Camera mainCamera;

    #region Server

    [ServerCallback]
    private void Update() 
    {
        if(agent.hasPath) return;
        if(agent.remainingDistance > agent.stoppingDistance) return;

        agent.ResetPath();    
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        targeter.ClearTarget();
        
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) {return; }

        agent.SetDestination(hit.position);

    }
    #endregion

    #region Client
    
    #endregion


}
