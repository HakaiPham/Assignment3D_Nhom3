using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] npcPrefabs;  // Array of different NPC prefabs
    public Transform spawnPoint;  // Where NPCs will be spawned
    public Transform endpoint; // Endpoint NPCs should reach
    public Transform[] waypoints; // Path for the NPCs to follow
    public Vector2 daySpawnRange = new Vector2(1f, 3f); // Random spawn interval during the day
    public Vector2 eveningSpawnRange = new Vector2(5f, 10f); // Random spawn interval during the evening
    public int maxNPCs = 10; // Maximum number of active NPCs

    [Header("Time Control Integration")]
    public TimeController timeController; // Reference to TimeController

    private List<GameObject> activeNPCs = new List<GameObject>();

    void Start()
    {
        if (timeController == null)
        {
            timeController = FindObjectOfType<TimeController>();
        }
        StartCoroutine(SpawnNPCs());
    }

    void Update()
    {
        // Destroy all NPCs immediately after 9 PM
        if (timeController != null)
        {
            DateTime currentTime = timeController.GetCurrentTime();
            if (currentTime.Hour >= 21 || currentTime.Hour < 6)
            {
                DestroyAllNPCs();
            }
        }
    }

    IEnumerator SpawnNPCs()
    {
        while (true)
        {
            if (timeController != null && activeNPCs.Count < maxNPCs)
            {
                DateTime currentTime = timeController.GetCurrentTime();
                float hour = currentTime.Hour + currentTime.Minute / 60f;
                if ((hour >= 6f && hour < 21f) && timeController.CheckCanSpawnCustomer())
                {
                    SpawnRandomNPC();
                }
            }
            float interval = GetRandomSpawnInterval();
            if (interval != -1f)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return null; // Prevent freezing
            }
        }
    }

    void SpawnRandomNPC()
    {
        if (npcPrefabs.Length == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, npcPrefabs.Length);
        GameObject npc = Instantiate(npcPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            List<Transform> fullPath = new List<Transform>(waypoints);
            if (endpoint != null)
            {
                fullPath.Add(endpoint);
            }
            npcMovement.waypoints = fullPath.ToArray();
            npcMovement.speed = AdjustSpeedBasedOnTime();
            npcMovement.OnNPCDestroyed += () => RemoveNPC(npc);
        }
        activeNPCs.Add(npc);
    }

    float GetRandomSpawnInterval()
    {
        if (timeController != null)
        {
            DateTime currentTime = timeController.GetCurrentTime();
            float hour = currentTime.Hour + currentTime.Minute / 60f;

            if (hour >= 6f && hour < 18f) // Daytime: 6 AM - 6 PM
            {
                return UnityEngine.Random.Range(daySpawnRange.x, daySpawnRange.y);
            }
            else if (hour >= 18f && hour < 21f) // Evening: 6 PM - 9 PM
            {
                return UnityEngine.Random.Range(eveningSpawnRange.x, eveningSpawnRange.y);
            }
        }
        return -1f; // No spawning from 9 PM - 6 AM
    }

    float AdjustSpeedBasedOnTime()
    {
        if (timeController != null)
        {
            DateTime currentTime = timeController.GetCurrentTime();
            float hour = currentTime.Hour + currentTime.Minute / 60f;

            if (hour >= 6f && hour < 18f) // Daytime
            {
                return 5f; // Speed during the day
            }
            else if (hour >= 18f && hour < 21f) // Evening
            {
                return 7f; // Speed during the evening
            }
        }
        return 0f; // No speed at night as they won't spawn
    }

    void DestroyAllNPCs()
    {
        foreach (GameObject npc in activeNPCs)
        {
            if (npc != null)
            {
                Destroy(npc);
            }
        }
        activeNPCs.Clear();
    }

    void RemoveNPC(GameObject npc)
    {
        if (activeNPCs.Contains(npc))
        {
            activeNPCs.Remove(npc);
        }
    }
}