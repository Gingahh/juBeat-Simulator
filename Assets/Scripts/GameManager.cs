using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{
    selecting,
    playing,
    results
}

public class GameManager : MonoBehaviour
{
    public GameState state;

    public GameObject notePrefab;
    //public GameObject audioPrefab;

    public AudioSource musicSource;

    public float secPerBeat; //seconds for/between each song beat
    public float songPosition; //song position in seconds
    public float songPositionInBeats;
    public float dspSongTime; //Time since MAP has started in seconds
    public float spawnTime; //Time note should be spawned to sync with music (in beats)
    public float approachRateInBeats; //game runs on beats baby

    public int notesOnScreen;

    //Below has been moved to Map file
    //public float songBPM;
    //public float firstBeatOffset;
    //public float approachRate; //Time before the note should be hit (approach rate) in seconds

    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.selecting;
        /*musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / Map.Instance.songBPM;
        dspSongTime = (float)AudioSettings.dspTime; //dspTime is when the mp3 started
        approachRateInBeats = Map.Instance.approachRate / secPerBeat;
        notesOnScreen = 0;

        //musicSource.Play();*/
    }
    // Update is called once per frame
    void Update()
    {
        if(state == GameState.selecting) {
            if(Input.GetKeyDown(KeyCode.Space)){
                resetGame();
                musicSource.Play();
                Debug.Log("Gamestate is now 'playing'");
                state = GameState.playing;
            }
            //probably just if space pressed gamestate = playing
        }
        else if(state == GameState.playing) {
            //something something if not generated generate UI
            if(!Build.Instance.gridBuilt) {
                Debug.Log("Building New Grid");
                Build.Instance.BuildButtonGrid();
                Build.Instance.SetBindings();
            }

            //Timing
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - Map.Instance.firstBeatOffset);
            songPositionInBeats = songPosition / secPerBeat;
            //Timing

            //Notes
            if(Map.Instance.buttonID.Count > 0) { //NOTE: THIS IS BETWEEN 10-15ms late always very cringe
                if(songPositionInBeats >= Map.Instance.onBeat[0] - approachRateInBeats) { //to avoid cringe slightly offbeat notes a more accurate timing method should be used lol
                    spawnNote(Map.Instance.buttonID[0]);
                    Map.Instance.buttonID.RemoveAt(0);
                    Map.Instance.onBeat.RemoveAt(0);
                }
            }
            else if(notesOnScreen == 0) {
                Debug.Log("Gamestate is now 'results'");
                Build.Instance.DestroyButtonGrid();
                musicSource.Stop();
                state = GameState.results;
            }
        }
        else if(state == GameState.results) {
            //results screen
            //probably space to play again
            if(Input.GetKeyDown(KeyCode.Space)){
                Debug.Log("Gamestate is now 'playing'");
                ScoreKeeper.Instance.resetScore();
                Map.Instance.LoadMap(); //pass in mapID later
                resetGame();
                musicSource.Play();
                state = GameState.playing;
            }
        }
    }
    public void spawnNote(int buttonID/*GameObject button, float noteBeat*/){
        notesOnScreen++;

        GameObject button = Build.Instance.allButtons[buttonID];
        float xPos = button.GetComponent<Button>().transform.position.x;
        float yPos = button.GetComponent<Button>().transform.position.y;
        Vector2 spawnPosition = new Vector2(xPos, yPos);
        GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
        Debug.Log("note instantiated at:\n- " + songPosition + " seconds\n- " + songPositionInBeats + "beats");
        newNote.transform.parent = button.transform;
        StartCoroutine(noteCycleCo(newNote));

    }
    private IEnumerator noteCycleCo(GameObject note){
        StartCoroutine(ListenForKeyCo(note)); //Coroutine for input
        //perhaps a cringe solution

        float timeElapsed = 0f; //for hit windows
        float timePerLoop = Map.Instance.approachRate / 50f;
        Vector3 scaleChange = new Vector3(0.02f, 0.02f, 0f); //Change in scale each loop, this is smooth enough, should go unchanged
        note.transform.localScale = new Vector2(0.0f, 0.0f);
        Debug.Log("scale set to 0");
        while(note.transform.localScale.x < 1.0f && note.transform.localScale.y < 1.0f){ //10 loops if changed by .1f each time: so it's 50 loops at .02f
            note.transform.localScale += scaleChange;
            if(note.GetComponent<Note>().resolved && !note.GetComponent<Note>().scored) {
                ScoreKeeper.Instance.addScore(getJudgement(timeElapsed));
                note.GetComponent<Note>().scored = true;
            }
            yield return new WaitForSeconds(timePerLoop); //time between each loop: so with 50 loops this is a 0.5 second approach rate
            timeElapsed += timePerLoop;
        }
        //SpriteRenderer noteSprite = GetComponent<SpriteRenderer>();
        while(timeElapsed <= Map.Instance.approachRate + 0.15f) { //Kill note after 150ms
            //make it gradually transparent here in the future
            if(note.GetComponent<Note>().resolved && !note.GetComponent<Note>().scored) {
                ScoreKeeper.Instance.addScore(getJudgement(timeElapsed));
                note.GetComponent<Note>().scored = true;
            }
            yield return new WaitForSeconds(timePerLoop);
            timeElapsed += timePerLoop;
        }
        //Destroy note: end of lifecycle... also give a miss if they never inputted anything
        if(!note.GetComponent<Note>().resolved) {
            note.GetComponent<Note>().resolved = true;
            ScoreKeeper.Instance.addScore(getJudgement(timeElapsed));
        }
        while(!note.GetComponent<Note>().doneListening) {
            //sort of like mutual exclusion I think I can't believe I'm using this thanks golden
            yield return new WaitForSeconds(0.001f); //FUCK YES IT WORKED GOLDEN GOATED
        }
        Destroy(note);
        notesOnScreen--;
        //yield return new WaitForSeconds(0.0f); //idk if this is necessary lol
    }
    public IEnumerator ListenForKeyCo(GameObject note){
        while(!note.GetComponent<Note>().resolved && !Input.GetKeyDown(note.GetComponent<Note>().noteKey)) {
            yield return null;
        }
        if(!note.GetComponent<Note>().resolved) {
            Debug.Log("key was indeed pressed while a note was out");
            note.SetActive(false);
            note.GetComponent<Note>().resolved = true;
        }
        note.GetComponent<Note>().doneListening = true;
    }
    /*public IEnumerator ListenForKeyCo(GameObject note){
        while(!Input.GetKeyDown(note.GetComponent<Note>().noteKey)) {
            yield return null;
            if(note.GetComponent<Note>() == null) break;
        }
        Debug.Log("key was indeed pressed while a note was out");
        note.GetComponent<Note>().resolved = true;
        note.SetActive(false);
        Destroy(note);
        yield return null;
    }*/
    public Judgement getJudgement(float timeElapsed){
        if(Mathf.Abs(timeElapsed - Map.Instance.approachRate) <= 0.05) { //50ms
            Debug.Log("Judgement: Perfect");
            return Judgement.perfect;
        }
        else if(Mathf.Abs(timeElapsed - Map.Instance.approachRate) <=  0.1f) { //100ms
            Debug.Log("Judgement: Good");
            return Judgement.good;
        }
        else if(Mathf.Abs(timeElapsed - Map.Instance.approachRate) <= 0.15f) { //150ms
            Debug.Log("Judgement: Bad");
            return Judgement.bad;
        }
        else {
            Debug.Log("Judgement: Miss");
            return Judgement.miss;
        }
        //Here or maybe somewhere else visualize judgement
    }
    public void resetGame(){
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / Map.Instance.songBPM;
        dspSongTime = (float)AudioSettings.dspTime; //dspTime is when the mp3 started
        approachRateInBeats = Map.Instance.approachRate / secPerBeat;
        notesOnScreen = 0;
    }
    //poggers it won't submit to github
}
