using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Character
{
    PrinceHaul, VhoKiz, Arkayn
}

public enum Arena
{
    Heaven, Void, TechNoir
}

public class CharacterSelectScript : MonoBehaviour
{
    public Animator animController;
    public GameObject SelectedArenaIndicator;

    // Button fields
    public Button ArkaynButton;
    public GameObject ArkaynXImage;
    public Button VhokizButton;
    public GameObject VhokizXImage;

    public TextMeshProUGUI CharacterNameTMP;
    public TextMeshProUGUI CharacterDescTMP;

    // character skills
    public TextMeshProUGUI Skill1NameTMP;
    public TextMeshProUGUI Skill1DescTMP;
    public Image Skill1SpriteUI;

    public TextMeshProUGUI Skill2NameTMP;
    public TextMeshProUGUI Skill2DescTMP;
    public Image Skill2SpriteUI;

    public TextMeshProUGUI Skill3NameTMP;
    public TextMeshProUGUI Skill3DescTMP;
    public Image Skill3SpriteUI;

    public TextMeshProUGUI UltimateNameTMP;
    public TextMeshProUGUI UltimateDescTMP;
    public Image UltimateSpriteUI;

    // default character/arena selection
    public static Character SelectedCharacter = Character.PrinceHaul;
    public static Arena SelectedArena = Arena.Heaven;

    SkillIconSprites SkillSprites;
    // Start is called before the first frame update
    void Start()
    {
        SkillSprites = GetComponent<SkillIconSprites>();
        SelectedCharacter = Character.PrinceHaul;
        SelectedArena = Arena.Heaven;

        // default selection
        SelectKingHaul();

        // character select Save-based values
        ArkaynButton.enabled = Save.HeavenStageCompleted;
        ArkaynXImage.SetActive(!Save.HeavenStageCompleted);

        VhokizButton.enabled = Save.VoidStageCompleted;
        VhokizXImage.SetActive(!Save.VoidStageCompleted);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #region CHARACTER SELECTION METHODS
    public void SelectKingHaul()
    {
        animController.Play("princeHaul_idle_anim");
        SelectedCharacter = Character.PrinceHaul;

        // set skill icons
        Skill1SpriteUI.sprite = SkillSprites.PrinceHaulSkill1Icon;
        Skill2SpriteUI.sprite = SkillSprites.PrinceHaulSkill2Icon;
        Skill3SpriteUI.sprite = SkillSprites.PrinceHaulSkill3Icon;
        UltimateSpriteUI.sprite = SkillSprites.PrinceHaulUltimateIcon;

        SetCharacterPreviewTexts("Prince Haul", "The son of King Fulchard");

        // set skill info
        SetSkill1Text(PrinceHaulScript.Skill1, PrinceHaulScript.Skill1Desc);
        SetSkill2Text(PrinceHaulScript.Skill2, PrinceHaulScript.Skill2Desc);
        SetSkill3Text(PrinceHaulScript.Skill3, PrinceHaulScript.Skill3Desc);
        SetUltimateText(PrinceHaulScript.UltimateSkill, PrinceHaulScript.UltimateSkillDesc);
    }
    public void SelectArkayn()
    {
        animController.Play("arkayn_idle_anim");
        SelectedCharacter = Character.Arkayn;

        // set skill icons
        Skill1SpriteUI.sprite = SkillSprites.ArkaynSkill1Icon;
        Skill2SpriteUI.sprite = SkillSprites.ArkaynSkill2Icon;
        Skill3SpriteUI.sprite = SkillSprites.ArkaynSkill3Icon;
        UltimateSpriteUI.sprite = SkillSprites.ArkaynUltimateIcon;

        SetCharacterPreviewTexts("Arkayn", "Leads the Heaven world. He guards the sapphire stone");

        // set skill info
        SetSkill1Text(ArkaynScript.Skill1, ArkaynScript.Skill1Desc);
        SetSkill2Text(ArkaynScript.Skill2, ArkaynScript.Skill2Desc);
        SetSkill3Text(ArkaynScript.Skill3, ArkaynScript.Skill3Desc);
        SetUltimateText(ArkaynScript.UltimateSkill, ArkaynScript.UltimateSkillDesc);
    }
    public void SelectVhoKiz()
    {
        animController.Play("vho'kiz_idle_anim");
        SelectedCharacter = Character.VhoKiz;

        // set skill icons
        Skill1SpriteUI.sprite = SkillSprites.VhokizSkill1Icon;
        Skill2SpriteUI.sprite = SkillSprites.VhokizSkill2Icon;
        Skill3SpriteUI.sprite = SkillSprites.VhokizSkill3Icon;
        UltimateSpriteUI.sprite = SkillSprites.VhokizUltimateIcon;

        SetCharacterPreviewTexts("Vho’kiz", "Leads the Void world. He guards the ruby stone");

        // set skill info
        SetSkill1Text(VhoKizScript.Skill1, VhoKizScript.Skill1Desc);
        SetSkill2Text(VhoKizScript.Skill2, VhoKizScript.Skill2Desc);
        SetSkill3Text(VhoKizScript.Skill3, VhoKizScript.Skill3Desc);
        SetUltimateText(VhoKizScript.UltimateSkill, VhoKizScript.UltimateSkillDesc);
    }
    #endregion

    #region ARENA SELECTION METHODS
    public void SelectHeaven()
    {
        SetSelectedArenaIndicatorAnchoredXPosition(-239);
        SelectedArenaIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Heaven";
        SelectedArena = Arena.Heaven;
    }
    public void SelectVoid()
    {
        SetSelectedArenaIndicatorAnchoredXPosition(0);
        SelectedArenaIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Void";
        SelectedArena = Arena.Void;
    }
    public void SelectTechNoir()
    {
        SetSelectedArenaIndicatorAnchoredXPosition(236);
        SelectedArenaIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tech Noir";
        SelectedArena = Arena.TechNoir;
    }
    #endregion

    #region HELPER METHODS
    /// <summary>
    /// Change position of the SelectedArenaIndicator below the selected arena.
    /// </summary>
    private void SetSelectedArenaIndicatorAnchoredXPosition(float x)
    {
        Vector3 prevPos = SelectedArenaIndicator.GetComponent<RectTransform>().anchoredPosition;
        prevPos.x = x;
        SelectedArenaIndicator.GetComponent<RectTransform>().anchoredPosition = prevPos;
    }

    /// <summary>
    /// Sets the character preview text UIs
    /// </summary>
    /// <param name="charName"></param>
    /// <param name="charDesc"></param>
    private void SetCharacterPreviewTexts(string charName, string charDesc)
    {
        CharacterNameTMP.text = charName;
        CharacterDescTMP.text = charDesc;
    }
    private void SetSkill1Text(string skillName, string skillDesc)
    {
        Skill1NameTMP.text = skillName;
        Skill1DescTMP.text = skillDesc;
    }
    private void SetSkill2Text(string skillName, string skillDesc)
    {
        Skill2NameTMP.text = skillName;
        Skill2DescTMP.text = skillDesc;
    }
    private void SetSkill3Text(string skillName, string skillDesc)
    {
        Skill3NameTMP.text = skillName;
        Skill3DescTMP.text = skillDesc;
    }
    private void SetUltimateText(string skillName, string skillDesc)
    {
        UltimateNameTMP.text = skillName;
        UltimateDescTMP.text = skillDesc;
    }
    #endregion
}
