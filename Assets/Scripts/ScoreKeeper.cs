using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Judgement {
    miss,
    bad,
    good,
    perfect
}

public class ScoreKeeper : MonoBehaviour
{
    public int score;
    public int combo;
    public int multiplier;
    public int maxCombo;

    public static ScoreKeeper Instance;

    void Awake(){
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        resetScore();
    }

    // Update is called once per frame
    void Update()
    {
        findMultiplier();
    }
    public void addScore(Judgement judge){
        if(judge == Judgement.perfect) {
            score += 100 * multiplier;
            combo++;
        }
        else if(judge == Judgement.good) {
            score += 50 * multiplier;
            combo++;
        }
        else if(judge == Judgement.bad) {
            score += 25 * multiplier;
            combo = 0;
        }
        else if(judge == Judgement.miss) {
            combo = 0;
        }
        updateMaxCombo();
        Debug.Log("Score is now: " + score);
    }
    public void findMultiplier(){
        if(combo >= 100) {
            multiplier = 4;
        }
        if(combo >= 50) {
            multiplier = 3;
        }
        if(combo >= 20) {
            multiplier = 2;
        }
        else {
            multiplier = 1;
        }
    }
    public void resetScore() {
        score = 0;
        combo = 0;
        maxCombo = 0;
        multiplier = 1;
    }
    public void updateMaxCombo(){
        if(combo > maxCombo) {
            maxCombo = combo;
        }
    }
}
