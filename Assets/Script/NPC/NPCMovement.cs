using System;
using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 3f;
    public float detectionDistance = 2f;
    public float rotationSpeed = 5f;
    public LayerMask obstacleMask;

    public event Action OnNPCDestroyed; // Event for when NPC is destroyed
    public Transform endPoint; // Endpoint for NPC to reach
    private Animator animator;
    private bool isAvoidingObstacle = false;

    void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        MoveForward();

        // Check if the NPC has reached the endpoint
        if (HasReachedEndPoint(endPoint.position))
        {
            Destroy(gameObject); // Destroy the NPC
            OnNPCDestroyed?.Invoke(); // Trigger NPC destroyed event
        }
    }

    void MoveForward()
    {
        if (isAvoidingObstacle) return; // Avoid stopping movement during rotation

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance, obstacleMask))
        {
            StartCoroutine(AvoidObstacle());
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            animator.SetBool("IsWalk", true); // Set walking animation
        }
    }

    public bool HasReachedEndPoint(Vector3 endPointPosition)
    {
        return Vector3.Distance(transform.position, endPointPosition) < 1f; // NPC has reached the endpoint
    }

    IEnumerator AvoidObstacle()
    {
        isAvoidingObstacle = true;
        animator.SetBool("IsWalk", false); // Stop walking animation while avoiding

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
        animator.SetBool("IsWalk", true); // Resume walking animation
    }
}
