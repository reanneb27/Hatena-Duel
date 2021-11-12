using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaScript : MonoBehaviour
{
    public GameObject BackToMenuDialog;

    public GameObject PrinceHaul;
    public GameObject Arkayn;
    public GameObject VhoKiz;

    public GameObject HeavenArena;
    public GameObject VoidArena;

    public GameObject Player1UI;
    public GameObject Player2UI;

    private Vector3 Player1Pos = new Vector3(-7.31f, -3.9f, 98.29884f);
    private Vector3 Player2Pos = new Vector3(6.73f, -3.9f, 179.0211f);
    private GameObject Player1;
    private GameObject Player2;

    // Start is called before the first frame update
    void Start()
    {
        switch (CharacterSelectScript.SelectedCharacter)
        {
            case Character.PrinceHaul:
                Player1 = Instantiate(PrinceHaul, Player1Pos, Quaternion.identity);
                break;
            case Character.Arkayn:
                Player1 = Instantiate(Arkayn, Player1Pos, Quaternion.identity);
                break;
            case Character.VhoKiz:
                Player1 = Instantiate(VhoKiz, Player1Pos, Quaternion.identity);
                break;
        }
        switch (CharacterSelectScript.SelectedArena)
        {
            case Arena.Heaven:
                HeavenArena.SetActive(true);
                Player2 = Instantiate(Arkayn, Player2Pos, Quaternion.identity);
                Player2.GetComponent<ArkaynScript>().enabled = false;
                break;
            case Arena.Void:
                VoidArena.SetActive(true);
                Player2 = Instantiate(VhoKiz, Player2Pos, Quaternion.identity);
                Player2.GetComponent<VhoKizScript>().enabled = false;
                break;
        }

        // set players defaults
        Player1.tag = "Player1";
        Player2.tag = "Player2";

        Player2.GetComponent<SpriteRenderer>().flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            BackToMenuDialog.SetActive(true);
        }
    }

    #region BACK TO MENU METHODS
    public void BackToMenuYes()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
    public void BackToMenuNo()
    {
        BackToMenuDialog.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion
}
