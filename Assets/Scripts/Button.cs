using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //public GameObject[,] noteGrid;
    public KeyCode keyAssigned;
    //public int xPosition;
    //public int yPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        buttonDown();
    }

    private void buttonDown() {
        if(Input.GetKeyDown(keyAssigned)) {
            transform.localScale = new Vector2(1.2f, 1.2f);
        }
        if(Input.GetKeyUp(keyAssigned)) {
            transform.localScale = new Vector2(1.3f, 1.3f);
        }
    }
}
    
