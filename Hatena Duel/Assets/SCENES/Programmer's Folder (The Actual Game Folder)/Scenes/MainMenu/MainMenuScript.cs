using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject ExitConfirmDialog;


    public void NewGame()
    {
        
    }
    public void Continue()
    {

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
