using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoreScript : MonoBehaviour
{
    public TextMeshProUGUI MainTextGUI;

    private int _LoreTextCount;
    private int LoreTextCount { get => _LoreTextCount; set { _LoreTextCount = value; NextText(); } }
    private string[] LoreText = 
        { 
            "A long time ago, the outside walls of Hatena City were packed with hostile mysterious creatures. ",
            "King Fulchard searched for a way to protect his newborn son and his people. ",
            "The King found out from the written scripts of their ancestors that three stones are needed: Sapphire, Emerald, Ruby. ",
            "They are to be placed in equal spaces outside the city to protect the city from these mysterious creatures. ",
            "The King had collected these stones from three different worlds. These stones have protected the city for over 30 years. ",
            "The prince found out that the three stones had lost power to protect the city. ",
            "Asking his father why the stones have lost their power, the King said that these stones are only effective during his fathers' era.",
            "Since the absence of the power of these stones, mysterious creatures had been living outside the walls of Hatena, leaving the city unprotected from frequent impulsive attacks of these creatures. ",
            "Because the King has gotten older and is no longer capable of long adventures, the prince has to find a way to collect the New Era Stones from other worlds.",
            "Your goal is to collect the New Era Stones from the different worlds to be able to protect the city from mysterious creatures."
        };

    // Start is called before the first frame update
    void Start()
    {
        LoreTextCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
            LoreTextCount++;
    }

    void NextText()
    {
        if (LoreTextCount >= LoreText.Length)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        MainTextGUI.text = LoreText[LoreTextCount];
    }
}
