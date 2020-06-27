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
    public Vector3 thowForce;
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
    public GameObject carryingObj;
    public SpriteRenderer sprite;
    public float health;
    public Color hurtColor;

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

    void OnCollisionStay(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        hasJumped = false;
        player.SetBool("up", false);
        if (other.gameObject.tag == "Enemy" && !player.GetBool("attack"))
        {
            hasJumped= true;
            health -= 1;
            if(health == 2f)
            {
                sprite.color = hurtColor;
            } else if(health == 1f){
                sprite.color = new Color(255f, 0f, 0f,  255f);
            } else {
                //reload scene                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
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

    //pickup logic
    void OnTriggerStay2D(Collider2D other)
    {
        // other.attachedRigidbody.AddForce(-0.1F * other.attachedRigidbody.velocity);        
        if(player.GetBool("Pickup") && player.GetBool("Carrying")){
            PickupBox(other.gameObject);
        } else if(player.GetBool("Pickup") && !player.GetBool("Carrying")){
            return;
        }
    }

    void PickupBox(GameObject other){
        
            Debug.Log(other.gameObject.tag);
            if(other.gameObject.tag == "Liftable"){
                Debug.Log("pickn up boxes");
                // other.transform.position = transform.position;
                // Vector3 box = other.transform.position;
                // box.y += 1.1f;
                // other.transform.position = box;
                Vector2 pickupForce = new Vector2(0f, 750f);
                player.SetBool("Carrying", false);
                other.GetComponent<Rigidbody2D>().gravityScale = 0f;
                other.GetComponent<Rigidbody2D>().AddForce(pickupForce);
                // other.rigidbody2D.gravityScale = 0.0f;
                carryingObj = other.gameObject;
            }
    }
        
    void ThrowBox(){
        player.SetBool("Pickup", false);
        holding = false;
        if(GameObject.Find("Player").GetComponent<SpriteRenderer>().flipX){
            carryingObj.GetComponent<Rigidbody2D>().AddForce(-thowForce);
        } else {
            thowForce = new Vector3(10000f,1000f,0f);
            carryingObj.GetComponent<Rigidbody2D>().AddForce(thowForce);
        }
        carryingObj.GetComponent<Rigidbody2D>().gravityScale = 10f;
    }

    // void OnCollisionExit2D(Collision2D other){        
    //     if(player.GetBool("attack")){
    //         if (other.gameObject.tag == "Enemy")
    //         {
    //             Destroy(other.gameObject);
    //             //damage enemy
    //         }
    //     }
    // }

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

        //reset
        if(Input.GetKeyDown(KeyCode.R)){
            //reload scene                
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);            
        }

        //left and right movement
        if(Input.GetAxisRaw("Horizontal") == -1 &&  player.GetFloat("Speed") >= -player.GetInteger("MaxSpeed") && !player.GetBool("down")){
            // Debug.Log(person.GetComponent<Rigidbody2D>().velocity);
            GetComponent<Rigidbody2D>().AddForce(leftMovement);
            player.SetBool("right",true);
            GameObject.Find("Player").GetComponent<SpriteRenderer>().flipX = true;
            player.SetBool("idle", false);
            var newSpeed = velocity.x;
            player.SetFloat("Speed", newSpeed)  ;
        } else if(Input.GetAxisRaw("Horizontal") == 1 && player.GetFloat("Speed") <= player.GetInteger("MaxSpeed") && !player.GetBool("down")){
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
            player.SetBool("Carrying", true);
            ThrowBox();
        };

        //correcting jumping error
        if(velocity.y == 0){
            hasJumped = false;
        }
        
    }
}
