using Assets.Scripts;
using UnityEngine;

namespace Assets.Core
{
    public interface IItem
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Sprite Icon { get; set; }
        public void Use (Entity entity);
    }
}
