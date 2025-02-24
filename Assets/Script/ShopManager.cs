using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public int id;
        public string name;
        public int price;
        public GameObject prefab; // Prefab to spawn
    }

    public List<ShopItem> shopItems = new List<ShopItem>();
    public TextMeshProUGUI moneyText;
    public Transform spawnPoint; // Where the box will spawn
    public GameObject boxPrefab; // The container box prefab

    private PlayerCurrentMoney playerMoney; // Reference to PlayerCurrentMoney

    private void Awake()
    {
        playerMoney = FindObjectOfType<PlayerCurrentMoney>();
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].id = i;
            shopItems[i].name = shopItems[i].prefab.gameObject.name;
        }
    }
    void Start()
    {
        // Find PlayerCurrentMoney in the scene

        if (playerMoney == null)
        {
            Debug.LogError("PlayerCurrentMoney script not found in the scene!");
            return;
        }

        UpdateMoneyUI(); // Set initial UI money value

        //for (int i = 0; i <= shopItems.Count; i++)
        //{
        //    shopItems[i].id = i;
        //    shopItems[i].name = shopItems[i].prefab.gameObject.name;
        //}
    }

    public void BuyItem(int itemId)
    {
        ShopItem item = shopItems.Find(x => x.id == itemId);

        if (item != null)
        {
            int currentMoney = playerMoney.UpdateCurrentMoneY();

            if (currentMoney >= item.price)
            {
                playerMoney.TruTien(-item.price); // Deduct money
                FindObjectOfType<DailyTransactionTracker>().AddExpense(item.price);
                UpdateMoneyUI(); // Update UI
                SpawnItemBox(item);
                Debug.Log($"Bought {item.name} for {item.price}!");
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
        else
        {
            Debug.LogError("Item not found!");
        }
    }

    void SpawnItemBox(ShopItem item)
    {
        if (boxPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Box prefab or spawn point missing!");
            return;
        }

        // Create a box at the spawn point
        GameObject box = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);
        box.name = "Item Box - " + item.name;

        // Attach the RefillBox script to manage item storage
        RefillBox refillBox = box.GetComponent<RefillBox>();
        if (refillBox == null)
        {
            Debug.LogError("RefillBox script missing on the box prefab!");
            return;
        }

        // Set item details in RefillBox
        refillBox.itemID = item.id;
        refillBox.item = item.prefab;
        refillBox.amount = Random.Range(3, 5); // Random quantity between 3-4
        refillBox.name = "Box of " + item.name;

        Debug.Log($"Spawned a box containing {refillBox.amount} of {item.name}.");
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + playerMoney.UpdateCurrentMoneY() + "$";
        }
    }
}
