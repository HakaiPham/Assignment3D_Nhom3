using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerControll : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    [SerializeField] List<GameObject> _ListItem;
    [SerializeField]
    GameObject[] _GetItemOnScene;
    [SerializeField] private List<GameObject> _ItemNeedShopping;
    private Vector3 originalPosition;
    [SerializeField] private int _CurrentTargetIndex = 0;//Theo dõi mục tiêu hiện tại
    bool _IsNextItem = false;
    public bool _IsFinishedShopping = false;
    int itemCount = 0;
    CheckOutCounter checkOutManager;
    public bool _HasCompletedCheckout = false;
    [SerializeField] float distance;
    Animator animator;
    [SerializeField] GameObject[] _Shelf; // Danh sách kệ hàng
    public bool hasPurchased;
    int randomShelf; //biến dùng để Random kệ hàng
    bool isOnlyStart = false; //cờ này chỉ chạy 1 lần duy nhất
    [SerializeField]
    List<GameObject> listItemIsCheckOut = new List<GameObject>();//Đây là list chứa các vật phẩm được sinh ra khi khách hàng thanh toán
    //
    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        _GetItemOnScene = GameObject.FindGameObjectsWithTag("ItemStore");
        _Shelf = GameObject.FindGameObjectsWithTag("Shelf");
        foreach (var item in _GetItemOnScene)
        {
            _ListItem.Add(item);
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        ShoppingItem();
        checkOutManager = FindObjectOfType<CheckOutCounter>();
        randomShelf = Random.Range(0, _Shelf.Length);
    }

   //Khách hàng sẽ phải đi vào xem vật phẩm còn hàng không 
    void Update()
    {
        if (_CurrentTargetIndex == _ItemNeedShopping.Count && !_IsFinishedShopping)
        {
            if (!hasPurchased&&!isOnlyStart)
            {
                MoveTo(_Shelf[randomShelf].transform.position);
                isOnlyStart = true;
                return;
            }
            else if(hasPurchased)
            {
                MoveToCheckOut();

            }
        }
        // Nếu đã mua xong và chưa hoàn thành việc trở về vị trí ban đầu
        if (_HasCompletedCheckout)
        {
            // Kiểm tra nếu agent đã về vị trí ban đầu
            float distance = Vector3.Distance(transform.position, originalPosition);
            if (distance < 1f)
            {
                _IsFinishedShopping = true;
                animator.SetBool("IsWalk", false);
                // Xóa danh sách vật phẩm và cập nhật trạng thái
                _ItemNeedShopping.Clear();
                _IsNextItem = false;
            }
        }
        //bug khi nhân vật mua cùng lúc 2 món hàng cùng loại sẽ xảy ra bug
        if (_ItemNeedShopping.Count > 0 && agent.remainingDistance < 0.1f)
        {
            //agent.remainingDistance là khoảng cách từ vị trí hiện tại đến đích còn cách khoản bao nhiêu
            if (_CurrentTargetIndex < _ItemNeedShopping.Count)
            {
                agent.speed = 0;
                if (!_IsNextItem)//Xác định mục tiêu hiện tại không vượt quá số lượng vật phẩm cần mua
                {
                    _CurrentTargetIndex++;
                    animator.SetTrigger("IsShopping");
                    StartCoroutine(WaitBuy());
                    _ItemNeedShopping[_CurrentTargetIndex - 1].SetActive(false);
                }
            }
        }
    }
    //Nếu xuất hiện 1 vật phẩm tới tận 2 lần cùng tên thì khi đó khách hàng chỉ có thể mua một lần
    //Lần 2 nếu gặp lại đúng vật phẩm đó (cùng tên) thì sẽ bõ qua vật phẩm đó
    //Nếu không còn vật phẩm nào khác thì player sẽ tới quầy thanh toán
    //Nghĩa là nếu vật phẩm đó tồn tại trong List thì sẽ không được thêm vào nữ
    public void Move()
    {
        animator.SetBool("IsWalk", true);
        agent.SetDestination(_ItemNeedShopping[_CurrentTargetIndex].gameObject.transform.position);
    }
    public void MoveTo(Vector3 position) // Đây là hàm giúp duy trì 1 một khoảng cách nhất định ở quầy thanh toán khi đang đợi xếp hàng
    {
        animator.SetBool("IsWalk", true);
        agent.SetDestination(position);
        StartCoroutine(CheckArrival());
    }
    private IEnumerator CheckArrival()
    {
        //đợi cho đến khi quảng đường còn lại bé hơn 0.5 và không còn tính toán đường đi nữa
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f && !agent.pathPending);
        animator.SetBool("IsWalk", false); // Chỉ tắt animation khi thực sự đến nơi
        yield return new WaitForSeconds(2f);
        if (!hasPurchased) // điều kiện chạy khi mà object không có mua món hàng nào hoặc món hàng đã được đặt trước
            //và danh sách mua không còn bất kì vật nào thì chạy về vị trí ban đầu
        {
            animator.SetBool("IsWalk", true);
            agent.SetDestination(originalPosition);
            yield return new WaitUntil(() => agent.remainingDistance < 0.5f && !agent.pathPending);
            animator.SetBool("IsWalk", false); // Chỉ tắt animation khi thực sự đến nơi
        }
    }
    public void ShoppingItem()
    {
        if (_ListItem.Count == 0) return;
       //Xác định số lượng vật phẩm cần mua
        // Danh sách các món đồ đã mua trong lần này
        List<GameObject> purchasedItems = new List<GameObject>();
        int countItem = Random.Range(0,3);
        //Xác định các loại vật phẩm cần mua
        hasPurchased = false;//Đánh dấu đã mua
        foreach (GameObject item in _ListItem)
        {
            ItemStore shoppingItem = item.GetComponent<ItemStore>();

            // Kiểm tra nếu món đồ chưa được mua và không bị khóa
            if (!shoppingItem.isPurchased && !shoppingItem.isLocked)
            {
                shoppingItem.isLocked = true;  // Đánh dấu là đang được mua
                shoppingItem.isPurchased = true;  // Đánh dấu là đã mua
                purchasedItems.Add(item);
                hasPurchased = true;
                if(purchasedItems.Count == countItem)
                {
                    break;//Nếu đủ số lượng thì thoát ra khỏi vòng lặp
                }
            }
        }
        if (hasPurchased)
        {
            _ItemNeedShopping.AddRange(purchasedItems);
            StartCoroutine(WaitBuy());//Bắt đầu di chuyển đến mục tiêu đầu tiên
        }

    }
    public void GetCheckOutPostition(Vector3 checkoutPosition)
    {
        animator.SetBool("IsWalk", true);
        agent.SetDestination(checkoutPosition);

    }
    //Làm cách nào để khách hàng khi đến vật phẩm tiếp theo nếu nó không có hàng trên kệ
    // thì bỏ qua thì đi tới món hàng tiếp theo còn không thì đi thanh toán
    public void MoveToCheckOut()
    {
        //bool checkOutIsBusy = checkOutManager.CheckOutIsBusy();
        if (!_HasCompletedCheckout)
        {
            Vector3 checkOutPoint = checkOutManager.CheckoutPoint().transform.position;
            distance = Vector3.Distance(checkOutPoint, transform.position);
            checkOutManager.AddCustomerToQueue(this.gameObject);
            //if (!checkOutIsBusy)
            //{
            //    checkOutManager.AddCustomerToQueue(this.gameObject);
            //    _IsComingCheckOut = true;//đang đi tới quầy 
            //}
            //if (!_IsComingCheckOut)//Không có tời quầy thanh toán
            //{
            //    animator.SetBool("IsWalk", false);
            //}
            // Nếu Player đã đến quầy thanh toán
            if (distance < 0.5f)
            {
                animator.SetBool("IsWalk", false);
                // Hiển thị các vật phẩm đã mua
                // Chỉ tạo vật phẩm nếu danh sách rỗng (tránh tạo trùng lặp)
                foreach (GameObject item in _ItemNeedShopping)
                {
                    if (itemCount == _ItemNeedShopping.Count) break;
                    itemCount++;
                    GameObject items = Instantiate(item, transform.position, Quaternion.identity);
                    items.transform.SetParent(gameObject.transform);
                    items.SetActive(true);
                    listItemIsCheckOut.Add(items);
                }
                // Nếu người chơi nhấn H, xác nhận thanh toán hoàn tất
                if (Input.GetKeyDown(KeyCode.H))
                {
                    checkOutManager.CheckOutCompleted();
                    foreach (GameObject item in listItemIsCheckOut)
                    {
                        Destroy(item);
                    }
                    // Khi kết thúc 1 ngày thì nó sẽ tự động xóa đi tất cả các item đã bị ẩn đi (mua đi)
                    _ListItem.Clear();
                    _GetItemOnScene = new GameObject[0]; // Reset mảng 
                    listItemIsCheckOut.Clear();
                    animator.SetBool("IsWalk", true);
                    _HasCompletedCheckout = true; // Cờ xác nhận thanh toán
                    agent.SetDestination(originalPosition); // Di chuyển về vị trí ban đầu
                }
            }
        }
    }
    IEnumerator WaitBuy()//Đợi mua rồi di chuyển
    {
        if (_CurrentTargetIndex >= 0 && _CurrentTargetIndex < _ItemNeedShopping.Count)
        {
            Move();
        }
        _IsNextItem = true;
        yield return new WaitForSeconds(2f);
        _IsNextItem = false;
        agent.speed = 3.5f;
    }
}
