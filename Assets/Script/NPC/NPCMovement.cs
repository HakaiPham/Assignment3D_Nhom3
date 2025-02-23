using System;
using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 3f;
    public float detectionDistance = 2f;
    public float rotationSpeed = 5f;
    public LayerMask obstacleMask;
    public event Action OnNPCDestroyed;
    public Transform[] waypoints;

    private Animator animator;
    private bool isAvoidingObstacle = false;
    private int currentWaypointIndex = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        if (isAvoidingObstacle) return;

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance, obstacleMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                StartCoroutine(AvoidObstacle());
            }
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            animator.SetBool("IsWalk", true);

            // Increased threshold for reaching waypoints
            if (Vector3.Distance(transform.position, targetPosition) < 1.5f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    OnNPCDestroyed?.Invoke();
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator AvoidObstacle()
    {
        isAvoidingObstacle = true;
        animator.SetBool("IsWalk", false);

        float randomTurn = UnityEngine.Random.Range(-90f, 90f);
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomTurn, 0);
        float time = 0;

        while (time < 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        isAvoidingObstacle = false;
        animator.SetBool("IsWalk", true);
    }
}