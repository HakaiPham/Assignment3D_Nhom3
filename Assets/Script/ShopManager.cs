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
    public float money = 1000f; // Player's money
    public TextMeshProUGUI moneyText;
    public Transform spawnPoint; // Where the item will spawn

    void Start()
    {
        moneyText.text = "Money: " + money.ToString("F2");

        for (int i = 1; i <= 43; i++)
        {
            shopItems.Add(new ShopItem
            {
                id = i,
                name = "Item " + i,
                price = Random.Range(10, 500), // Random price between 10 and 500
                prefab = Resources.Load<GameObject>("Item" + i) // Load prefab from Resources folder
            });
        }
    }

    public void BuyItem(int itemId)
    {
        ShopItem item = shopItems.Find(x => x.id == itemId);

        if (item != null)
        {
            if (money >= item.price)
            {
                money -= item.price;
                moneyText.text = "Money: " + money.ToString("F2");
                SpawnItem(item.prefab);
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

    void SpawnItem(GameObject itemPrefab)
    {
        if (itemPrefab != null && spawnPoint != null)
        {
            Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Missing Item Prefab or Spawn Point!");
        }
    }
}
