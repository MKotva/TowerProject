using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Populate area with CellObjs around the central position.
        float start_off_w = -CellWidth * (GridWidth - 1);
        float start_off_h = -CellHeight * (GridHeight - 1);
        CellArray=new GameObject[GridWidth, GridHeight];
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                CellArray[i,j] = Instantiate(CellObj, new Vector3(start_off_w+i*CellWidth, start_off_h+j*CellHeight,0), Quaternion.identity);
                //CellArray[i,j].transform.position -=gameObject.transform.position; This means we can now manipulate it as we wish
            }
        }

        //Spawn player and enemy sprites.

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
