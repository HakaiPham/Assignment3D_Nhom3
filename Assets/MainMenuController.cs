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
        // Load saved sound settings
        LoadSoundSettings();
    }

    #region Main Menu
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("ScenceGame");
    }

    public void OnContinueButtonClicked()
    {
        SceneManager.LoadScene("ScenceGame");
    }

    public void OnQuitButtonClicked()
    {
        // Save sound settings before quitting
        SaveSoundSettings();
        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    #endregion

    #region Options Menu
    public void OnBackButtonClicked()
    {
        // Save settings when exiting options
        SaveSoundSettings();
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnBackgroundMusicVolumeChanged()
    {
        float bgMusicVolume = backgroundMusicSlider.value;
        backgroundMusic.volume = bgMusicVolume;
        PlayerPrefs.SetFloat("BackgroundMusicVolume", bgMusicVolume);
    }

    public void OnSoundEffectsVolumeChanged()
    {
        float sfxVolume = soundEffectsSlider.value;
        soundEffectsAudioSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", sfxVolume);
    }
    #endregion

    #region Sound Settings Persistence
    private void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("BackgroundMusicVolume", backgroundMusicSlider.value);
        PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsSlider.value);
        PlayerPrefs.Save(); // Save changes immediately
    }

    private void LoadSoundSettings()
    {
        float bgMusicVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1f);

        backgroundMusicSlider.value = bgMusicVolume;
        soundEffectsSlider.value = sfxVolume;
        backgroundMusic.volume = bgMusicVolume;
        soundEffectsAudioSource.volume = sfxVolume;
    }
    #endregion
}
