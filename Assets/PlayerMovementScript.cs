using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementScript : MonoBehaviour
{
    public bool hasJumped;
    public bool holding;
    public Vector3 leftMovement;
    public Vector3 rightMovement;
    public Vector3 jumpForce;
    public GameObject person;
    public Animator player;
    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRigid;
    public AudioSource Attack;
    public AudioSource Hit;
    public AudioSource Complete;
    public AudioSource Pickup;
    public AudioSource Jump;

    public void AttackFX(){
        Attack.Play();
    }

    public void HitFX(){
        Hit.Play();
    }

    public void CompleteFX(){
        Complete.Play();
    }

    public void PickupFX(){
        Pickup.Play();
    }

    public void JumpFX(){
        Jump.Play();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        hasJumped = false;
        player.SetBool("up", false);
        if (other.gameObject.tag == "Enemy" && !player.GetBool("attack"))
        {
            Debug.Log("enemy hit you");
            hasJumped= true;
            HitFX();
        }
        if(player.GetBool("attack")){
            if (other.gameObject.tag == "Enemy")
            {
                // Debug.Log("Attacking enemy");
                Destroy(other.gameObject);
            } else if(other.gameObject.tag == "Button")
            {
                // Debug.Log("Button Hit!");
                CompleteFX();
                Debug.Log(other.gameObject.name);
                buttonPush(other.gameObject.name);
            }
        }
        
        player.SetBool("attack", false);
    }

    void OnCollisionExit2D(Collision2D other){        
        if(player.GetBool("attack")){
            if (other.gameObject.tag == "Enemy")
            {
                Destroy(other.gameObject);
                //damage enemy
            }
        }
    }

    public void buttonPush(string name){
        if(name == "StartBtn"){
            changeScene();
        } else if(name == "InfoBtn"){
            Debug.Log("Info Button pushed");
        } else if(name == "VolumeBtn"){
            Debug.Log("Volume button pushed");
        }
    }

    public void changeScene(){
        var scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + scene.buildIndex + "'.");
        // if(scene.buildIndex == 1){
        //     canvas.SetActive(false);
        //     video.GetComponent<VideoPlayer>.Play();
        // }
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {        
        // Debug.Log("collided and hasJumped is " + hasJumped);
        Vector2 velocity = person.GetComponent<Rigidbody2D>().velocity;
        // Debug.Log("player y velocity is " + velocity.y);

        //left and right movement
        if(Input.GetAxisRaw("Horizontal") == -1 &&  player.GetFloat("Speed") >= -player.GetInteger("MaxSpeed") && !holding && !player.GetBool("down")){
            // Debug.Log(person.GetComponent<Rigidbody2D>().velocity);
            GetComponent<Rigidbody2D>().AddForce(leftMovement);
            player.SetBool("right",true);
            GameObject.Find("Player").GetComponent<SpriteRenderer>().flipX = true;
            player.SetBool("idle", false);
            var newSpeed = velocity.x;
            player.SetFloat("Speed", newSpeed)  ;
        } else if(Input.GetAxisRaw("Horizontal") == 1 && player.GetFloat("Speed") <= player.GetInteger("MaxSpeed") && !holding && !player.GetBool("down")){
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
            Vector2 temp = new Vector2(0f,playerRigid.velocity.y);
            playerRigid.velocity = temp;
            
        }

        //jumping and crouching 
        if(Input.GetAxisRaw("Vertical") == 1 && !hasJumped){
            hasJumped = true;
            GetComponent<Rigidbody2D>().AddForce(jumpForce);
            player.SetBool("up", true);
            JumpFX();
        } else if(Input.GetAxisRaw("Vertical") == -1){
            player.SetBool("down", true);
            playerCollider.enabled = false;
        } else if(Input.GetAxisRaw("Vertical") != -1){
            player.SetBool("down", false);
            playerCollider.enabled = true;
        }

        //attacking
        if(Input.GetKeyDown(KeyCode.Space) && hasJumped){
            AttackFX();
            Debug.Log("attack");
            player.SetBool("attack", true);
        }

        //picking up and throwing
        if(Input.GetKeyDown(KeyCode.C)){
            //picking up logic for a nearby liftable block            
            player.SetBool("Pickup", true);
            holding = true;
        } else if(Input.GetKeyDown(KeyCode.X) && player.GetBool("Pickup")){
            //throwing the picked up box logic
            player.SetBool("Pickup", false);
            holding = false;
        };

        //correcting jumping error
        if(velocity.y == 0){
            hasJumped = false;
        }
        
    }
}
