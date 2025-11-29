using Assets.Core;


namespace Assets.Scripts
{
    public class Armor : IItem
    {
        public string Name { get; set; }
        public int ProtectionPoints { get; set; }
        public int Value { get; set; }

        public void Use(Entity entity)
        {
        }
    }
}
