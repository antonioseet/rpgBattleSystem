using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public GameObject tonyGameObj;
    public GameObject roxyGameObj;
    public GameObject forestCreatureGameObj;
    public GameObject JackOGameObj;

    public GameObject blueHealthChunk;
    public GameObject redHealthChunk;
    private Text blueChunk;
    private Text redChunk;
    private float fadeTimer;

    public Announcer announcer;
    public new AudioManager audio;
    public SfxManager sfx;

    public GameObject attackButton, specialAttackButton, potionButtonObject;
    private Button potionButton;

    private List<Character> partyList = new List<Character>();
    private List<Character> enemyList = new List<Character>();

    public Text playerHealthText;
    public Text enemyHealthText;
    public Text turnCounterText;

    public GameObject dialogueBox;
    public DialogueText dialogueText;

    private int numberOfPotions;
    private int turnCounter = 1;
    private Queue<Instruction> instructions = new Queue<Instruction>();

    private float battleTime;
    private float turnTimer;
    private float timePerAttack = 4;
    private float notificationTime;

    private bool lowHealthSoundClipPlayed = false;
    private bool lateTurnSoundClipPlayed = false;
    private bool gameOver = false;

    private const int POTION_HP = 120;

    private Vector3 originalPosition, originalEnemyPosition;


    public GameState gameState = GameState.BattleStart;
    private FaintEnum faintEnum = FaintEnum.whoFainted;

    public enum GameState
    {
        BattleStart,
        PlayerActionSelect,
        ExecuteTurn,
        CharacterFaints,
        GameOverWin,
        GameOverLoss
    }

    private enum FaintEnum
    {
        whoFainted,
        whoComesOut,
        skip
    }

    // Start is called before the first update()
    void Start()
    {
        characterInit();
        enemyInit();
        numberOfPotions = 3;
        battleTime = Time.time;
        notificationTime = Time.time;

        potionButton = potionButtonObject.GetComponent<Button>();

        blueChunk = blueHealthChunk.GetComponent<Text>();
        redChunk = redHealthChunk.GetComponent<Text>();

    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.BattleStart:
                attackButton.SetActive(false);
                specialAttackButton.SetActive(false);
                potionButtonObject.SetActive(false);
                potionButton.interactable = false;
                partyList[0].setIsMoving(false);
                enemyList[0].setIsMoving(false);
                dialogueBox.SetActive(false);

                if (Time.time - battleTime > 5)
                {
                    gameState = GameState.PlayerActionSelect;
                }

                break;
            case GameState.PlayerActionSelect:

                turnCounterText.text = "Turn: " + turnCounter;

                if (!lowHealthSoundClipPlayed & partyList[0].getCurrentHealth() < partyList[0].getMaxHealth() / 4)
                {
                    if (enemyList[0].getCurrentHealth() < enemyList[0].getMaxHealth() / 4)
                        announcer.announceBothLowHealth();
                    else
                        announcer.announcePlayerLowHealth();
                    lowHealthSoundClipPlayed = true;
                }
                else if (!lateTurnSoundClipPlayed & turnCounter > 4)
                {
                    announcer.announceLateTurn();
                    lateTurnSoundClipPlayed = true;
                }

                if (Time.time - turnTimer > 5)
                {
                    dialogueBox.SetActive(true);
                    dialogueText.setNewText("What will " + partyList[0].characterName + " do?");
                    attackButton.SetActive(true);
                    specialAttackButton.SetActive(true);
                    potionButtonObject.SetActive(true);
                    partyList[0].setIsMoving(true);
                    enemyList[0].setIsMoving(true);
                }

                //if player takes too long to choose action announcer speaks
                if (Time.time - turnTimer > 15)
                {
                    int randInt = Random.Range(0, 100);
                    turnTimer = Time.time;

                    if (randInt < 70)
                        announcer.announceWaiting();
                }

                break;
            case GameState.ExecuteTurn:

                attackButton.SetActive(false);
                specialAttackButton.SetActive(false);
                potionButtonObject.SetActive(false);
                partyList[0].setIsMoving(false);
                enemyList[0].setIsMoving(false);

                if (instructions.Count > 0 && Time.time - turnTimer > timePerAttack)
                {
                    Instruction instruction = instructions.Dequeue();
                    instruction.execute();
                    turnTimer = Time.time;
                    fadeTimer = Time.time;

                    if (partyList[0].hasFainted || enemyList[0].hasFainted)
                    {
                        gameState = GameState.CharacterFaints;
                        instructions.Clear();
                    }

                }
                else if (instructions.Count == 0 & Time.time - turnTimer > timePerAttack)
                {
                    gameState = GameState.PlayerActionSelect;
                    turnCounter++;
                }

                

                break;
            case GameState.CharacterFaints:

                switch (faintEnum)
                {

                    case FaintEnum.whoFainted:
                        if (Time.time - turnTimer > timePerAttack)
                        {
                            if (partyList[0].hasFainted)
                            {
                                dialogueText.setNewText(partyList[0].characterName + " has fainted!");
                                print("Someone Fainted");
                                if (!announcer.announcer.isPlaying)
                                    announcer.announceFaint();
                            }
                            else
                            {
                                dialogueText.setNewText(enemyList[0].characterName + " has fainted!");
                            }
                            
                            turnTimer = Time.time;
                            faintEnum = FaintEnum.whoComesOut;
                        }

                        if (partyHasBeenDefeated(partyList) | partyHasBeenDefeated(enemyList))
                            faintEnum = FaintEnum.skip;

                        break;
                    case FaintEnum.whoComesOut:
                        if ( Time.time - turnTimer > timePerAttack)
                        {
                            if (partyList[0].hasFainted)
                            {
                                partyList[0].characterOBJ.SetActive(false);
                                switchCharPosition(partyList);
                                dialogueText.setNewText(partyList[0].characterName + " is ready to fight!");
                                numberOfPotions = 3;
                            }
                            else
                            {
                                //Here we check who fainted and what we do next
                                enemyList[0].characterOBJ.SetActive(false);
                                switchCharPosition(enemyList);
                                dialogueText.setNewText(enemyList[0].characterName + " is ready to fight!");
                            }
                            turnTimer = Time.time;
                            faintEnum = FaintEnum.whoFainted;
                            gameState = GameState.PlayerActionSelect;
                        }
                        break;
                        
                    case FaintEnum.skip:
                        
                        if (Time.time - turnTimer > timePerAttack)
                        {
                            if (!gameOver)
                            {
                                if (partyHasBeenDefeated(partyList))
                                {
                                    gameState = GameState.GameOverLoss;
                                    partyList[0].characterOBJ.SetActive(false);
                                    audio.playLoss();
                                }
                                else if (partyHasBeenDefeated(enemyList))
                                {
                                    gameState = GameState.GameOverWin;
                                    enemyList[0].characterOBJ.SetActive(false);
                                    audio.playWin();
                                }
                                gameOver = true;
                            }
                        }
                            break;
                }
                break;

            case GameState.GameOverWin:
                if(Time.time - turnTimer > 5)
                {
                    dialogueText.setNewText("Victory for the BLUE corner!");
                    enemyList[1].characterOBJ.SetActive(false);
                    turnTimer = Time.time;
                }
                break;
                
            case GameState.GameOverLoss:
                if (Time.time - turnTimer > 5)
                {
                    dialogueText.setNewText("Victory for the RED corner!");
                    partyList[1].characterOBJ.SetActive(false);
                    turnTimer = Time.time;
                }
                break;
            default:
                break;
        }
        
        uiUpdates();
    }

    public bool partyHasBeenDefeated(List<Character> party)
    {
        return party[0].hasFainted && party[1].hasFainted;
    }

    public void changeInstructionOrderBasedOnSpeed(Queue<Instruction> instructions)
    {
        Instruction temp1 = instructions.Dequeue();
        Instruction temp2 = instructions.Dequeue();

        int instructionSpeed1 = temp1.attacker.speedStat;
        int instructionSpeed2 = temp2.attacker.speedStat;

        if(instructionSpeed1 >= instructionSpeed2)
        {
            instructions.Enqueue(temp1);
            instructions.Enqueue(temp2);
        }
        else
        {
            instructions.Enqueue(temp2);
            instructions.Enqueue(temp1);
        }
    }

    //Initialize the characters and their attacks, then place into list
    private void characterInit()
    {
        Character tony = tonyGameObj.GetComponent<Character>();
        Character roxy = roxyGameObj.GetComponent<Character>();

        tony.assignAttacks(
            new Attack("Fire Sword", 51, 95, false),
            new Attack("Lightning Bolt", 54, 95, true));
        roxy.assignAttacks(
            new Attack("Jump Kick", 45, 95, false),
            new Attack("Flux", 60, 95, true));

        partyList.Add(tony);
        partyList.Add(roxy);
        partyList[0].setIsMoving(true);

    }

    private void enemyInit()
    {
        Character forestCreature = forestCreatureGameObj.GetComponent<Character>();
        Character JackO = JackOGameObj.GetComponent<Character>();

        forestCreature.assignAttacks(
            new Attack("Slash", 60, 85, false),
            new Attack("Bite", 62, 89, true));
        JackO.assignAttacks(
            new Attack("Sickle Slash", 60, 85, false),
            new Attack("Haunting Screech", 75, 60, true));

        enemyList.Add(forestCreature);
        enemyList.Add(JackO);
        enemyList[0].setIsMoving(true);
    }

    //Switch the position of the characters and assign appropriate mainCharacter
    public void switchCharPosition(List<Character> party)
    {
        float tempX = party[0].transform.position.x;
        float tempY = party[0].transform.position.y + 1;
        float tempZ = party[0].transform.position.z;

        party[0].transform.position = new Vector3(
            party[1].transform.position.x,
            party[1].transform.position.y + 1,
            party[1].transform.position.z);

        party[1].transform.position = new Vector3(tempX, tempY, tempZ);

        Character tempChar = party[0];
        tempChar.setIsMoving(false);
        party.RemoveAt(0);
        party.Add(tempChar);
        lowHealthSoundClipPlayed = false;
    }


    // (1) Update health text for player and opponent
    // (2) Update button Text for individual characters
    public void uiUpdates()
    {
        bool androidBuild = false;

        string tick = "||";
        int increment = 20;

        if (androidBuild)
        {
            tick = "||";
            increment = 20;
        }
        else
        {
            tick = "|";
            increment = 10;
        }


        //Health bars for the characters
        string healthTicks = "";
        for (int i = 0; i < partyList[0].getDisplayedHealth(); i += increment)
        {
            healthTicks = healthTicks + tick;
        }
        playerHealthText.text = partyList[0].characterName + ":" + healthTicks + "  "
                              + partyList[0].getDisplayedHealth() + "/" + partyList[0].getMaxHealth();

        string enemyHealthTicks = "";
        for (int i = 0; i < enemyList[0].getDisplayedHealth(); i += increment)
        {
            enemyHealthTicks = enemyHealthTicks + tick;
        }
        enemyHealthText.text = enemyList[0].characterName + ":" + enemyHealthTicks + "  "
                              + enemyList[0].getDisplayedHealth() + "/" + enemyList[0].getMaxHealth();

        //Labels for the buttons
        attackButton.GetComponentInChildren<Text>().text = "Use "+partyList[0].getAttackNames()[0];
        specialAttackButton.GetComponentInChildren<Text>().text = "Cast "+partyList[0].getAttackNames()[1];


        //Potion button interactibility
        if (numberOfPotions <= 0)
        {
            potionButton.interactable = false;
            potionButtonObject.GetComponentInChildren<Text>().text = "Out of potions";
        }
        else if (partyList[0].isFullHealth())
        {
            potionButton.interactable = false;
            potionButtonObject.GetComponentInChildren<Text>().text = "HP FULL";
        }
        else
        {
            potionButton.interactable = true;
            potionButtonObject.GetComponentInChildren<Text>().text = "Use potion (" + numberOfPotions + ")";
        }

        //fade away the damage indicator
        fadeDamageIndicators();

    }

    public void fadeDamageIndicators()
    {
        if (Time.time - fadeTimer >= .01f)
        {
            if (blueChunk.color.a >= 0)
            {
                Color color = blueChunk.color;
                color.a = color.a - .007f; ;
                blueChunk.color = color; 
                fadeTimer = Time.time;
            }

            if (redChunk.color.a >= 0)
            {
                Color color = redChunk.color;
                color.a = color.a - .007f; ;
                redChunk.color = color;
                fadeTimer = Time.time;
            }
        }
       
    }

    public void regularAttackInstruction()
    {
        turnTimer = Time.time;
        Instruction attackOpponent = new Instruction(partyList[0], enemyList[0], partyList[0].getAttack(), announcer, turnCounter, dialogueText, sfx, blueChunk, redChunk);
        instructions.Enqueue(attackOpponent);
        instructions.Enqueue(randomEnemyAction());
        changeInstructionOrderBasedOnSpeed(instructions);
        gameState = GameState.ExecuteTurn;
    }

    public void specialAttackInstruction()
    {
        turnTimer = Time.time;
        Instruction attackOpponentInstruction = new Instruction(partyList[0], enemyList[0], partyList[0].getSpecialAttack(), announcer, turnCounter, dialogueText, sfx, blueChunk, redChunk);
        instructions.Enqueue(attackOpponentInstruction);
        instructions.Enqueue(randomEnemyAction());
        changeInstructionOrderBasedOnSpeed(instructions);
        gameState = GameState.ExecuteTurn;
    }

    public void potionInstruction()
    {
        turnTimer = Time.time;
        int healthHealed = POTION_HP;
        if (numberOfPotions > 0)
        {
            //we should not have to check here because we are disabling the button in UI updates
            //Come back to it
            
            numberOfPotions -= 1;

            int currentHP = partyList[0].getCurrentHealth();
            int maxHP = partyList[0].getMaxHealth();

            //print(currentHP + "" + maxHP + "" + maxHP-currentHP));

            if (currentHP + POTION_HP > maxHP)
            {
                healthHealed = maxHP - currentHP;
                blueChunk.text = "+" + (maxHP-currentHP);
            }
            else
            {
                blueChunk.text = "+" + POTION_HP;
            }
            dialogueText.setNewText(partyList[0].characterName + " healed for " + healthHealed + " HP!");
            blueChunk.color = Color.green;
            partyList[0].changeHealth(POTION_HP);

        }
        instructions.Enqueue(randomEnemyAction());
        announcer.announceHeal();
        gameState = GameState.ExecuteTurn;
    }

    private Instruction randomEnemyAction()
    {
        return new Instruction(enemyList[0], partyList[0], getRandomAttackFromList(enemyList[0].getAttacks()), announcer, turnCounter, dialogueText, sfx, blueChunk, redChunk);
    }

    private Attack getRandomAttackFromList(List<Attack> list)
    {
        float randomListIndex = Random.Range(0F, list.Count - 1);
        int randomIndex = Mathf.RoundToInt(randomListIndex);
        return list[randomIndex];
    }

}