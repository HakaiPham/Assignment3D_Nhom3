using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{


    [SerializeField] public GameObject mainMenu;

    void Start()
    {
    }
    public void NewGameButton()
    {
      // GameData.isNewGame = true;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync("ScenceGame");
    }
    public void TiepTucButton()
    {
        SceneManager.LoadSceneAsync("ScenceGame");
    }

    public void ThoatGame()
    {
        Application.Quit();
    }
    //public void OpenMainMenu()
    //{
    //    mainMenu.SetActive(true);
    //}
    //public void OffMainMenu()
    //{
    //    mainMenu.SetActive(false);
    //}
}
