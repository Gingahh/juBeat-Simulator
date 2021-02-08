using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour {
    public const int GRID_WIDTH = 9;
    public const int GRID_HEIGHT = 9;
    public const int BUTTON_COUNT = 9;

    public GameObject buttonPrefab;
    public GameObject[] allButtons;
    public KeyCode[] keyBindings;

    public bool gridBuilt;

    public static Build Instance;

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        keyBindings = new KeyCode[BUTTON_COUNT];
        keyBindings[0] = KeyCode.Keypad1; //lol why is this the only way I can get this simple ass shit to work
        keyBindings[1] = KeyCode.Keypad4;
        keyBindings[2] = KeyCode.Keypad7; //I am pissed
        keyBindings[3] = KeyCode.Keypad2;
        keyBindings[4] = KeyCode.Keypad5; //I will fix this when I have time
        keyBindings[5] = KeyCode.Keypad8;
        keyBindings[6] = KeyCode.Keypad3;
        keyBindings[7] = KeyCode.Keypad6;
        keyBindings[8] = KeyCode.Keypad9;
        allButtons = new GameObject[BUTTON_COUNT];
        BuildButtonGrid();
        SetBindings();
    }

    // Builds Buttons Obviously
    public void BuildButtonGrid(){
        int buttonNum = 0;
        gridBuilt = true;
        for(int i = 0; i < GRID_WIDTH; i+=3) {
            for(int j = 0; j < GRID_HEIGHT; j+=3) {
                Vector2 targetPosition = new Vector2(i,j);
                GameObject button = Instantiate(buttonPrefab, targetPosition, Quaternion.identity) as GameObject;
                button.transform.parent = this.transform;
                button.name = "Button " + buttonNum; 
                allButtons[buttonNum] = button;
                
                //button.GetComponent<Button>().keyAssigned = keyBindings[buttonNum];
                //button.tag = i + "-" + j;
                buttonNum++;
            }
        }
    }
    public void BuildScoreBoard(){
        //build scoreboard maybe goes here if possible
    }
    public void ShowText(string message){
        //start and play again
        GameObject menutext = new GameObject();
    }
    public void SetBindings(){
        for(int i = 0; i < BUTTON_COUNT; i++) {
            allButtons[i].GetComponent<Button>().keyAssigned = keyBindings[i];
        }
    }
    public void DestroyButtonGrid(){
        for(int i = 0; i < BUTTON_COUNT; i++) {
            Destroy(allButtons[i]);
        }
        gridBuilt = false;
    }
}
