using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            //Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget != null && newTarget != this.transform) // Ensure the target is not itself
        {
            target = newTarget;
            //Debug.Log("Target set to " + target.name);
            StartCoroutine(UpdatePath());
        }
    }

    IEnumerator UpdatePath()
    {
        while (target != null)
        {
            agent.SetDestination(target.position);
            //Debug.Log("Moving towards target: " + target.name + " at position " + target.position);
            yield return new WaitForSeconds(0.5f); // Adjust update frequency as needed
        }
    }
}
