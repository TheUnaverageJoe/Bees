using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BeeBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject currentTarget, hive, currentFlower;
    public bool foundFlower, isExploring, goingHome, atTarget;
    public float nectar, rotateChance, rotateAmount;
    HiveBehavior hiveScript;
    //MAX_NECTAR is 50 because its approx 50mg of nectar
    private const float MAX_NECTAR = 50;


    // NOTE: here we potentially include "Boid" behavior
    //       and check other bees in our radius to prevent
    //       collision and simulate a swarm

    // Start is called before the first frame update
    void Start()
    {
        // Bee is at hive
        agent = GetComponent<NavMeshAgent>();
        hive = GameObject.FindGameObjectWithTag("Hive");
        hiveScript = hive.GetComponent<HiveBehavior>();
        agent.speed = 3;
        //Debug.Log(agent.isOnNavMesh);

        foundFlower = false;
        isExploring = true;
        goingHome = false;
        atTarget = false;

        nectar = 0;

        currentTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        rotateChance = Random.Range(0.0f, 10.0f);
        rotateAmount = Random.Range(-20.0f, 20.0f);

        // Go to target
        if (currentTarget != null && !atTarget) {

            //Debug.Log("Has a Target" + currentTarget.transform.position + GetInstanceID());
            agent.SetDestination(currentTarget.transform.position);
        }
        // Slurp nectar from currentFlower (AKA currentTarget)
        if (atTarget && currentTarget != null && currentTarget.CompareTag("Flower") && nectar < MAX_NECTAR) {
            if (currentTarget.GetComponent<FlowerBehavior>().suckNectar()) {
                nectar++;
                //Debug.Log("Nectar is: " + nectar);
            } else {
                isExploring = true;
                foundFlower = false;
                atTarget = false;
                goingHome = false;
            }
        }
        // Go home
        if (nectar >= MAX_NECTAR) {
            isExploring = false;
            atTarget = false;
            goingHome = true;
            currentTarget = hive;
        }

        // Impliment:
        // drop off nectar at hive
        if(goingHome && atTarget){
            hive.GetComponent<HiveBehavior>().Nectar += nectar;
            nectar = 0;
            this.neutralState();
        }

        // Impliment:
        // isExploring pathfinding
        if(isExploring){
            //Debug.Log("Should be exploring");
            agent.SetDestination(transform.position+transform.forward);
            if(rotateChance < 0.05f){
                //Debug.Log("Rotated");
                transform.RotateAround(transform.position, Vector3.up, rotateAmount);
            }
        }
    }

    // neutralState()
    // Reset all state values to neutral (AKA bee spawns in hive)
    public void neutralState() {
        foundFlower = false;
        isExploring = false;
        goingHome = false;
        atTarget = false;
    }

    // recieveSignal()
    // Transition event handler
    // Pre:  int - signal code:
    //             0 for "go find flower"
    //             1 for "go home"
    // Post: bool - true if signal was recieved, else false
    public bool recieveSignal(int signal) {
        // 0: Exploring
        if (signal == 0) {
            // Maintain state
            foundFlower = false;
            isExploring = true;
            goingHome = false;
            atTarget = false;
            
            return true;
        }
        // 1: Go home
        else if (signal == 1) {
            // Maintain state
            goingHome = true;
            currentTarget = hive;
            atTarget = false;

            return true;
        }
        return false;
    }

    // dropOffNectar()
    // Called by Hive
    // Pre:  none
    // Post: returns nectar in inventory
    // Hive doesnt need agency, function is depreciated
    public float dropOffNectar() {
        float temp = nectar;
        nectar = 0;
        return temp;
    }

    public void foundFlowerFunc(GameObject flower){
        //Debug.Log("foundFlowerFunc was called");
        foundFlower = true;
        goingHome = false;
        atTarget = false;
        isExploring = false;
        currentFlower = flower;
        currentTarget = flower;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Bee hit something physically");
        if (other.gameObject.CompareTag("Hive") &&
            currentTarget!=null && currentTarget == hive) {

            Debug.Log("Touched hive");
            //GameObject child = transform.GetChild(0).gameObject;
            //child.SetActive(false);
            //gameObject.SetActive(false);
            hiveScript.storedBees++;
            Destroy(this.gameObject);
            goingHome = false;
            atTarget = true;
        }
        if (other.gameObject.CompareTag("Flower")) {
            //isExploring = false;
            foundFlower = false;
            atTarget = true;
            //currentTarget = other.gameObject;
            //currentFlower = other.gameObject;
        }
    }
}
