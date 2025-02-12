using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class StoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<ItemStore> availableItem  = new List<ItemStore>();//Danh sách Item trong cửa hàng (rỗng)
    private List<ShelvesStore> availableShelf = new List<ShelvesStore>();// Danh sách Shelf trong cửa hàng(rỗng)
    // Danh sách sản phầm hiện rỗng
    private void Awake()
    {
        FindAllItemOnScence();

    }

    // Update is called once per frame

    public List<ShelvesStore> FindAllShelfOnScence()
    {
        ShelvesStore[] listShelf = FindObjectsOfType<ShelvesStore>();
        foreach (ShelvesStore shelf in listShelf)
        {
            availableShelf.Add(shelf);
        }
        return availableShelf;
    }
    public List<ItemStore> FindAllItemOnScence()
    {
        ItemStore[] listItem = FindObjectsOfType<ItemStore>();
        //Tìm tất cả các object sản phẩm
        foreach (ItemStore itemStore in listItem)
        {
            availableItem.Add(itemStore);
        }
        Debug.Log("Tổng Item trong danh sách là: "+availableItem.Count);
        return availableItem;
    }
    public List<ItemStore> GetRandomItem()
    {
        List<ItemStore> shoppingList = new List<ItemStore>();
        if(availableItem.Count == 0) 
        {
            Debug.Log("không có sản phẩm nào trong cửa hàng");
            return shoppingList;
        }
        for (int i = 0; i < 4; i++) //Số item có thể mua
        {
            int randomIndex = Random.Range(0, availableItem.Count);//Loại item
            shoppingList.Add(availableItem[randomIndex]);
        }
        return shoppingList;
    }
}
