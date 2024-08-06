using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;
    private float stoppingDistance = 1.5f; // Distance at which the enemy stops

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget != null && newTarget != this.transform) // Ensure the target is not itself
        {
            target = newTarget;
            StartCoroutine(UpdatePath());
        }
    }

    IEnumerator UpdatePath()
    {
        while (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= stoppingDistance)
            {
                agent.isStopped = true;
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attack"); // Set attacking animation
                // Add your attack logic here
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                animator.SetBool("isWalking", true);
            }

            yield return new WaitForSeconds(0.5f); // Adjust update frequency as needed
        }

        animator.SetBool("isWalking", false);
    }

    public void Die()
    {

        animator.SetTrigger("Dead");
        Destroy(gameObject, 2f); // Adjust delay as needed
    }
}
