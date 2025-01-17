using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerControll : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] GameObject[] _ListItem;
    [SerializeField] private List<GameObject> _ItemNeedShopping;
    private Vector3 originalPosition;
    [SerializeField] private int _CurrentTargetIndex = 0;//Theo dõi mục tiêu hiện tại
    bool _IsNextItem = false;
    public bool _IsFinishedShopping = false;
    [SerializeField] public Transform _CheckOutCounter;
    int itemCount = 0;
    CheckOutCounter checkOutManager;
    public bool _HasCompletedCheckout = false;
    [SerializeField] float distance;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        Debug.Log("Original Position: " + originalPosition);
        ShoppingItem();
        checkOutManager = FindObjectOfType<CheckOutCounter>();
    }

    // Update is called once per frame
    //Bug 2 đối tượng cùng mua 1 món đồ gây ra va chạm không thỏa mãn điều kiện
    //Bug sau khi TMDK trở về vị trí ban đầu thì lại chạy về phía quầy thanh toán
    void Update()
    {
        if (_CurrentTargetIndex >= _ItemNeedShopping.Count&&!_IsFinishedShopping)
        {
            MoveToCheckOut();

        }
        // Nếu đã mua xong và chưa hoàn thành việc trở về vị trí ban đầu
        if (_HasCompletedCheckout)
        {
            // Kiểm tra nếu agent đã về vị trí ban đầu
            float distance = Vector3.Distance(transform.position, originalPosition);
            if (distance < 1f)
            {
                Debug.Log("Đã về vị trí ban đầu.");
                _IsFinishedShopping = true;

                // Xóa danh sách vật phẩm và cập nhật trạng thái
                _ItemNeedShopping.Clear();
                _IsNextItem = false;

                checkOutManager.CheckOutCompleted();
            }
        }
        if (_ItemNeedShopping.Count > 0 && agent.remainingDistance < 0.05f)
        {
            //agent.remainingDistance là khoảng cách từ vị trí hiện tại đến đích còn cách khoản bao nhiêu
            if(_CurrentTargetIndex < _ItemNeedShopping.Count)
            {
                _CurrentTargetIndex++;
                if (!_IsNextItem)//Xác định mục tiêu hiện tại không vượt quá số lượng vật phẩm cần mua
                {
                    StartCoroutine(WaitBuy());
                }
            }
        }
    }
    public void Move()
    {
        agent.SetDestination(_ItemNeedShopping[_CurrentTargetIndex].gameObject.transform.position);
    }
    public void ShoppingItem()
    {
        //Xác định số lượng vật phẩm cần mua
        int itemCount = Random.Range(1,6);
        //Xác định các loại vật phẩm cần mua
        for (int i = 0; i < itemCount; i++)
        {
            //Xác định loại vật phẩm cần mua
            int randomItem = Random.Range(0,_ListItem.Length);
            //Thêm các vật phẩm đã mua vào list 
            _ItemNeedShopping.Add(_ListItem[randomItem]);
        }
        StartCoroutine(WaitBuy());//Bắt đầu di chuyển đến mục tiêu đầu tiên
    }
    public void GetCheckOutPostition(Vector3 checkoutPosition)
    {
        agent.SetDestination(checkoutPosition);

    }
    public void MoveToCheckOut()
    {
        bool checkOutIsBusy = checkOutManager.CheckOutIsBusy();
        if (!_HasCompletedCheckout)
        {
            Debug.Log("Đang đợi thanh toán");
            Vector3 checkOutPoint = checkOutManager.CheckoutPoint().transform.position;
            distance = Vector3.Distance(transform.position, checkOutPoint);
            if (!checkOutIsBusy)
            {
                checkOutManager.AddCustomerToQueue(this.gameObject);
            }
            // Nếu Player đã đến quầy thanh toán
            if (distance < 0.5f)
            {
                // Hiển thị các vật phẩm đã mua
                foreach (GameObject item in _ItemNeedShopping)
                {
                    if (itemCount == _ItemNeedShopping.Count) break;
                    itemCount++;
                    Instantiate(item, checkOutPoint, Quaternion.identity);
                }

                // Nếu người chơi nhấn H, xác nhận thanh toán hoàn tất
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Debug.Log("Đã hoàn tất thanh toán");
                    _HasCompletedCheckout = true; // Cờ xác nhận thanh toán
                    agent.SetDestination(originalPosition); // Di chuyển về vị trí ban đầu
                }
            }
        }
    }
    IEnumerator WaitBuy()//Đợi mua rồi di chuyển
    {
        _IsNextItem = true;
        Move();
        yield return new WaitForSeconds(1f);
        _IsNextItem = false;
    }
}
