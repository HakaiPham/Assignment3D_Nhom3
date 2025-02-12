using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer2 : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent agent;//Ai di chuyển
    public List<ItemStore> shoppingList = new List<ItemStore>();//Danh sách cần mua
    private List<ShelvesStore> shelves = new List<ShelvesStore>();//Danh sách kệ hàng có trong cửa hàng(Nó chưa được khởi tạo danh sách)
    private int _CurrentItemIndex = 0;//Món hàng hiện tại
    private bool isShopping = false; // Đang mua hàng không
    StoreManager storeManager;
   public void StartShopping()//nên mới phải truyền tham số vào
   {
        isShopping = true;
        MoveToNextItem();
   }
    private void Start()
    {
        storeManager = FindFirstObjectByType<StoreManager>();
        shoppingList = storeManager.GetRandomItem();
        shelves = storeManager.FindAllShelfOnScence();
        StartShopping();
    }
    public void MoveToNextItem()
    {
        if(_CurrentItemIndex >= shelves.Count)
        {
            GoToCheckout();
            return;
        }
        //Bug không lấy được name vật phẩm
        if (shoppingList.Count > _CurrentItemIndex && shoppingList[_CurrentItemIndex] != null)
        {
            string itemName = shoppingList[_CurrentItemIndex].ItemName;
            ShelvesStore targetShelf = FindShelfWithItem(itemName);
            if (targetShelf != null)
            {
                Debug.Log("Khách hàng đi tới item " + itemName + " ở " + targetShelf.ShelfName);
                agent.SetDestination(targetShelf.Position);
            }
            else
            {
                Debug.Log("Khách hàng không tìm thấy item");
                _CurrentItemIndex++; //Bỏ qua món này và tìm món tiếp theo
                MoveToNextItem();
            }
        }
        else
        {
            Debug.Log("Lỗi: shoppingList rỗng hoặc phần tử đang null!");
        }
    }
    ShelvesStore FindShelfWithItem(string itemName)
    {
        if (shelves == null || shelves.Count == 0)
        {
            Debug.LogWarning("⚠ Không có kệ hàng nào được tìm thấy!");
            return null;
        }
        foreach (ShelvesStore shelf in shelves)
        {
            List<ItemStore> shelfItems = shelf.GetItems(); // Lấy danh sách item từ kệ
            Debug.Log("ItemCount: "+shelfItems.Count);

            if (shelfItems == null || shelfItems.Count == 0)
            {
                Debug.Log("🔍 Kệ " + shelf.ShelfName + " không có hàng.");
                continue;
            }

            Debug.Log("📦 Đang kiểm tra kệ: " + shelf.ShelfName + " - Số lượng hàng: " + shelfItems.Count);

            foreach (ItemStore item in shelfItems)
            {
                if (item == null) continue;

                Debug.Log("🛒 Kiểm tra item: " + item.ItemName + " - Stock: ");

                if (item.ItemName == itemName)
                {
                    Debug.Log("✅ Đã tìm thấy món hàng: " + item.ItemName + " trên kệ: " + shelf.ShelfName);
                    return shelf;
                }
            }
        }


        Debug.LogWarning("❌ Không tìm thấy item: " + itemName + " trong kho.");
        return null; // Không tìm thấy món hàng
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Shelf")
        {
           ShelvesStore shelves = collision.GetComponent<ShelvesStore>();
            if(shelves != null && shoppingList.Contains(shelves.items[_CurrentItemIndex]) ) 
            {
                TakeItem(shelves);
            }
        }
    }
    public void TakeItem(ShelvesStore shelves)
    {
        string itemName = shoppingList[_CurrentItemIndex].ItemName;
        foreach(var item in shelves.items)
        {
            if(item.ItemName == itemName)
            {
                Debug.Log("Đã lấy item "+item.ItemName + " từ "+shelves.ShelfName);
                _CurrentItemIndex++;//Tìm món tiếp theo
                MoveToNextItem();
                return;
            }
        }
    }
    void GoToCheckout()
    {
        isShopping = false;
        Debug.Log("🛍 Khách hàng đã mua xong, đi thanh toán!");
        // Gọi NavMeshAgent để di chuyển đến quầy thanh toán
    }
}
