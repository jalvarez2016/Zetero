using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public bool hasJumped;
    public Vector3 leftMovement;
    public Vector3 rightMovement;
    public Vector3 jumpForce;
    public GameObject person;
    public Animator player;

    void OnCollisionEnter2D(Collision2D other)
    {
        hasJumped = false;
        player.SetBool("up", false);
        if (other.gameObject.name.Contains("Enemy"))
        {
            Debug.Log("enemy hit you");
                hasJumped= true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        // Debug.Log("collided and hasJumped is " + hasJumped);
        Vector2 velocity = person.GetComponent<Rigidbody2D>().velocity;
        // Debug.Log("player y velocity is " + velocity.y);

        //left and right movement
        if(Input.GetAxisRaw("Horizontal") == -1 &&  player.GetFloat("Speed") >= -player.GetInteger("MaxSpeed")){
            // Debug.Log(person.GetComponent<Rigidbody2D>().velocity);
            GetComponent<Rigidbody2D>().AddForce(leftMovement);
            player.SetBool("right",true);
            GameObject.Find("Player").GetComponent<SpriteRenderer>().flipX = true;
            player.SetBool("idle", false);
            var newSpeed = velocity.x;
            player.SetFloat("Speed", newSpeed)  ;
        } else if(Input.GetAxisRaw("Horizontal") == 1 && player.GetFloat("Speed") <= player.GetInteger("MaxSpeed")){
            // Debug.Log(person.GetComponent<Rigidbody2D>().velocity);
            GetComponent<Rigidbody2D>().AddForce(rightMovement);
            player.SetBool("right", true);
            GameObject.Find("Player").GetComponent<SpriteRenderer>().flipX = false;
            player.SetBool("idle", false);
            var newSpeed = velocity.x;
            player.SetFloat("Speed", newSpeed);
        } else {
            player.SetBool("right", false);
            player.SetBool("idle", true);
        }

        //jumping and crouching 
        if(Input.GetAxisRaw("Vertical") == 1 && !hasJumped){
            hasJumped = true;
            GetComponent<Rigidbody2D>().AddForce(jumpForce);
            player.SetBool("up", true);
        } else if(Input.GetAxisRaw("Vertical") == -1){
            player.SetBool("down", true);
        } else if(Input.GetAxisRaw("Vertical") != -1){
            player.SetBool("down", false);
        }

        //correcting jumping error
        if(velocity.y == 0){
            hasJumped = false;
        }
        
    }
}
