using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectOnKeyPress : MonoBehaviour
{
    public GameObject objectToToggle; // Vật thể cần bật/tắt
    public GameObject TB; // Vật thể TB cần bật/tắt dựa vào phạm vi
    public Transform player; // Vị trí nhân vật
    public float detectionRange = 0.1f; // Phạm vi phát hiện
    public KeyCode toggleKey = KeyCode.F; // Phím để bật/tắt

    private bool isPlayerInRange = false; // Kiểm tra xem nhân vật có trong phạm vi không

    void Update()
    {
        // Kiểm tra khoảng cách giữa nhân vật và vật thể
        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerInRange = distance <= detectionRange;

        // Quản lý hiển thị của TB
        if (TB != null)
        {
            TB.SetActive(isPlayerInRange);
        }

        // Hiển thị thông báo nếu nhân vật trong phạm vi
        if (isPlayerInRange)
        {
            Debug.Log("Press 'F' to toggle the object.");
        }

        // Kiểm tra nếu phím F được nhấn và nhân vật trong phạm vi
        if (isPlayerInRange && Input.GetKeyDown(toggleKey))
        {
            if (objectToToggle != null)
            {
                objectToToggle.SetActive(!objectToToggle.activeSelf);
            }
        }
    }
}
