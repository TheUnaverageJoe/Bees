using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBehavior : MonoBehaviour
{
    // NOTES: 11/23/2021
    // bool to maintain if flower is depleted
    // change the state of collider based on depleted or not
    // timer for replenish Nectar
    private int Nectar;
    private bool depleted;
    SphereCollider objCollider;
    int replenishTime;
    float timer = 0;
    BeeBehavior beeScript;
    TestNavMesh beeTest;

    // Start is called before the first frame update
    void Start()
    {
        // consider frame rate and how fast bee consumes 1 nectar
        Nectar = Random.Range(1, 5);//amount of nectar
        replenishTime = Random.Range(1, 3);//num of minutes till replenish
        objCollider = GetComponent<SphereCollider>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Flower has no nectar left
        //disable its collider because its inactive
        //start incrementing timer
        if(depleted){
            objCollider.enabled = false;
            timer += Time.deltaTime;
        }
        //if  timer has reached the replenishTime minutes
        //enable the collider and reset variables such
        //as nectar, timer, depleted
        if(timer >= replenishTime*60){
            objCollider.enabled = true;
            depleted = false;
            Nectar = Random.Range(1, 5);
            timer = 0;
        }
    }
    public bool suckNectar() {
        if (Nectar <= 0) {
            depleted = true;
            return false;
        } else {
            Nectar--;
            return true;
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Bee")){
            Debug.Log("Trigger went off");
            //Debug.Log(suckNectar());
            //Debug.Log(Nectar);
            //send signal to bee
             beeScript = other.gameObject.GetComponent<BeeBehavior>();
             beeScript.foundFlowerFunc(gameObject);
            //beeScript.sendSignal(found_a_Flower);
        }
    }
}