using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneFlock : MonoBehaviour
{
    public Drone dronePrefab;
    List<Drone> drones = new List<Drone>();
    public FlockBehavior behavior;



    [Range(10, 5000)]
    public int startingCount = 250;
    const float DroneDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    [Range(1f, 10f)]
    public float bulletRadius = 0.2f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            Drone newDrone = Instantiate(
                dronePrefab,
                UnityEngine.Random.insideUnitCircle * startingCount * DroneDensity,
                Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
                transform
                );
            newDrone.name = "Drone " + i;
            newDrone.Initialize(this);
            drones.Add(newDrone);
        }
    }

    void BubbleSort(Drone[] arr, int n) // O(N^2)
    {
        int i, j;
        Drone temp;
        bool swapped;               // let n =10
        for (i = 0; i < n - 1; i++)  // i=0..9
        {
            swapped = false;
            for (j = 0; j < n - i - 1; j++)   // i=0: j=0..9
                                              // i=1; j=0..8
                                              // i=2; j=0..7
                                              // i
            {
                if (arr[j].Temperature > arr[j + 1].Temperature) // check whether to swap
                {

                    // Swap arr[j] and arr[j+1]
                    temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }

            // If no two elements were
            // swapped by inner loop, then break
            //if (swapped == false)
            //    break;
        }
    }

    float AvgInterDroneDistance()
    {
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Drone drone in this.drones)
        {
            // decide on next movement direction
            List<Transform> context = GetNearbyObjects(drone);
            Vector2 move = drone.CalcMove(context, Vector2.zero, squareAvoidanceRadius, 10);  //behavior.CalculateMove(drone, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            drone.Move(move);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        Debug.Log("Found " + bullets.Length + " bullets");
        foreach (GameObject bullet in bullets)
        {
            Bullet thebullet = bullet.GetComponent<Bullet>();
            List<Transform> destroyed = GetHitByBullet(thebullet);

            foreach (Transform drone in destroyed)
            {
                GameObject go = drone.gameObject;
                Drone droneDestroyed = go.GetComponent<Drone>();
                droneDestroyed.Destroy();
            }
        }
        
        /*
        foreach (GameObject bullet in bullets)
        {
            Bullet thebullet = bullet.GetComponent<Bullet>();

            if (thebullet.IsGone == true)
            {
                Destroy(bullet);
            }
        }
        */
        
        List<Drone> whatremains = new List<Drone>();
        foreach (Drone drone in drones)
        {
            if (drone.Destroyed == true)
                Destroy(drone.gameObject);
            else
                whatremains.Add(drone);
        }
        drones = whatremains;
    }

    List<Transform> GetNearbyObjects(Drone drone)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(drone.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != drone.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }


    List<Transform> GetHitByBullet(Bullet bullet)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(bullet.transform.position, 
                                                                bulletRadius);

        Debug.Log("Number of nearby drones = "+contextColliders.Length);
        foreach (Collider2D c in contextColliders)
        {
            if (c.tag == bullet.tag)
                continue;

            context.Add(c.transform);

            GameObject gobj = c.transform.gameObject;
        }
        return context;
    }


}
