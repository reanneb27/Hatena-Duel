using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    MenuAudioManagerScript MenuAudio;

    public GameObject ExitConfirmDialog;
    public GameObject OverwriteSaveFileDialog;
    public Button ContinueButton;
    private bool SaveFileExists { get => File.Exists(SaveManager.SaveFilePath); }

    public void Start()
    {
        MenuAudio = FindObjectOfType<MenuAudioManagerScript>();
        MenuAudio.Play("main_menu_bg");

        ContinueButton.interactable = SaveFileExists;
    }

    #region NEW GAME METHODS
    public void NewGame()
    {
        if (SaveFileExists)
            OverwriteSaveFileDialog.SetActive(true);
        else
            NewGameYes();
    }
    public void NewGameYes()
    {
        CreateNewSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void NewGameNo()
    {
        OverwriteSaveFileDialog.SetActive(false);
    }
    private void CreateNewSave()
    {
        SaveSerializer newSave = new SaveSerializer();
        SaveManager.UpdateSave(newSave);
        Debug.Log(SaveManager.SaveFilePath);
    }
    #endregion
    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    #region EXIT METHODS
    public void ExitGame()
    {
        ExitConfirmDialog.SetActive(true);
    }
    public void ExitYes()
    {
        Application.Quit();
        Debug.Log("Game exit");
    }
    public void ExitNo()
    {
        ExitConfirmDialog.SetActive(false);
    }
    #endregion
}
