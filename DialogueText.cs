using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueText : MonoBehaviour
{

    public Text dialogueTextObject;
    private string currentText;
    private bool isAnimating;
    private int charArrayCounter;
    private char[] charArray;

    private float textTimer = 0;
    private float textSpeed = .03f;

    public GameStateManager gameStateManager;


    // Start is called before the first frame update
    void Start()
    {
        currentText = "";
        isAnimating = false;
        charArray = new char[0];
        charArrayCounter = 0;
    }

    // Update is called once per frame but we want the dialgue to update based on time
    // this allows for the rolling text animation
    void Update()
    {

        if(Time.time - textTimer > textSpeed)
        {
            if (gameStateManager.gameState != GameStateManager.GameState.BattleStart)
            {
                if (isAnimating)
                {
                    if (charArray.Length != 0 & charArrayCounter < charArray.Length)
                    {
                        dialogueTextObject.text += charArray[charArrayCounter];
                        charArrayCounter++;
                        textTimer = Time.time;
                    }
                }
            }

            if (charArrayCounter == charArray.Length)
            {
                isAnimating = false;
                charArrayCounter = 0;
            }
        }
    }

    public void setNewText(string newText)
    {
        // Only change if there is new text to display
        if (!currentText.Equals(newText))
        {
            dialogueTextObject.text = "";
            currentText = newText;
            charArray = newText.ToCharArray();
            isAnimating = true;
        }
        
    }



}
