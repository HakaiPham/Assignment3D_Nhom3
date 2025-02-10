using TMPro;
using UnityEngine;

public class TriggerPanel : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactionDistance = 3f; // Max distance for interaction
    public LayerMask interactableLayer; // Layer for interactable objects
    public KeyCode interactionKey = KeyCode.E; // Key to trigger panel

    [Header("UI & Feedback")]
    private bool isLookingAtPanel = false; // Check if player is looking at the panel
    public GameObject InteractPromt;

    [Header("Panel Stuff")]
    public GameObject ShopPanel;
    

    

    void Update()
    {
        CheckForPanelInteraction();
    }

    void CheckForPanelInteraction()
    {
        InteractPromt.SetActive(false);
        // Raycast from camera center
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // If we hit a trigger panel
            if (hit.collider.CompareTag("PC"))
            {
                isLookingAtPanel = true;
                if (InteractPromt != null) InteractPromt.SetActive(true);

                // If player presses interaction key
                if (Input.GetKeyDown(interactionKey))
                {
                    ActivatePanel(hit.collider.gameObject);
                }
            }
        }
        else
        {
            // Player is not looking at a panel
            if (!isLookingAtPanel)
            {
                InteractPromt.SetActive(false);
            }
        }
    }

    void ActivatePanel(GameObject panel)
    {
        Debug.Log("Panel Activated: " + panel.name);
        // You can add functionality here, such as opening a UI, playing a sound, etc.
    }
}
