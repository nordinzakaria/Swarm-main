using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    List<Bullet> bullets = new List<Bullet>();

    public void Add(Bullet bullet)
    { bullets.Add(bullet); }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
