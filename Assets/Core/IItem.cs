using Assets.Scripts;

namespace Assets.Core
{
    public interface IItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public void Use (Entity entity);
    }
}
