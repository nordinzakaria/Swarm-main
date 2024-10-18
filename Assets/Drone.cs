using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Drone : MonoBehaviour
{
    public int Code {  get;  set; }
    public int Temperature { set; get; } = 0;
    public bool Destroyed { get; set; }


    DroneFlock agentFlock;
    public DroneFlock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        Code = Random.Range(1000, 9999);
    }

    public void Destroy()
    {
        Destroyed = true;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        Temperature = (int) (Random.value * 100);
    }

    public void Initialize(DroneFlock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
