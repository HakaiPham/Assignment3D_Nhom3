using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonInfo : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    private ShopManager shopManager;

    void Start()
    {
        // Find and assign the ShopManager in the scene
        shopManager = FindObjectOfType<ShopManager>();

        if (shopManager == null)
        {
            Debug.LogError("ShopManager not found in the scene!");
            return;
        }

        UpdateItemInfo();
    }

    void UpdateItemInfo()
    {
        if (shopManager != null)
        {
            ShopManager.ShopItem item = shopManager.shopItems.Find(x => x.id == itemID);

            if (item != null)
            {
                nameText.text = item.name;
                priceText.text = "Price: $" + item.price;
            }
            else
            {
                Debug.LogError("Item with ID " + itemID + " not found in ShopManager!");
            }
        }
    }

    public void BuyItem()
    {
        if (shopManager != null)
        {
            shopManager.BuyItem(itemID);
            UpdateItemInfo(); // Refresh UI after buying
        }
    }
}
