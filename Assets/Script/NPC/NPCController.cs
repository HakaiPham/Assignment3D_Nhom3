using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] npcPrefabs; // NPC Prefabs to spawn
    [SerializeField] private Transform[] spawnPoints; // Points where NPCs will spawn
    [SerializeField] private Transform endPoint; // End point where NPCs are destroyed

    private List<GameObject> activeNPCs = new List<GameObject>(); // List to keep track of spawned NPCs

    void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        while (true)
        {
            // Spawn a new NPC
            SpawnNPC();

            // Check if NPCs have reached the end point and destroy them
            CheckNPCsAtEndPoint();

            // Wait for a random interval between 2 to 10 seconds before spawning the next NPC
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 7.5f));
        }
    }

    void SpawnNPC()
    {
        // Pick a random spawn point and NPC prefab
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject npcPrefab = npcPrefabs[UnityEngine.Random.Range(0, npcPrefabs.Length)];

        // Instantiate NPC at the spawn point
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        activeNPCs.Add(npc);

        // Add the end point to the NPC's movement script
        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            npcMovement.endPoint = endPoint; // Set the endpoint for the NPC
            npcMovement.OnNPCDestroyed += () => RemoveNPCFromList(npc); // Remove from list when destroyed
        }
    }

    void CheckNPCsAtEndPoint()
    {
        // Loop through each active NPC and check if it's reached the endpoint
        foreach (GameObject npc in activeNPCs)
        {
            NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
            if (npcMovement != null && npcMovement.HasReachedEndPoint(endPoint.position))
            {
                Destroy(npc); // Destroy NPC
                activeNPCs.Remove(npc); // Remove from the active list
            }
        }
    }

    // Method to remove NPC from the list when it is destroyed
    void RemoveNPCFromList(GameObject npc)
    {
        activeNPCs.Remove(npc);
    }
}
