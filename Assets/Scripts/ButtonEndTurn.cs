using Assets.Scripts;
using UnityEngine;

public class ButtonEndTurn : MonoBehaviour
{
    public CombatSceneController control=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void On_Click()
    {
        if (control.TurnState == -1)
        {
            control.TurnState++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
