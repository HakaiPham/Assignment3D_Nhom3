using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    public GameObject PhaSanMenu;
    private bool isPaused = false;

    public bool IsPhaSan = false;

    void Start()
    {
        pauseMenu.SetActive(false); // Đảm bảo menu tắt khi bắt đầu
        PhaSanMenu.SetActive(false);
        Cursor.visible = false; // Ẩn con trỏ chuột khi bắt đầu game
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ vào giữa màn hình
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Sử dụng GetKeyDown để tránh kích hoạt nhiều lần
        {
            TogglePauseMenu();
        }
        if(IsPhaSan == false){
            PhaSan();
        }
      
    }

    private void TogglePauseMenu()
    {
        isPaused = !isPaused; // Đảo trạng thái
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // Dừng game
            Cursor.visible = true; // Hiện con trỏ chuột khi menu bật
            Cursor.lockState = CursorLockMode.None; // Cho phép di chuyển chuột tự do
        }
        else
        {
            Time.timeScale = 1f; // Tiếp tục game
            Cursor.visible = false; // Ẩn con trỏ chuột khi tiếp tục chơi
            Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ vào giữa màn hình
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Đảm bảo game tiếp tục khi về menu chính
        Cursor.visible = true; // Hiện con trỏ chuột khi về menu chính
        Cursor.lockState = CursorLockMode.None; // Cho phép di chuyển chuột tự do
        PhaSanMenu.SetActive(false );
        SceneManager.LoadSceneAsync("MainMenu");

    }

    public void TiepTucButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false; // Ẩn con trỏ chuột khi tiếp tục chơi
    }

    public void PhaSan()
    {
        PlayerCurrentMoney playerMoney = FindObjectOfType<PlayerCurrentMoney>();

        if (playerMoney != null && playerMoney.GetMoney() == 0)
        {
            IsPhaSan = true;
            PhaSanMenu.SetActive(true); // Hiển thị menu thua cuộc (nếu cần)
            Time.timeScale = 0f; // Dừng game
            Cursor.visible = true; // Hiện con trỏ chuột khi menu bật
            Cursor.lockState = CursorLockMode.None; // Cho phép di chuyển chuột tự do
        }
    }
}
