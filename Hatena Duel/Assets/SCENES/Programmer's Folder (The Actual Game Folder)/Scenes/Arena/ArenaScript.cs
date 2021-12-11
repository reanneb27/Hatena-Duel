using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaScript : MonoBehaviour
{
    public GameObject BackToMenuDialog;

    public GameObject PrinceHaul;
    public GameObject Arkayn;
    public GameObject VhoKiz;

    public GameObject HeavenArena;
    public GameObject VoidArena;

    // Character Profile Sprites
    public Sprite PrinceHaulSprite;
    public Sprite ArkaynSprite;
    public Sprite VhokizSprite;

    // PLAYER 1 UI FIELDS
    public Image P1CharacterProfileImgUI;
    public TextMeshProUGUI P1CharacterNameUI;
    public RectTransform P1HealthBarUI;
    public RectTransform P1RageBarUI;

    public TextMeshProUGUI P1Skill1TextUI;
    public RectTransform P1Skill1CDUI;
    public TextMeshProUGUI P1Skill2TextUI;
    public RectTransform P1Skill2CDUI;
    public TextMeshProUGUI P1Skill3TextUI;
    public RectTransform P1Skill3CDUI;
    public TextMeshProUGUI P1UltimateSkillTextUI;
    public RectTransform P1UltimateSkillCDUI;

    // player 2 skills UI
    public TextMeshProUGUI P2Skill1TextUI;
    public RectTransform P2Skill1CDUI;
    public TextMeshProUGUI P2Skill2TextUI;
    public RectTransform P2Skill2CDUI;
    public TextMeshProUGUI P2Skill3TextUI;
    public RectTransform P2Skill3CDUI;
    public TextMeshProUGUI P2UltimateSkillTextUI;
    public RectTransform P2UltimateSkillCDUI;

    private Vector3 Player1Pos = new Vector3(-7.31f, -3.9f, 98.29884f);
    private Vector3 Player2Pos = new Vector3(6.73f, -3.9f, 179.0211f);
    private GameObject Player1;
    private GameObject Player2;

    // player settings
    SettingsSerializer PlayerSettings;
    CharacterBase Player1Character;

    // Start is called before the first frame update
    void Start()
    {
        switch (CharacterSelectScript.SelectedCharacter)
        {
            case Character.PrinceHaul:
                Player1 = Instantiate(PrinceHaul, Player1Pos, Quaternion.identity);

                // set profile sprite
                P1CharacterProfileImgUI.sprite = PrinceHaulSprite;
                P1CharacterProfileImgUI.SetNativeSize();
                break;
            case Character.Arkayn:
                Player1 = Instantiate(Arkayn, Player1Pos, Quaternion.identity);

                // set profile sprite
                P1CharacterProfileImgUI.sprite = ArkaynSprite;
                P1CharacterProfileImgUI.SetNativeSize();
                break;
            case Character.VhoKiz:
                Player1 = Instantiate(VhoKiz, Player1Pos, Quaternion.identity);

                // set profile sprite
                P1CharacterProfileImgUI.sprite = VhokizSprite;
                P1CharacterProfileImgUI.SetNativeSize();
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

        // set player 1 defaults
        PlayerSettings = SettingsSerializer.CreateFromSettings();
        Player1Character = Player1.GetComponent<CharacterBase>();
        Player1Character.PlayerSettings = PlayerSettings;

        P1CharacterNameUI.text = Player1Character.characterName;
        P1Skill1TextUI.text = Player1Character.Skill1Name;
        P1Skill2TextUI.text = Player1Character.Skill2Name;
        P1Skill3TextUI.text = Player1Character.Skill3Name;
        P1UltimateSkillTextUI.text = Player1Character.UltimateSkillName;

        Player1.tag = "Player1";

        // set player 2 defaults
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

        // player 1 cooldown UIs
        P1Skill1CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player1Character.NextSkill1Time - Time.time, 0, Player1Character.Skill1Cooldown) / Player1Character.Skill1Cooldown, 1, 1);
        P1Skill2CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player1Character.NextSkill2Time - Time.time, 0, Player1Character.Skill2Cooldown) / Player1Character.Skill2Cooldown, 1, 1);
        P1Skill3CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player1Character.NextSkill3Time - Time.time, 0, Player1Character.Skill3Cooldown) / Player1Character.Skill3Cooldown, 1, 1);

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
