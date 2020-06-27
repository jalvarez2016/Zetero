using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleManagerScript : MonoBehaviour
{
    public GameObject blocker;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Liftable")
        {
            Destroy(blocker);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
