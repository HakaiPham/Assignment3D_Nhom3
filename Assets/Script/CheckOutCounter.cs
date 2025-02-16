using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckOutCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform checkOutPoint;
    private Queue<GameObject> customerQueue = new Queue<GameObject>();//hàng đợi khách hàng
    public bool isCheckoutBusy = false;//Trạng thái quầy thanh toán
    private int lastQueueCount = -1; // Biến lưu số lượng hàng đợi trước đó
    public float queueSpacing = 1.5f;// Khoảng cách của các khách hàng trong hàng đợi
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Tạo 1 biến xác định có người nào đang thanh toán không?
        //Nếu có thì người kia sẽ phải đợi cho đến khi được thanh toán xong
        //Biến sẽ thành true khi mà người chơi hoàn thanh xong việc thanh toán
        //Sau khi hoàn thành xong việc biến sẽ trở lại thành false
    }
    public void AddCustomerToQueue(GameObject customer)
    {
        if (!customerQueue.Contains(customer)) // Đảm bảo khách hàng không bị thêm trùng lặp
        {
            customerQueue.Enqueue(customer);
            UpdateQueuePosition();
        }
    }
    public void UpdateQueuePosition()
    {
        int index = 0;//Xử lý khách hàng đang có trong hàng đợi
        foreach (GameObject customer in customerQueue)
        {
            Vector3 targetPosition = checkOutPoint.position - new Vector3 (0, 0, index *queueSpacing);
            customer.GetComponent<CustomerControll>().MoveTo(targetPosition);
            index++;
        }
    }
    public void ProcessQueue()
    {
        if(customerQueue.Count > 0)
        {
            isCheckoutBusy = true;
            GameObject currentCustomer = customerQueue.Dequeue();//Lấy khách hàng đầu tiên'
            currentCustomer.GetComponent<CustomerControll>().GetCheckOutPostition(checkOutPoint.position);
            UpdateQueuePosition();
        }
    }
    public void CheckOutCompleted()
    {
        isCheckoutBusy = false; // Quầy thanh toán trống
        if(customerQueue.Count > 0)
        {
            ProcessQueue(); // Xử lý khách hàng tiếp theo

        }
    }
    public Transform CheckoutPoint()
    {
        return checkOutPoint;
    }
    public bool CheckOutIsBusy()
    {
        return isCheckoutBusy;
    }
}
