namespace Assets.Scripts.RoomControllerScripts
{
	public class EmptyRoom: RoomBase
	{
        void Update()
        {
            if (base.controller != null)
                FinishRoom();
        }
    }
}