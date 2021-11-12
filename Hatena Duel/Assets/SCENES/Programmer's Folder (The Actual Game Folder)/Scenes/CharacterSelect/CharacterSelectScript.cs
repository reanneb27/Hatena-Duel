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

    public static Character SelectedCharacter = Character.PrinceHaul;
    public static Arena SelectedArena = Arena.Heaven;

    // Start is called before the first frame update
    void Start()
    {
        SelectedCharacter = Character.PrinceHaul;
        SelectedArena = Arena.Heaven;

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
    }
    public void SelectArkayn()
    {
        animController.Play("arkayn_idle_anim");
        SelectedCharacter = Character.Arkayn;

        SetCharacterPreviewTexts("Arkayn", "Leads the Heaven world. He guards the sapphire stone");
    }
    public void SelectVhoKiz()
    {
        animController.Play("vho'kiz_idle_anim");
        SelectedCharacter = Character.VhoKiz;

        SetCharacterPreviewTexts("Vho’kiz", "Leads the Void world. He guards the ruby stone");
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
    #endregion
}
