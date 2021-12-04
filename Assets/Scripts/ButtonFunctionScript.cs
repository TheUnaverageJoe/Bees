using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionScript : MonoBehaviour
{
    private GameObject hive;
    private HiveBehavior hiveScript;
    private GameObject slider;
    // Start is called before the first frame update
    void Start()
    {
        hive = GameObject.Find ("Hive");
        hiveScript = hive.GetComponent <HiveBehavior>();
        slider = GameObject.Find ("UISlider");
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

    public void deployButton() {
        float bees = hiveScript.BeeQueue.Count;
        float percent = slider.GetComponent<Slider>().value;
        int totalValue = (int)(bees*percent);
        hiveScript.deployNBees(totalValue);
    }

    public void createButton() {
        hiveScript.produceBee();
    }

    public void giveButton() {
        hiveScript.Nectar += 5;
    }
}
