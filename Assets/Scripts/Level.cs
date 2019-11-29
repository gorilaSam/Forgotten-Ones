﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public EnemySpawnArray spawnArray;
    public GameObject[] Enemy;
    private int LastIndexofArray;
    public float EnemySpeed = 10f;
    public float timeBetweenSpawns = 3f;
    private float yChangeMod = 0.06f;
    private float nextTimeToSpawn = 0f;
    private int keepTrackOfPlayLength = 0;
    private int x; // random Gen num for which state to do
    private int a; //random num Gen for State 2 shooting type's Movement Pattern
    private int numTimesXisZero = 0;
    

    void changeState(int i)
    {
        if (i == 0)
        {
            spawnArray.currentState = 0;
        }else if (i == 1)
        {
            spawnArray.currentState = EnemySpawnArray.states.reverseSweep;
        }
        else if (i == 2)
        {
            spawnArray.currentState = EnemySpawnArray.states.shortSweep;
        }
        else if (i == 3)
        {
            spawnArray.currentState = EnemySpawnArray.states.flippedShortSweep;
        }
        else if (i == 4)
        {
            spawnArray.currentState = EnemySpawnArray.states.v;
        }
        else if (i == 5)
        {
            spawnArray.currentState = EnemySpawnArray.states.upsideDownV;
        }
        else if (i == 6)
        {
            spawnArray.currentState = EnemySpawnArray.states.outsideTwo;
        }
        else if (i == 7)
        {
            spawnArray.currentState = EnemySpawnArray.states.center;
        }
        else if (i == 8)
        {
            spawnArray.currentState = EnemySpawnArray.states.staggeredTwoThree;
        }
        else if (i == 9)
        {
            spawnArray.currentState = EnemySpawnArray.states.twoAndTwo;
        }
        else if (i == 10)
        {
            spawnArray.currentState = EnemySpawnArray.states.three;
        }

    }

    void changeEnemyMovementType(int i)
    {
        if (i == 0)
        {
            spawnArray.currentBehaviour = EnemySpawnArray.state.straight;
        }else if (i == 1)
        {
            spawnArray.currentBehaviour = EnemySpawnArray.state.wavy;
        }
        else if (i == 2)
        {
            spawnArray.enemySpawner.speed = 6f;
            spawnArray.currentBehaviour = EnemySpawnArray.state.loop;
        }
    }


    IEnumerator MyCoroutine()
    {
        //prevent too many of one state type in a row
        if (numTimesXisZero >= 2)
        {
            x = 1;
        } else if (numTimesXisZero <= -1)
        {
            x = 0;
        }
        else
        {
            x = UnityEngine.Random.Range(0, 2);
        }
        
        if (x == 0)
        {
            //Random state 1   //non-shooting sweep-ish types
            spawnArray.Enemy = Enemy[UnityEngine.Random.Range(0, 2)];
            spawnArray.enemySpawner.canShoot = false;
            int n = UnityEngine.Random.Range(0, 6);
            changeState(n);
            if (n == 4 || n == 5)
            {
                changeEnemyMovementType(0);   //dont want v shape configurations to be anything but straight
            }
            else
            {
                changeEnemyMovementType(UnityEngine.Random.Range(0, 2));  //otherwise choose wavy or straight
            }
            spawnArray.ExecuteState();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        else if(x == 1)
        {
            //Random state 2  //These ones should shoot
            
            yield return new WaitForSeconds(1);
            spawnArray.Enemy = Enemy[UnityEngine.Random.Range(2, LastIndexofArray)];
            spawnArray.enemySpawner.canShoot = true;
            int tempA = a;
            while (tempA == a)
            {
                a = UnityEngine.Random.Range(6, 11);
            }
           
            changeState(a); 
            if (a == 7 || a == 8)
            {
                changeEnemyMovementType(0);   //center and twoThree are best straight
            }
            else
            {
                changeEnemyMovementType(UnityEngine.Random.Range(0, 3));  //otherwise choose any
            }
            spawnArray.enemySpawner.speed = spawnArray.enemySpawner.speed / 2;
            spawnArray.ExecuteState();          
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        LastIndexofArray = Enemy.Length;
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time >= nextTimeToSpawn)
        //nextTimeToSpawn = Time.time + spawnRate;
        
        if (Time.time >= nextTimeToSpawn)
        {
            if (keepTrackOfPlayLength == 10)   // a way to increase enemy speed through out time, this could be score based later
            {
                EnemySpeed += 2.5f;
                keepTrackOfPlayLength = 0;
            }
            if (numTimesXisZero >= 3 || numTimesXisZero <= -2)
            {
                numTimesXisZero = 0;
            }
            
            spawnArray.enemySpawner.speed = EnemySpeed;
            spawnArray.enemySpawner.yChange = EnemySpeed / 133f;
            spawnArray.enemySpawner.yChangeMod = EnemySpeed / 166f;
            StartCoroutine("MyCoroutine");
            nextTimeToSpawn = Time.time + timeBetweenSpawns;
            if (x == 0)
            {
                numTimesXisZero += 1;
            }else if (x == 1)
            {
                numTimesXisZero -= 1;
            }
            keepTrackOfPlayLength += 1;

        }

    }

}
