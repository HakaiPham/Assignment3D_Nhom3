using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainMenuPanel; // Main menu panel
    public GameObject optionsPanel;  // Options menu panel

    [Header("Sound Settings")]
    public Slider backgroundMusicSlider; // Slider for background music volume
    public Slider soundEffectsSlider;    // Slider for sound effects volume
    public AudioSource backgroundMusic;  // Background music AudioSource
    public AudioSource soundEffectsAudioSource; // Sound effects AudioSource

    void Start()
    {
        // Load the saved sound settings
        float bgMusicVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 1f); // Default is 1 (max)
        float sfxVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f); // Default is 1 (max)

        // Apply the saved volume settings
        backgroundMusicSlider.value = bgMusicVolume;
        soundEffectsSlider.value = sfxVolume;
        backgroundMusic.volume = bgMusicVolume;
        soundEffectsAudioSource.volume = sfxVolume;
    }

    #region Main Menu
    public void OnStartButtonClicked()
    {
        // Load the game scene (assuming it's called "GameScene")
        SceneManager.LoadScene("GameScene");
    }

    public void OnContinueButtonClicked()
    {
        // Load the saved game or continue from where the player left off
        // You can implement your own logic to load saved data here
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButtonClicked()
    {
        // Quit the game
        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        // Show the options menu and hide the main menu
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    #endregion

    #region Options Menu
    public void OnBackButtonClicked()
    {
        // Return to the main menu
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnBackgroundMusicVolumeChanged()
    {
        // Update the background music volume and save it
        float bgMusicVolume = backgroundMusicSlider.value;
        backgroundMusic.volume = bgMusicVolume;
        PlayerPrefs.SetFloat("BackgroundMusicVolume", bgMusicVolume);
        PlayerPrefs.Save();
    }

    public void OnSoundEffectsVolumeChanged()
    {
        // Update the sound effects volume and save it
        float sfxVolume = soundEffectsSlider.value;
        soundEffectsAudioSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    #endregion
}
