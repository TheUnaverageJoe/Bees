using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveBehavior : MonoBehaviour
{
    // Conversion 
    public float Honey;
    public float Nectar;
    public GameObject bee;
    private GameObject[] Bees;
    public Queue<GameObject> BeeQueue;
    public int totalBees;
    public const float REQUIRED_NECTAR_FOR_BEE = 20;
    //nice and simple numba 1
    public const float REQUIRED_HONEY_FOR_BEE = 1;
    public int enemyHealth, storedBees;
    private int createdBeesCounter;


    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------------------------------
        // Ian's Code
        Honey = 0;
        Nectar = 0;
        enemyHealth = 5;

        createdBeesCounter = 0;
        storedBees = 0;

        // Initialize starting 'totalBees' number of bees
        //bee = GameObject.FindGameObjectWithTag("Bee");
        BeeQueue = new Queue<GameObject>();
        
        for (int i = 0; i < totalBees; i++) {
            GameObject larva = createBee();
            larva.name = "Bee " + createdBeesCounter.ToString();
            createdBeesCounter++;
            storedBees++;
            //BeeQueue.Enqueue(larva);
            //Debug.Log(larva.GetInstanceID());
        }

        //-------------------------------------------------------------

        Bees = GameObject.FindGameObjectsWithTag("Bee");
        //test for future in case we need to dynamically resize Bees array capacity
        //GameObject[] temp = new GameObject[Bees.Length*2];
        
        //copys Bees array starting at index 0 into temp array
        //Bees.CopyTo(temp, 0);
        //Assign temp to bees, they should be same length and have same elements
        //Bees = temp;
        //temp = null;

        //Debug.Log("starting number of bees: " + Bees.Length);
        //Debug.Log("starting number of temp: " + temp.Length);
        //Debug.Log("temp @0: " + Bees[0]);

    }



    // Update is called once per frame for rendering
    //void Update(){
    //    return;
    //}

    void FixedUpdate(){
        //Does hive produce honey or bees?
        //If bees in hive produce honey?
        //for each bee increase rate of nectar to honey conversion
        //if(BeeQueue.)

        if (Nectar >= storedBees*50 && storedBees > 0) {
            Nectar -= storedBees*50;
            Honey += (storedBees);
            
        }
        if(Honey >= REQUIRED_HONEY_FOR_BEE){
            produceBee();
        }
        // From research: it requires nectar from 2 million flowers for
        //  1 lb of honey. That conversion rate is crazy small
        
    }

    // deployNBees()
    // This function dequeues n number of bees from the BeeQueue and 
    // accesses their recieveSignal() function with code 0. This 
    // function returns true if the hive has bees to deploy
    // Pre:  int : number of bees 
    // Post: bool : whether or not command executed properly
    //       true if command executed
    //       return false if no bees in hive
    public bool deployNBees(int n) {
        if (BeeQueue.Count == 0 || BeeQueue.Count < n) return false;
        for (int i = 0; i < n; i++) {
            createBee();
            bee.GetComponent<BeeBehavior>().recieveSignal(0);
        }
        return true;
    }

    // canDefend()
    // A hive-based function that relies solely on the number of bees
    // currently in the hive to determine if the hive can defend itself
    // or not
    bool canDefend(){
        bool canDefend = (enemyHealth < BeeQueue.Count) ? true : false;
        return canDefend;
    }

    // produceBee()
    // Based on a max amount of nectar, create a new bee in the hive
    bool produceBee() {
        if (Nectar < REQUIRED_NECTAR_FOR_BEE || Honey < 1) return false;
        storedBees += 1;
        Nectar -= REQUIRED_NECTAR_FOR_BEE;
        Honey -= REQUIRED_HONEY_FOR_BEE;
        //BeeQueue.Enqueue(createBee());
        return true;
    }

    // createBee()
    // Add new bee clone to BeeQueue
    //NEED to return by reference so the attached scripts have a gameobject to access?
    GameObject createBee() {
    //ref GameObject createBee() {
        // Create clone GameObject and include into queue of bees in hive
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up);
        Vector3 newPosition = new Vector3(
            transform.position.x + Random.Range(-2f, 2f),
            transform.position.y,
            transform.position.z + Random.Range(-2f, 2f));

        GameObject clone = Instantiate(bee, newPosition, rotation);
        //ref GameObject toReturn = ref clone;

        // Set initial state
        clone.GetComponent<BeeBehavior>().neutralState();
        return clone;
        //return ref clone;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Bee")) {
            //storedBees++;
            //Debug.Log("storedBees: " + storedBees);
            Nectar += other.gameObject.GetComponent<BeeBehavior>().dropOffNectar();
            //BeeQueue.Enqueue(other.gameObject);
        }
    }
}
