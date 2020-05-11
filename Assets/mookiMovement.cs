using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mookiMovement : MonoBehaviour
{
    public GameObject Mooki;
    public Vector2 walkForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = Mooki.GetComponent<Rigidbody2D>().velocity;
        if(velocity.x == 0){
            walkForce = -walkForce;
            GetComponent<Rigidbody2D>().AddForce(walkForce);
            if(GetComponent<SpriteRenderer>().flipX){
                GetComponent<SpriteRenderer>().flipX = false;
            } else {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        } else {
            GetComponent<Rigidbody2D>().AddForce(walkForce);
        }
        
    }
}
