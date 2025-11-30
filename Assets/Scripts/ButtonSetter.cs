using UnityEngine;

public class ButtonSetter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CombatSceneController control;
    public SelectedAction actionToDo;
    void Start()
    {
        
    }
    public void On_Click()
    {
        control.curSelectedAction = actionToDo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
