using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System;

//Enum for actions where we need to store the state. Defend, Rest and Potions have immediate effects.
public enum SelectedAction
{
    None,
    Move,
    Attack
}

public class CombatSceneController : MonoBehaviour
{
    public int GridHeight = 5;
    public int GridWidth = 15;
    public float CellHeight = 0.5f;
    public float CellWidth = 1f;
    public float ZPerHeight = 1;
    public Vector3 baseOffs = new Vector3(0, -3, 0);
    public GameObject CellObj;
    public Entity player;
    private Vector2Int playerPos;
    public List<Entity> enemies;
    private List<Tuple<Entity, Vector2Int>> enemiesPos;
    private GameObject[,] CellArray = null;

    public SelectedAction curSelectedAction = SelectedAction.None;
    private int TurnState=-1;
    private bool TurnDisabled=false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Populate area with CellObjs around the central position.
        float start_off_w =  (-CellWidth/2) * (GridWidth - 1);
        float start_off_h =  -2.5f+(-CellHeight/2) * (GridHeight - 1);
        CellArray=new GameObject[GridWidth, GridHeight];
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                var temp = Instantiate(CellObj, new Vector3(start_off_w+i*CellWidth, start_off_h+j*CellHeight,0), Quaternion.identity);
                //CellArray[i,j].transform.position -=gameObject.transform.position; This means we can now manipulate it as we wish

                CellArray[i, j]=temp.GetComponentInChildren<Button>().transform.gameObject;
                CellArray[i, j].transform.SetParent(transform);
                CellArray[i, j].GetComponent<GridButton>().Init(i, j, gameObject);
                Destroy(temp);
            }
        }

        //Spawn player and enemy sprites. TODO, IDK how we do this. For now I'm linking player as an object. I can manipulate

        //Player always spawns at (0,2), move him there.
        player.gameObject.transform.position = CellArray[0, 2].transform.position;
        playerPos = new Vector2Int(0, 2);
        //we start the enemy positioning from (15,5)
        enemiesPos=new List<Tuple<Entity, Vector2Int>>();
        for (int i=0; i<enemies.Count; i++)
        {
            int h = GridHeight-1 - i % 5;
            int w = GridWidth - 1 - h / 5;

            enemiesPos.Add(new Tuple<Entity, Vector2Int>(enemies[i], new Vector2Int(w, h)));
            enemiesPos[i].Item1.transform.position = CellArray[w,h].transform.position;
        }

    }
    //Method for grid buttons to send signal that they've been pressed.
    public void ReceiveClick(int w, int h)
    {
        //TODO: Logic of what happens
        Debug.Log(w + ", " + h);
        switch (curSelectedAction)
        {
            case SelectedAction.Move:
                MoveGO(w, h, player.gameObject);
                playerPos = new Vector2Int(w, h);
                TurnState++;
                break;
            case SelectedAction.Attack:
                break;
            default:
                break;
        }
        curSelectedAction = SelectedAction.None;       
    }
    public void MoveGO(int w, int h, GameObject go)
    {
        //go.transform.position=CellArray[w, h].transform.position;
        go.transform.DOMove(CellArray[w, h].transform.position, 1);
    }
    // Update is called once per frame
    void Update()
    {
        if (!TurnDisabled)
        {
            if (TurnState == -1)
            {
                //player to move
            }
            else
            {
                bool endedTurn = getEnemyAction(TurnState);
                //TODO: This should move the enemy,. in future also lock controls
                if (endedTurn)
                {
                    TurnState++;
                }
            }
        }
        if (TurnState >= enemies.Count)
        {
            TurnState = -1;
        }
    }
    private bool getEnemyAction(int idx)
    {
        Entity enemy= enemiesPos[idx].Item1;
        Vector2Int curEnemyPos= enemiesPos[idx].Item2;
        Vector2Int resultPos=curEnemyPos;
        const int max_move= 3;
        if (Math.Abs(curEnemyPos.x - playerPos.x) + Math.Abs(curEnemyPos.y - playerPos.y) > 1)
        {
            //move
            if (Math.Abs(curEnemyPos.x - playerPos.x) > 3)
            {
                if (curEnemyPos.x - playerPos.x > 0)
                {
                    resultPos = new Vector2Int(curEnemyPos.x - 3, curEnemyPos.y);
                    //TODO: Update position
                }
                else
                {
                    resultPos = new Vector2Int(curEnemyPos.x + 3, curEnemyPos.y);
                }
            }
            else if (Math.Abs(curEnemyPos.y - playerPos.y) > 3)
            {
                if (curEnemyPos.y - playerPos.y > 0)
                {
                    resultPos = new Vector2Int(curEnemyPos.x, curEnemyPos.y - 3);
                }
                else
                {
                    resultPos = new Vector2Int(curEnemyPos.x, curEnemyPos.y + 3);
                }
            }
            else
            {
                var xMove = curEnemyPos.x - playerPos.x;
                var xAbs = Mathf.Abs(xMove);
                var yMove = Mathf.Min(max_move - xAbs, Mathf.Abs(curEnemyPos.y-playerPos.y));
                if (curEnemyPos.y < playerPos.y)
                {
                    yMove *= -1;
                }
                resultPos = new Vector2Int(curEnemyPos.x - (curEnemyPos.x - playerPos.x), curEnemyPos.y-yMove);                
            }
            resultPos = new Vector2Int(Math.Clamp(resultPos.x, 0,GridWidth-1), Math.Clamp(resultPos.y,0,GridHeight-1));
            MoveGO(resultPos.x, resultPos.y, enemy.gameObject);//Bug
            enemiesPos[idx] = new Tuple<Entity, Vector2Int>(enemy,resultPos);
        }
        //We should select and execute enemy action here
        return true;
    }
    public void OnTimeout()
    {
        TurnDisabled = false;
    }
}
