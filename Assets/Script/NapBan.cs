using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapBan : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 originalPosition;
    public Vector3 openPosition; // Vị trí mở ra
    public float speed = 5f;
    private bool isMoving = false; // Cờ kiểm soát di chuyển
    bool isOpening;
    void Start()
    {
        originalPosition = transform.localPosition;
        openPosition = new Vector3(0.307f, 0.702f, 0.539f); // Đẩy nắp về phía trước
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (isOpening)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, openPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.localPosition, openPosition) < 0.01f) isMoving = false; // Dừng di chuyển khi đến nơi
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f) isMoving = false; // Dừng di chuyển khi về vị trí cũ
            }
        }
    }
    public void MoveTo()
    {
        isOpening = !isOpening; // Đảo trạng thái
        isMoving = true;
    }
}
