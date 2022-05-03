using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class Component
    {
        public Component Next;

        public virtual void Update(GameTime gametime) => Next?.Update(gametime);
        public virtual void UpdateData(GameTime gametime) => Next?.UpdateData(gametime);

        public void Add(Component component)
        {
            if (Next == null)
                Next = component;
            else
                Next.Add(component);
        }
    }
}