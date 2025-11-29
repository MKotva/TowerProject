using Assets.Scripts;
using UnityEngine;

public class GridButton : MonoBehaviour
{
    public int w=0;
    public int h=0;
    public CombatSceneController control=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Init(int _w, int _h, GameObject _control)
    {
        w= _w;
        h= _h;
        control = _control.GetComponent<CombatSceneController>();
    }
    public void On_Click()
    {
        control.ReceiveClick(w,h);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
