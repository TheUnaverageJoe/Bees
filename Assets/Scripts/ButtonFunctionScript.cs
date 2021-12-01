using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void TogglePause() {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
    }
}
