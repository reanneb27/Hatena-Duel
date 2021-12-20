using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaScript : MonoBehaviour
{
    MenuAudioManagerScript MenuAudio;

    // Dialog fields
    public GameObject GameOverDialog;
    public TextMeshProUGUI WinnerTitleUI;
    public TextMeshProUGUI GameOverTextUI;
    public Image StoneImageUI;
    public TextMeshProUGUI StoneText1;
    public TextMeshProUGUI StoneText2;

    public GameObject BackToMenuDialog;

    public GameObject PrinceHaul;
    public GameObject Arkayn;
    public GameObject VhoKiz;

    public GameObject HeavenArena;
    public GameObject VoidArena;

    // Countdown fields
    public TextMeshProUGUI CountdownUI;
    int countdownSeconds = 180;
    int currentCountdownSeconds = 180;
    float countdownEndingTime;

    // Character Profile Sprites
    public Sprite PrinceHaulSprite;
    public Sprite ArkaynSprite;
    public Sprite VhokizSprite;

    // PLAYER 1 UI FIELDS
    public Image P1CharacterProfileImgUI;
    public TextMeshProUGUI P1CharacterNameUI;
    public RectTransform P1HealthBarUI;
    public RectTransform P1RageBarUI;

    public Image P1Skill1SpriteUI;
    public TextMeshProUGUI P1Skill1TextUI;
    public RectTransform P1Skill1CDUI;
    public Image P1Skill2SpriteUI;
    public TextMeshProUGUI P1Skill2TextUI;
    public RectTransform P1Skill2CDUI;
    public Image P1Skill3SpriteUI;
    public TextMeshProUGUI P1Skill3TextUI;
    public RectTransform P1Skill3CDUI;

    // player 2 skills UI
    public Image P2CharacterProfileImgUI;
    public TextMeshProUGUI P2CharacterNameUI;
    public RectTransform P2HealthBarUI;
    public RectTransform P2RageBarUI;

    public Image P2Skill1SpriteUI;
    public TextMeshProUGUI P2Skill1TextUI;
    public RectTransform P2Skill1CDUI;
    public Image P2Skill2SpriteUI;
    public TextMeshProUGUI P2Skill2TextUI;
    public RectTransform P2Skill2CDUI;
    public Image P2Skill3SpriteUI;
    public TextMeshProUGUI P2Skill3TextUI;
    public RectTransform P2Skill3CDUI;

    private Vector3 Player1Pos = new Vector3(-7.31f, -3.9f, 98.29884f);
    private Vector3 Player2Pos = new Vector3(6.73f, -3.9f, 179.0211f);
    private GameObject Player1;
    private GameObject Player2;

    SkillIconSprites SkillSprites;

    // player settings
    SettingsSerializer PlayerSettings;
    CharacterBase Player1Character;
    CharacterBase Player2Character;

    // Start is called before the first frame update
    void Awake()
    {
        MenuAudio = FindObjectOfType<MenuAudioManagerScript>();
        MenuAudio.Stop("main_menu_bg");

        countdownEndingTime = Time.time + countdownSeconds;

        SkillSprites = GetComponent<SkillIconSprites>();

        switch (CharacterSelectScript.SelectedCharacter)
        {
            case Character.PrinceHaul:
                Player1 = Instantiate(PrinceHaul, Player1Pos, Quaternion.identity);
                
                // set UI sprites
                P1CharacterProfileImgUI.sprite = PrinceHaulSprite;
                P1CharacterProfileImgUI.SetNativeSize();

                P1Skill1SpriteUI.sprite = SkillSprites.PrinceHaulSkill1Icon;
                P1Skill2SpriteUI.sprite = SkillSprites.PrinceHaulSkill2Icon;
                P1Skill3SpriteUI.sprite = SkillSprites.PrinceHaulSkill3Icon;
                break;
            case Character.Arkayn:
                Player1 = Instantiate(Arkayn, Player1Pos, Quaternion.identity);

                // set UI sprites
                P1CharacterProfileImgUI.sprite = ArkaynSprite;
                P1CharacterProfileImgUI.SetNativeSize();

                P1Skill1SpriteUI.sprite = SkillSprites.ArkaynSkill1Icon;
                P1Skill2SpriteUI.sprite = SkillSprites.ArkaynSkill2Icon;
                P1Skill3SpriteUI.sprite = SkillSprites.ArkaynSkill3Icon;
                break;
            case Character.VhoKiz:
                Player1 = Instantiate(VhoKiz, Player1Pos, Quaternion.identity);

                // set UI sprites
                P1CharacterProfileImgUI.sprite = VhokizSprite;
                P1CharacterProfileImgUI.SetNativeSize();

                P1Skill1SpriteUI.sprite = SkillSprites.VhokizSkill1Icon;
                P1Skill2SpriteUI.sprite = SkillSprites.VhokizSkill2Icon;
                P1Skill3SpriteUI.sprite = SkillSprites.VhokizSkill3Icon;
                break;
        }
        switch (CharacterSelectScript.SelectedArena)
        {
            case Arena.Heaven:
                HeavenArena.SetActive(true);
                Player2 = Instantiate(Arkayn, Player2Pos, Quaternion.identity);

                // set UI sprites
                P2CharacterProfileImgUI.sprite = ArkaynSprite;
                P2CharacterProfileImgUI.SetNativeSize();

                P2Skill1SpriteUI.sprite = SkillSprites.ArkaynSkill1Icon;
                P2Skill2SpriteUI.sprite = SkillSprites.ArkaynSkill2Icon;
                P2Skill3SpriteUI.sprite = SkillSprites.ArkaynSkill3Icon;
                break;
            case Arena.Void:
                VoidArena.SetActive(true);
                Player2 = Instantiate(VhoKiz, Player2Pos, Quaternion.identity);

                // set UI sprites
                P2CharacterProfileImgUI.sprite = VhokizSprite;
                P2CharacterProfileImgUI.SetNativeSize();

                P2Skill1SpriteUI.sprite = SkillSprites.VhokizSkill1Icon;
                P2Skill2SpriteUI.sprite = SkillSprites.VhokizSkill2Icon;
                P2Skill3SpriteUI.sprite = SkillSprites.VhokizSkill3Icon;
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

        Player1.tag = "Player1";
        Player1.layer = LayerMask.NameToLayer("Player1");

        // set player 2 defaults
        Player2Character = Player2.GetComponent<CharacterBase>();

        P2CharacterNameUI.text = Player2Character.characterName;
        P2Skill1TextUI.text = Player2Character.Skill1Name;
        P2Skill2TextUI.text = Player2Character.Skill2Name;
        P2Skill3TextUI.text = Player2Character.Skill3Name;

        Player2.tag = "Player2";
        Player2.layer = LayerMask.NameToLayer("Player2");
        Player2.GetComponent<SpriteRenderer>().flipX = true;

        // ignore each players' collision
        Physics2D.IgnoreCollision(Player1.GetComponent<Collider2D>(), Player2.GetComponent<Collider2D>());

        // set player 2 as AI
        Player2Character.IsAI = true;

        // play player 2 theme bg song
        MenuAudio.Play(CharacterSelectScript.SelectedArena == Arena.Heaven ? "arkayn_theme" : "vhokiz_theme");
    }

    bool gameOver = false;
    bool player1Won = true;
    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                BackToMenuDialog.SetActive(true);
            }

            // ------------------- A N Y  U I  U P D A T E ------------------- //
            currentCountdownSeconds = (int)(countdownEndingTime - Time.time);
            int minutes = currentCountdownSeconds / 60;
            int secs = currentCountdownSeconds - minutes * 60;
            string countdownStringFormat = minutes.ToString("00") + ":" + secs.ToString("00");
            CountdownUI.text = countdownStringFormat;
            // --------------------------------------------------------------- //


            // ------------------- P L A Y E R  1  U I  U P D A T E -----------------------
            // -------- cooldown UIs
            P1Skill1CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player1Character.NextSkill1Time - Time.time, 0, Player1Character.Skill1Cooldown) / Player1Character.Skill1Cooldown, 1, 1);
            P1Skill2CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player1Character.NextSkill2Time - Time.time, 0, Player1Character.Skill2Cooldown) / Player1Character.Skill2Cooldown, 1, 1);
            P1Skill3CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player1Character.NextSkill3Time - Time.time, 0, Player1Character.Skill3Cooldown) / Player1Character.Skill3Cooldown, 1, 1);

            // -------- health and rage UIs
            P1HealthBarUI.localScale = new Vector3(Player1Character.health / Player1Character.maxHealth, 1, 1);
            P1RageBarUI.localScale = new Vector3(Player1Character.rage / Player1Character.maxRage, 1, 1);


            // ------------------- P L A Y E R  2  U I  U P D A T E -----------------------
            // -------- cooldown UIs
            P2Skill1CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player2Character.NextSkill1Time - Time.time, 0, Player2Character.Skill1Cooldown) / Player2Character.Skill1Cooldown, 1, 1);
            P2Skill2CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player2Character.NextSkill2Time - Time.time, 0, Player2Character.Skill2Cooldown) / Player2Character.Skill2Cooldown, 1, 1);
            P2Skill3CDUI.localScale = new Vector3(1 - Mathf.Clamp(Player2Character.NextSkill3Time - Time.time, 0, Player2Character.Skill3Cooldown) / Player2Character.Skill3Cooldown, 1, 1);

            // -------- health and rage UIs
            P2HealthBarUI.localScale = new Vector3(Player2Character.health / Player2Character.maxHealth, 1, 1);
            P2RageBarUI.localScale = new Vector3(Player2Character.rage / Player2Character.maxRage, 1, 1);


            // check if GAME OVER
            if (Player1Character is ArkaynScript && Player1Character.health == 0 && Player1Character.UltimateSkillIsRunning)
                return;
            if (Player2Character is ArkaynScript && Player2Character.health == 0 && Player2Character.UltimateSkillIsRunning)
                return;
            if (currentCountdownSeconds == 0 || Player1Character.health == 0 || Player2Character.health == 0)
            {
                gameOver = true;

                Player1Character.isDead = true;
                Player2Character.isDead = true;

                Player1Character.ChangeAnimationState(CharacterBase.CharacterState.IDLE);
                Player2Character.ChangeAnimationState(CharacterBase.CharacterState.IDLE);

                // check which player won
                if (currentCountdownSeconds == 0)
                {
                    if (Player1Character.health / Player1Character.maxHealth < Player2Character.health / Player2Character.maxHealth)
                        player1Won = false;
                }
                else
                {
                    if (Player1Character.health == 0)
                    {
                        player1Won = false;
                        Player1Character.ChangeAnimationState(CharacterBase.CharacterState.DEATH);
                    }
                    else
                        Player2Character.ChangeAnimationState(CharacterBase.CharacterState.DEATH);
                }

                // set gameover UI text and images first, then set it to active
                WinnerTitleUI.text = player1Won ? "You won!" : "You lost!";
                GameOverTextUI.text = player1Won ? "You defeated " : "You lost to " + Player2Character.characterName;
                StoneIconsScript stones = GetComponent<StoneIconsScript>();

                if (player1Won)
                {
                    if (CharacterSelectScript.SelectedArena == Arena.Heaven)
                    {
                        StoneImageUI.sprite = stones.EmeraldStone;
                        StoneText1.text = "You obtained emerald stone!";
                        StoneText2.text = "You can now play as Arkayn!";

                        if (Save.HeavenStageCompleted)
                        {
                            StoneImageUI.gameObject.SetActive(false);
                            StoneText1.text = "You have already obtained the emerald stone";
                            StoneText2.text = "";
                        }
                    }
                    else
                    {
                        StoneImageUI.sprite = stones.RubyStone;
                        StoneText1.text = "You obtained ruby stone!";
                        StoneText2.text = "You can now play as Vho'kiz!";

                        if (Save.VoidStageCompleted)
                        {
                            StoneImageUI.gameObject.SetActive(false);
                            StoneText1.text = "You have already obtained the ruby stone";
                            StoneText2.text = "";
                        }
                    }
                }
                else
                {
                    StoneImageUI.gameObject.SetActive(false);
                    StoneText1.text = "Better luck next time!";
                    StoneText2.text = "";
                }

                StartCoroutine(StartGameOverUIDelayed(3.5f));
            }
        }
    }

    IEnumerator StartGameOverUIDelayed(float secs)
    {
        float delay = Time.time + secs;

        while (Time.time < delay)
            yield return null;

        Time.timeScale = 0;
        GameOverDialog.SetActive(true);

        yield return null;
    }

    #region GAME OVER UI METHODS
    public void GameOverContinue()
    {
        // update save
        SaveSerializer newSave = SaveSerializer.CreateFromSettings();
        newSave.HeavenStageCompleted |= player1Won && CharacterSelectScript.SelectedArena == Arena.Heaven;
        newSave.VoidStageCompleted |= player1Won && CharacterSelectScript.SelectedArena == Arena.Void;
        SaveManager.UpdateSave(newSave);

        MenuAudio.Stop(CharacterSelectScript.SelectedArena == Arena.Heaven ? "arkayn_theme" : "vhokiz_theme");

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
    #endregion

    #region BACK TO MENU METHODS
    public void BackToMenuYes()
    {
        MenuAudio.Stop(CharacterSelectScript.SelectedArena == Arena.Heaven ? "arkayn_theme" : "vhokiz_theme");

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
