using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mookiMovement : MonoBehaviour
{
    public GameObject Mooki;
    public GameObject player;
    public Vector2 walkForce;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f) 
        {
            
            Vector2 velocity = Mooki.GetComponent<Rigidbody2D>().velocity;
            Vector3 playerPos = GameObject.Find("Player").transform.position;
            Vector3 enemyPos = Mooki.transform.position;
            Vector2 newWalk = walkForce;
            if(playerPos.x > enemyPos.x)
            {
                newWalk = -walkForce;
                if(GetComponent<SpriteRenderer>().flipX){
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }                
            if(velocity.x == 0){
                GetComponent<Rigidbody2D>().AddForce(newWalk);
                GetComponent<SpriteRenderer>().flipX = true;
            }

            timer = 2f;    
        } else {
            return;
        }
        
    }
}
