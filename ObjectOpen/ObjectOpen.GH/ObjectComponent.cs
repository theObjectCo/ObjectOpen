using Grasshopper.Kernel;

namespace ObjectOpen.GH
{
    public abstract class ObjectComponent : GH_Component
    {
        public ObjectComponent(
            string name,
            string nickname,
            string description,
            string subcategory)
          : base(
                name,
                nickname,
                description,
                "Object",
                subcategory)
        {
        }
    }
}