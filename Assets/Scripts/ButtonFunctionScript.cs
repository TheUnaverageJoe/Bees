using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionScript : MonoBehaviour
{
    private GameObject hive;
    private GameObject manager;
    private UIValueScript sliderScript;
    private HiveBehavior hiveScript;
    // Start is called before the first frame update
    void Start()
    {
        hive = GameObject.Find ("Hive");
        hiveScript = hive.GetComponent <HiveBehavior>();
        manager = GameObject.Find ("EventSystem");
        sliderScript = manager.GetComponent <UIValueScript>();
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

    public void deployButtonFunction() {
        hiveScript.deployNBees(sliderScript.totalValue);
    }
}
