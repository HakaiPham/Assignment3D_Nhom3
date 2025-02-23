using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject shopPanel; // The shop panel
    private bool isShopOpen = false; // Track shop state

    void Start()
    {
        // Find the shop panel in the scene by name
        shopPanel = GameObject.Find("Shop UI");

        if (shopPanel == null)
        {
            Debug.LogError("Shop UI panel not found! Make sure it's named 'Shop UI' in the hierarchy.");
            return;
        }

        shopPanel.SetActive(false); // Hide shop at the start
        LockCursor(); // Ensure cursor is locked at start
    }

    void Update()
    {
        if (shopPanel == null) return;

        if (Input.GetKeyDown(KeyCode.B)) // Press "B" to toggle shop
        {
            ToggleShop();
        }

        // Press "Esc" to close the shop (optional)
        if (Input.GetKeyDown(KeyCode.Escape) && isShopOpen)
        {
            CloseShop();
        }
    }

    void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.SetActive(isShopOpen);

        if (isShopOpen)
            UnlockCursor();
        else
            LockCursor();
    }

    void CloseShop()
    {
        isShopOpen = false;
        shopPanel.SetActive(false);
        LockCursor();
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Show cursor
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
    }
}
