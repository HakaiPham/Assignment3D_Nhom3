using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] npcPrefabs; // NPC Prefabs to spawn
    [SerializeField] private Transform[] spawnPoints; // Spawn locations
    [SerializeField] private Transform[] waypoints; // Path waypoints
    [SerializeField] private Transform endPoint; // Final destination
    [SerializeField] private TimeController timeController; // Reference to the TimeController

    private List<GameObject> activeNPCs = new List<GameObject>(); // Track active NPCs

    void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        while (true)
        {
            DateTime currentTime = timeController.GetCurrentTime();
            TimeSpan sunrise = TimeSpan.FromHours(timeController.sunriseHour);
            TimeSpan sunset = TimeSpan.FromHours(timeController.sunsetHour);

            float spawnInterval;

            // Faster spawn rate during the day, slower at night
            if (currentTime.TimeOfDay >= sunrise && currentTime.TimeOfDay <= sunset)
            {
                spawnInterval = UnityEngine.Random.Range(1f, 5f); // Daytime: Faster spawn
            }
            else
            {
                spawnInterval = UnityEngine.Random.Range(5f, 10f); // Nighttime: Slower spawn
            }

            SpawnNPC();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnNPC()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject npcPrefab = npcPrefabs[UnityEngine.Random.Range(0, npcPrefabs.Length)];

        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        activeNPCs.Add(npc);

        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            List<Transform> fullPath = new List<Transform>(waypoints);
            fullPath.Add(endPoint); // Add endpoint to the waypoint list
            npcMovement.waypoints = fullPath.ToArray();
            npcMovement.OnNPCDestroyed += () => RemoveNPCFromList(npc);
        }
    }

    void RemoveNPCFromList(GameObject npc)
    {
        activeNPCs.Remove(npc);
    }
}