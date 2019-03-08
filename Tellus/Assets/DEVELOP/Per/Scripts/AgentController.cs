using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private float tempLifeTime = 30; // REPLACE WITH DAMAGE MECHANIC

    private OverseerCameraController cameraController;
    private Camera cam;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform targetPosition;

    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private int amountOfAgentsToSpawn = 1;
    [SerializeField] int spawnInterval = 5;

    bool on = false;


    private LineRenderer lr;

    private void Start()
    {
        cameraController = FindObjectOfType<OverseerCameraController>();
        cam = cameraController.GetComponentInChildren<Camera>();
        targetPosition.position = transform.position;


        lr = GetComponent<LineRenderer>();
 
        lr.SetPosition(0, startPosition.position + Vector3.up);
        lr.SetPosition(1, targetPosition.position + Vector3.up);

    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            targetPosition.position = cameraController.CameraRaycastPosition();
            lr.SetPosition(1, targetPosition.position + Vector3.up);
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            //targetPosition.position = cameraController.CameraRaycastPosition();

            AIAgent[] agents = FindObjectsOfType<AIAgent>();
            foreach(AIAgent a in agents)
            {
                a.destination = targetPosition.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (on)
            {
                StopAllCoroutines();
                on = !on;
            }
            else
            {
                StartCoroutine(SpawnWave(amountOfAgentsToSpawn));
                on = !on;

            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            amountOfAgentsToSpawn -= 50;
            if (amountOfAgentsToSpawn <= 0)
                amountOfAgentsToSpawn = 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
            amountOfAgentsToSpawn += 50;
    }

    IEnumerator SpawnWave(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            var go = Instantiate(prefabToSpawn);
            go.transform.position = startPosition.position;
            go.transform.SetParent(this.transform);
            go.SetActive(true);
            var aiController = go.GetComponent<AIAgent>();

            StartCoroutine(aiController.SetTargetDestination(targetPosition));

            Destroy(go, tempLifeTime); // REPLACE WITH GAMEPLAY

            yield return new WaitForSeconds(0.01f);

        }

        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnWave(amountOfAgentsToSpawn));
    }

    private Vector3 TargetPosition(Transform target)
    {
        var finalPosition = new Vector3(target.position.x, target.position.y, target.position.z);
        return finalPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetPosition.position, 20f);
    }
}
