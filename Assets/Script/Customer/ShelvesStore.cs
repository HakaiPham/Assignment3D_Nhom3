using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShelvesStore : MonoBehaviour
{
    // Start is called before the first frame update
    public string ShelfName;
    public Vector3 Position;
    public List<ItemStore> items = new List<ItemStore>();
    void Awake()
    {
        Position = transform.position;
        ShelfName = GetComponent<ShelvesStore>().gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ItemStore"))
        {
            ItemStore item = collision.gameObject.GetComponent<ItemStore>();
            if (item != null)
            {
                items.Add(item);
                //Debug.Log("Đã thêm item vào kệ: " + item.ItemName + " - Stock: " + item.Stock);
                //Debug.Log("tổng item hiện tại là: " + items.Count);

            }
            else
            {
                Debug.LogWarning("Không tìm thấy ItemStore trên " + collision.gameObject.name);
            }
        }
    }
    public List<ItemStore> GetItems()
    {
        return items;
    }

}
