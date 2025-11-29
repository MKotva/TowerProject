using Assets.Core;


namespace Assets.Scripts
{
    public class Weapon : IItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int DamagePower { get; set; }
        public bool Ranged { get; set; }

        public void Use(Entity entity)
        {
        }
    }
}
