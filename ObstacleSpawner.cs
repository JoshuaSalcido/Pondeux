/* ObstacleSpawner.cs
*
* This class assists GameManager.cs. It 
* is canlled by GameManager.cs to 
* spawm
*
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObstacleSpawner : MonoBehaviour {

    public float spawnTime;

    public bool canMoveAreaChkr = false; 

    float xLoc;
    float yLoc;

    public AreaChecker sCollider;
    public AreaChecker lCollider;

    GameManager GM;

    public bool tier1, tier2, tier3, tier4, tier5;

    public List<float> randFloatList = new List<float>(); 

    // Use this for initialization
    void Awake () {
        sCollider = GameObject.Find("sCollider").GetComponent<AreaChecker>();
        lCollider = GameObject.Find("lCollider").GetComponent<AreaChecker>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ObjectGenerator() {
        float temp = Random.Range(0, 1f);
        randFloatList.Add(temp);

        if (GM.currentGameActive) {
            for (int index = 0; index < randFloatList.Count; index++) {

                //SPAWN TIER 1 (static and basic objects)
                // % OF TRIGGERING SUPER-OBSTACLE
                // % OF TRIGGERING POWERUP
                // % OF TRIGGERING EVENT (AUDIO OR VISUAL)
                // ONLY TWO BALLS CAN BE ACTIVE, SPAWN LESS FREQUENTLY

                if (GM.spawnLevel < 75) {
                    tier1 = true;

                    if (randFloatList[index] < 0.25) {
						//OBJECT IS SELECTED FROM OBSTACLE LIST
                        GM.objIndex = 0;//IF  I WERE TO USE OBJECT POOLING, I WOULD DO IT HERE
                        MoveAreaChecker(GM.objIndex);
                    }
                    if (randFloatList[index] < 0.5) {
                        //OBJECT IS SELECTED FROM OBSTACLE LIST
                        GM.objIndex = 1;//IF  I WERE TO USE OBJECT POOLING, I WOULD DO IT HERE
                        MoveAreaChecker(GM.objIndex);
                    } else if (randFloatList[index] < 0.75) {
                        //SPAWN TIER 1 OBJECT
                        GM.objIndex = 5;
                        MoveAreaChecker(GM.objIndex);
                    } else if (randFloatList[index] < 0.95) {
                        //SPAWN TIER 1 OBJECT
                        GM.objIndex = 6;
                        MoveAreaChecker(GM.objIndex);
                    } else if ((randFloatList[index] > .9f && randFloatList[index] < 0.95) && GM.itemCube.GetComponent<ItemCube>().isActive != true) {
                        //SPAWN ITEMCUBE
                        if (GM.itemCube.GetComponent<ItemCube>().canSpawn) {
                            GM.itemCube.GetComponent<ItemCube>().isActive = true;
                            GM.itemCube.position = GenerateRandomLocation();
                            GM.itemCube.GetComponent<ItemCube>().Move();
                        }
                    } else {
                        //SPAWN TIER 1 OBJECT
                        GM.objIndex = Random.Range(0, 7);
                        MoveAreaChecker(GM.objIndex);
                    }
                }

                //SPAWN TIER 2 (SAME OBSTACLES AS TIER 1, BUT NOW MOVABLE)
                // % OF TRIGGERING SUPER-OBSTACLE
                // % OF TRIGGERING POWERUP
                // % OF TRIGGERING EVENT (AUDIO OR VISUAL)
                // ONLY TWO BALLS CAN BE ACTIVE, SPAWN LESS FREQUENTLY
                else if (GM.spawnLevel < 350/*2000*/) {
                    tier1 = false;
                    tier2 = true;
                    //float temp = Random.Range(0, 1f);
                    //print("temp: " + temp);

                 	if (temp < 0.1 || temp > .9) {
                        //OBJECT IS SELECTED FROM OBSTACLE LIST
                        GM.objIndex = Random.Range(0, 1);
                        MoveAreaChecker(GM.objIndex);
                    } else if (temp > .4f && temp < 0.45 && GM.itemCube.GetComponent<ItemCube>().isActive != true) {
                        //SPAWN ITEMCUBE
                        if (GM.itemCube.GetComponent<ItemCube>().canSpawn) {
                            GM.itemCube.GetComponent<ItemCube>().isActive = true;
                            GM.itemCube.position = GenerateRandomLocation();
                            GM.itemCube.GetComponent<ItemCube>().Move();
                        }
                    } else if (temp < 0.45 && temp > .6f) {
                        //SPAWN TIER 2 OBJECT
                        GM.objIndex = Random.Range(2, 4);
                        MoveAreaChecker(GM.objIndex);
                    } else if ((temp > .55 && temp < 0.6) && GM.ballCount < 2) {
                        //SPAWN Ball
                        GM.SpawnBall();
                    } else {
                        //SPAWN TIER 1 OBJECT
                        GM.objIndex = Random.Range(0, 4);
                        MoveAreaChecker(GM.objIndex);
                    }
                }

                //SPAWN TIER 3 (SAME OBSTACLES AS TIER 2, BUT NOW SPAWN IN PATTERNS/GROUPS)
                // % OF TRIGGERING SUPER-OBSTACLE
                // % OF TRIGGERING POWERUP
                // % OF TRIGGERING EVENT (AUDIO OR VISUAL)
                // ONLY THREE BALLS CAN BE ACTIVE, SPAWN MORE FREQUENTLY
                // 15% SPEED INCREASE ON SPAWNED OBSTACLES
                else if (GM.spawnLevel < 600/*4000*/) {
                    //float temp = Random.Range(0, 1f);
                    //print("temp: " + temp);

                    tier2 = false;
                    tier3 = true;

                    if (temp < 0.1 || temp > .9) {
                        //OBJECT IS SELECTED FROM OBSTACLE LIST
                        GM.objIndex = Random.Range(0,8);
                        MoveAreaChecker(GM.objIndex);
                    }
                    //SPAWN ITEMCUBE
                    if (GM.itemCube.GetComponent<ItemCube>().canSpawn) {
                        //SPAWN ITEMCUBE
                        if (GM.itemCube.GetComponent<ItemCube>().canSpawn) {
                            GM.itemCube.GetComponent<ItemCube>().isActive = true;
                            GM.itemCube.position = GenerateRandomLocation();
                            GM.itemCube.GetComponent<ItemCube>().Move();
                        }
                    }
                    else if (temp < 0.45 || temp > .6f) {
                        //SPAWN TIER 3 OBJECT
                        GM.objIndex = Random.Range(2, 4);
                        MoveAreaChecker(GM.objIndex);
                    } else if ((temp > .55 && temp < 0.6) && GM.ballCount < 3) {
                        //SPAWN BALL
                        GM.SpawnBall();
                    } else {
                        GM.objIndex = Random.Range(0, 8);
                        MoveAreaChecker(GM.objIndex);
                    }
                }

                //SPAWN TIER 4 (SAME OBSTACLES AS TIER 3)
                // % OF TRIGGERING SUPER-OBSTACLE
                // % OF TRIGGERING POWERUP
                // % OF TRIGGERING EVENT (AUDIO OR VISUAL)
                // ONLY TWO BALLS CAN BE ACTIVE, SPAWN MORE FREQUENTLY
                // 25% SPEED INCREASE ON SPAWNED OBSTACLES
                else if (GM.spawnLevel < 850/*6000*/) {
                    //float temp = Random.Range(0, 1f);

                    //print("In Tier 4");
                    tier3 = false;
                    tier4 = true;
                    if (temp < 0.1 || temp > .9) {
                        //OBJECT IS SELECTED FROM OBSTACLE LIST
                        GM.objIndex = Random.Range(0, 2);
                        MoveAreaChecker(GM.objIndex);
                    } else if (temp < 0.45 || temp > .6f) {
                        //SPAWN TIER 1 OBJECT
                        GM.objIndex = Random.Range(3, 4);
                        MoveAreaChecker(GM.objIndex);
                    } else if ((temp > .55 && temp < 0.6) && GM.ballCount < 3) {
                        //SPAWN BALL
                        GM.SpawnBall();
                    } else {
                        GM.objIndex = Random.Range(0, 4);
                        MoveAreaChecker(GM.objIndex);
                    }
                }

                //SPAWN TIER 5 (SAME OBSTACLES AS TIER 4)
                // % OF TRIGGERING SUPER-OBSTACLE
                // % OF TRIGGERING POWERUP
                // % OF TRIGGERING EVENT (AUDIO OR VISUAL)
                // 25% SPEED INCREASE ON SPAWNED OBSTACLES
                else if (GM.spawnLevel < 150000) {
                    //float temp = Random.Range(0, 1f);
                    tier4 = false;
                    tier5 = true;

                    if (temp < 0.1 || temp > .9) {
                        GM.objIndex = Random.Range(0, 2);
                        MoveAreaChecker(GM.objIndex);
                    } else if (temp < 0.45 && temp > .6f) {
                        //SPAWN TIER 1 OBJECT
                        GM.objIndex = Random.Range(3, 4);
                        MoveAreaChecker(GM.objIndex);
                    } else if ((temp > .55 && temp < 0.6) && GM.ballCount < 3) {
                        //SPAWN TIER BALL
                        GM.SpawnBall();
                    } else {
                        GM.objIndex = Random.Range(0, 4);
                        MoveAreaChecker(GM.objIndex);
                    }
                }
            }
        }
    }

    public Vector2 GenerateRandomLocation() {
		xLoc = Random.Range(-6, 6);
        yLoc = Random.Range(-4, 4);
	
        //check if x and y coordinates are not outside of play area
        if (xLoc >= 6 || xLoc <= -6) {
            xLoc = Random.Range(-6, 6);
        }

        if (yLoc >= 4 || yLoc <= -4) {
            yLoc = Random.Range(-4, 4);
        }

        //Set new location
        Vector2 obstacleSpawnLocation = new Vector2(xLoc, yLoc);

        return obstacleSpawnLocation;
    }

    public int Tier() {
        if (tier1) {
            return 1;
        } else if (tier2) {
            return 2;
        } else if (tier3) {
            return 3;
        } else if (tier4) {
            return 4;
        } else if (tier5) {
            return 5;
        }
        return 0;
    }

    public void MoveAreaChecker(int objSelected) {

        //Move collider to check if anything is there
        // Check whether there's another obstacle at this location already 
        if (GM.obstacles[objSelected].GetComponent<Obstacles>().isLarge == true) {

                lCollider.gameObject.GetComponent<Transform>().position = GenerateRandomLocation();
                //print("moved large collider");
         } else {
                sCollider.gameObject.GetComponent<Transform>().position = GenerateRandomLocation();
                //print("moved small collider");
         }
    }

 
    public void SpawnObstacle(Vector2 spawnLocation) {
        if (lCollider.isAvailable == true && lCollider.collision == false) {
            //print("Should spawn from lCollider...");
            
            Instantiate(GM.obstacles[GM.objIndex], spawnLocation, transform.rotation);
            lCollider.isAvailable = false;
            lCollider.gameObject.GetComponent<Transform>().position = new Vector3(-12, 12, 0);
        } else if (sCollider.isAvailable == true && sCollider.collision == false) {
            //print("Should spawn from sCollider...");
            
            Instantiate(GM.obstacles[GM.objIndex], spawnLocation, transform.rotation);
            sCollider.isAvailable = false;
            lCollider.gameObject.GetComponent<Transform>().position = new Vector3(12, -12, 0);
        }
    }      
}
