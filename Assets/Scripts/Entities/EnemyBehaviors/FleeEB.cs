using SLS.StateMachineV3;
using System;
using UnityEngine;
using UnityEngine.AI;

public class FleeEB : StateBehavior
{
    [SerializeField] float speed;
    [SerializeField] float destUpdateRate = 2f;
    [SerializeField] float reachDistance;
    [SerializeField] State reachState;

    private NavMeshAgent agent;
    private TrackerEB playerTracker;
    private Timer_Old destUpdateTimer;

    public override void OnAwake()
    {
        agent = GetComponentFromMachine<NavMeshAgent>();
        playerTracker = state.parent.GetComponent<TrackerEB>();
        destUpdateTimer = new(destUpdateRate, UpdateDestination);
    }

    public override void OnEnter(State prev, bool isFinal)
    {
        agent.enabled = true;
        agent.speed = speed;
        UpdateDestination();
    }

    public override void OnFixedUpdate()
    {
        if (playerTracker.Distance(false) >= reachDistance)
        {
            playerTracker.PhaseTransition(reachState);
            agent.enabled = false;

            return;
        }

        destUpdateTimer += Time.fixedDeltaTime;
    }

    void UpdateDestination()
    {
        Vector3 playerPosition = playerTracker.target.transform.position;
        Vector3 currentPosition = agent.transform.position;
        Vector3 fleePoint = currentPosition + (currentPosition - playerPosition);
        agent.SetDestination(fleePoint);
    } 

}
