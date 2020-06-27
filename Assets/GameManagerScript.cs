using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject start;
    public GameObject[] puzzles;
    public GameObject[] enemies;
    public GameObject end;
    public int stageLength;
    public Vector3 placement;
    // Start is called before the first frame update
    void Start()
    {
        placement = new Vector3(0f,0f,0f);
        Instantiate(start, placement, Quaternion.identity);
        for (int i = 0; i < stageLength; i++)
        {
            Debug.Log(i);
        //instantiate puzzle 
            int choise = Random.Range(0, puzzles.Length);
            GameObject puzzle = puzzles[choise];
            placement.x += 20f;
            Instantiate(puzzle, placement, Quaternion.identity);
        //instantiate enemy
            int enemyNum = Random.Range(0, enemies.Length);
            float enemyPlacement = Random.Range(3, 15);
            float enemyx = enemyPlacement;
            enemyx += placement.x;
            Vector3 enemyPos = new Vector3(enemyx, placement.y, placement.z);
            GameObject enemy = enemies[enemyNum];
            Instantiate(enemy, enemyPos, Quaternion.identity);
        }
        float endPlacement = placement.x + 20f;
        Instantiate(end, new Vector3(endPlacement, 0f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
