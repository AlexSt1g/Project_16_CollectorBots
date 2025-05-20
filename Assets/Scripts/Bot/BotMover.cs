using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotMover : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }    

    public void MoveTo(Vector3 targetPosition)
    {        
        _agent.destination = targetPosition;
    }
}
