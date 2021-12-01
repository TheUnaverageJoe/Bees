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
    private const float MAX_NECTAR = 5;
    // NOTE: here we potentially include "Boid" behavior
    //       and check other bees in our radius to prevent
    //       collision and simulate a swarm

    // Start is called before the first frame update
    void Start()
    {
        // Bee is at hive
        agent = GetComponent<NavMeshAgent>();

        foundFlower = false;
        isExploring = true;
        goingHome = false;
        atTarget = false;

        nectar = 0;

        rotateChance = Random.Range(0.0f, 10.0f);
        rotateAmount = Random.Range(-5.0f, 20.0f);

        currentTarget = null;
        hive = GameObject.FindGameObjectWithTag("Hive");
    }

    // Update is called once per frame
    void Update()
    {
        // Go to target
        if (currentTarget != null && !atTarget) {
            agent.SetDestination(currentTarget.transform.position);
        }
        // Slurp nectar from currentFlower (AKA currentTarget)
        if (atTarget && currentTarget.CompareTag("Flower") && nectar < MAX_NECTAR) {
            if (currentTarget.GetComponent<FlowerBehavior>().suckNectar()) {
                nectar++;
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
        if(isExploring && !atTarget){
            agent.SetDestination(transform.position+transform.forward);
            if(rotateChance < 0.1f){
                Debug.Log("Rotated");
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
    // Hive doesnt need agency function is depreciated
    public float dropOffNectar() {
        float temp = nectar;
        nectar = 0;
        return temp;
    }

    private void OnCollision(Collision other) {
        if (other.gameObject.CompareTag("Hive")) {
            goingHome = false;
            atTarget = true;
        }
        if (other.gameObject.CompareTag("Flower")) {
            isExploring = false;
            foundFlower = true;
            atTarget = true;
            currentTarget = other.gameObject;
            currentFlower = other.gameObject;
        }
    }
}
