using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : MonoBehaviour
{
    // Start is called before the first frame update
    public string ItemName;
    public bool isPurchased = false;
    public bool isLocked = false; // Trạng thái khóa để ngăn chặn việc mua chung
    private void Awake()
    {
        ItemName = gameObject.name;
    }
}
