using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs; // Array to hold different NPC models
    public Transform[] spawnPoints; // Array of spawn locations
    public int maxNPCs = 8; // Maximum NPCs allowed
    private List<GameObject> activeNPCs = new List<GameObject>();

    void Start()
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0)
        {
            Debug.LogError("NPC Prefabs array is not set or empty.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn Points array is not set or empty.");
            return;
        }

        InvokeRepeating("SpawnNPC", 1f, 3f); // Spawn NPCs at intervals
    }

    void SpawnNPC()
    {
        if (activeNPCs.Count >= maxNPCs) return; // Don't spawn if limit reached

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        GameObject newNPC = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        activeNPCs.Add(newNPC);

        NPCMovement npcMovement = newNPC.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            npcMovement.OnNPCDestroyed += () => OnNPCDestroyed(newNPC);
        }
        else
        {
            Debug.LogError("NPC prefab does not have an NPCMovement component.");
        }
    }

    private void OnNPCDestroyed(GameObject npc)
    {
        activeNPCs.Remove(npc);
        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            npcMovement.OnNPCDestroyed -= () => OnNPCDestroyed(npc);
        }
    }
}
