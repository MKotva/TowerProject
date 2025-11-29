using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

//Enum for actions where we need to store the state. Defend, Rest and Potions have immediate effects.
public enum SelectedAction
{
    None,
    Move,
    AttackPrimary,
    AttackSecondary
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
    public List<Entity> enemies;
    private GameObject[,] CellArray = null;

    public SelectedAction curSelectedAction = SelectedAction.None;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Populate area with CellObjs around the central position.
        float start_off_w =  (-CellWidth/2) * (GridWidth - 1);
        float start_off_h =  -3+(-CellHeight/2) * (GridHeight - 1);
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

    }
    //Method for grid buttons to send signal that they've been pressed.
    public void ReceiveClick(int w, int h)
    {
        //TODO: Logic of what happens
        Debug.Log(w + ", " + h);
        MoveGO(w, h, player);
    }
    public void MoveGO( int w, int h, GameObject go)
    {
        //go.transform.position=CellArray[w, h].transform.position;

        go.transform.DOMove(CellArray[w, h].transform.position, 1);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
