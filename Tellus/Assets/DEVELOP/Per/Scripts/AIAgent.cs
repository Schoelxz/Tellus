using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    // TODO : Delegate generic attributes (health etc) into more generic system (interfaces, inheritance)
    /* GAMEPLAY PARAMS */ 
    private int startingHealth = 100;
    private int currentHealth;


    /* NAVIGATION PARAMS */
    public float pathUpdateFrequency = 2f;
    public NavMeshAgent agent;

    private NavMeshPath path;
    private float elapsedTime;
    public Vector3 destination;

    private float accuracy = 5f;

    /* PERFORMANCE PARAMS */
    private int frameInterval = 100;

    // Using OnEnable instead of Awake for object pooling
    // TODO : Implement more generic object pool
    private void OnEnable()
    {
        currentHealth = startingHealth;
        agent = this.GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        elapsedTime = pathUpdateFrequency;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > pathUpdateFrequency)
        {
            elapsedTime -= pathUpdateFrequency;
            UpdateNavigationPath();
        }

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            var startColor = new Color(1, 0, 0, 0.5f);
            var blueGreen = Color.Lerp(new Color(0, 0, 1, 0.5f), Color.green, i % 2);

            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.Lerp(startColor, blueGreen, i));

        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // TODO : Insert pooling system here
        Destroy(this.gameObject);
    }

    private void UpdateNavigationPath()
    {
        NavMesh.CalculatePath(transform.position, destination, agent.areaMask, path);
        agent.SetPath(path);
    }

    public IEnumerator SetTargetDestination(Transform goalPosition)
    {
        destination = goalPosition.position;
        while (Vector3.Distance(transform.position, goalPosition.position) > accuracy)
        {
            yield return null;
        }
        agent.Warp(destination);
    }

}
