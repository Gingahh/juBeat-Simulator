using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject notePrefab;
    public bool resolved, scored, doneListening;
    public GameObject noteButton;
    public KeyCode noteKey;

    public static Note Instance;

    void Awake(){
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        resolved = false;
        scored = false;
        noteButton = transform.parent.gameObject;
        noteKey = noteButton.GetComponent<Button>().keyAssigned;
    }
}
