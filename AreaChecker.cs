/* AreaChecker.cs
*
* This class assists ObstacleSpawner.cs. It 
* is called by ObstacleSpawner.cs to 
* move the area checker gameobject to around
* the play area to find a suitable spawn location. 
*
*/ 

using UnityEngine;
using System.Collections;

public class AreaChecker : MonoBehaviour
{
    public bool isAvailable;
    public bool collision;

    private ObstacleSpawner ObsSpwnr;
    private GameManager GM;

    // Use this for initialization
    void Start() {
        ObsSpwnr = GameObject.Find("GameManager").GetComponent<ObstacleSpawner>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
		
    void OnTriggerEnter2D(Collider2D col) {
        isAvailable = true;
        collision = false;

        if (GM.canSpawn) {
            StartCoroutine(Wait());
        }
	
        if (col.gameObject.tag == "Obstacle") {
            isAvailable = false;
            collision = true;
        } else if (col.gameObject.tag == "Collider") {
            isAvailable = false;
            collision = true;
        } else if (col.gameObject.tag == "Paddle") {
            isAvailable = false;
            collision = true;
        } else if (col.gameObject.tag == "Ball") {
            //print("Nothing happened when ball spwn");
        } else {
            // print("shoulds spawn");
            collision = false;
            isAvailable = true;
            //freeze position
            ObsSpwnr.SpawnObstacle(this.transform.position);
        }
    }
		
    IEnumerator Wait() {
        if (gameObject.name == "lCollider") {
			if (isAvailable == true && collision == false) {
				ObsSpwnr.SpawnObstacle (this.transform.position);
				GM.canSpawn = false;
			} else {
				ObsSpwnr.MoveAreaChecker (GM.objIndex);
			}
        } else if(gameObject.name == "sCollider") {
            if (isAvailable == true && collision == false) {
                ObsSpwnr.SpawnObstacle(this.transform.position);
                GM.canSpawn = false;
            } else {
                ObsSpwnr.MoveAreaChecker(GM.objIndex);
            }
        }
        else if(gameObject.name == "ItemCube") {
            if (isAvailable == true && collision == false) {
                GM.itemCube.position = this.transform.position;
                GM.canSpawn = false;
            } else {
                ObsSpwnr.MoveAreaChecker(GM.objIndex);
            }
        }
        yield return new WaitForSeconds(.5f);
    }
}
