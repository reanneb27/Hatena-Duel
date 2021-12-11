using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public TextMeshProUGUI CharacterNameTMP;
    public TextMeshProUGUI CharacterDescTMP;

    // character skills
    public TextMeshProUGUI Skill1NameTMP;
    public TextMeshProUGUI Skill1DescTMP;

    public TextMeshProUGUI Skill2NameTMP;
    public TextMeshProUGUI Skill2DescTMP;

    public TextMeshProUGUI Skill3NameTMP;
    public TextMeshProUGUI Skill3DescTMP;

    public TextMeshProUGUI UltimateNameTMP;
    public TextMeshProUGUI UltimateDescTMP;

    // default character/arena selection
    public static Character SelectedCharacter = Character.VhoKiz; // temporary; prince haul is supposed to be the default
    public static Arena SelectedArena = Arena.Heaven;

    // Start is called before the first frame update
    void Start()
    {
        SelectedCharacter = Character.PrinceHaul;
        SelectedArena = Arena.Heaven;

        // default selection
        //SelectKingHaul();
        // TODO: set the character buttons interactable here based on the Save.cs values, as well as the arena indicate if completed based on the Save.cs values
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

        SetCharacterPreviewTexts("Prince Haul", "The son of King Fulchard");

        // set skill info
        SetSkill1Text("Boomerang", "Can use immediately after the boomerang returns");
        SetSkill2Text("Shield", "Invulnerable for some time");
        SetSkill3Text("Copy Cat", "Gets a random non-ultimate ability from the enemy");
        SetUltimateText("Adrenaline Rush", "Cooldown is greatly decreased");
    }
    public void SelectArkayn()
    {
        animController.Play("arkayn_idle_anim");
        SelectedCharacter = Character.Arkayn;

        SetCharacterPreviewTexts("Arkayn", "Leads the Heaven world. He guards the sapphire stone");

        // set skill info
        SetSkill1Text("Hell's Flames", "Ignites the ground, dealing damage over time");
        SetSkill2Text("Heaven's Gate", "Creates a wall that only Arkayn can jump over");
        SetSkill3Text("Acid Rain", "Rains acid for 3 seconds at a specified spot");
        SetUltimateText("Divine Intervention", "Heals until rage is depleted. If Arkayn dies during ultimate, he revives half health and 0 rage.");
    }
    public void SelectVhoKiz()
    {
        animController.Play("vho'kiz_idle_anim");
        SelectedCharacter = Character.VhoKiz;

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
