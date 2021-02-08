using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public string mapFolderPath = @"D:\Program Files\Unity Projects\juBeatSim\Assets\Maps\";
    //public String songFile = "";
    public string[] files = Directory.GetFiles(@"D:\Program Files\Unity Projects\juBeatSim\Assets\Maps\");
    public string songButtonFile;
    public string songTimingFile;
    public string songButtonFileDir;
    public string songTimingFileDir;
    public List<int> buttonID;
    public List<float> onBeat;

    //Below is Map Data not incorporated into file reading yet
    public int mapID;
    public float songBPM;
    public float firstBeatOffset;
    public float approachRate;
    //end of map data

    public static Map Instance;

    void Awake(){
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        firstBeatOffset = 0; //47ms
        approachRate = 0.4f; //500ms
        songBPM = 170f;
        //songButtonFile = filePath + songButtonFile;
        //songTimingFile = filePath + songTimingFile;
        //songFile = filePath + songFile;
        songButtonFileDir = mapFolderPath + mapID + songButtonFile;
        songTimingFileDir = mapFolderPath + mapID + songTimingFile;
        LoadMap();
    }
    //LATER LOADMAP NEEDS TO TAKE IN MAP ID
    public void LoadMap(){
        GetMapNoteData();
    }
    private void GetMapNoteData(){
        if(File.Exists(songButtonFileDir) && File.Exists(songTimingFileDir)) {
            //Button Mapping File
            StreamReader sr = new StreamReader(songButtonFileDir);
            sr.BaseStream.Seek(0, SeekOrigin.Begin); //start from beginning
            var line = sr.ReadLine();
            int ID;
            while(line != null) {
                //add ID to list at position item (if needed for Lists)
                ID = int.Parse(line);
                //Debug.Log("ID: " + ID);
                buttonID.Add(ID);
                line = sr.ReadLine();
            }
            //Timing File
            sr.Close();
            StreamReader sr2 = new StreamReader(songTimingFileDir);
            sr2.BaseStream.Seek(0, SeekOrigin.Begin);
            line = sr2.ReadLine();
            float time;
            while(line != null) {
                //add time to list at position item (if needed for Lists)
                time = float.Parse(line);
                //Debug.Log("Beat Timing: " + time);
                onBeat.Add(time);
                line = sr2.ReadLine();
            }
            //Ensuring lists are of equal size
            int difference = buttonID.Count - onBeat.Count;
            if(difference > 0) { //buttonID has too many values
                for(int i = difference; i > 0; i--) {
                    //Debug.Log("Removed and extra item from buttonID");
                    buttonID.RemoveAt(buttonID.Count - 1);
                }
            }
            else if(difference < 0) { //onBeat has too many values
                for(int i = difference; i < 0; i++) {
                    //Debug.Log("Removed and extra item from onbeat");
                    onBeat.RemoveAt(onBeat.Count - 1);
                }
            }
        }
        else {
            Debug.Log("One or both files do not exist.");
        }
    }
    private void GetMapBackground(){
        //pull map bg from folder with map ID
    }
    private void GetMapData(){
        //file containing AR, Offset, and BPM
    }
}
