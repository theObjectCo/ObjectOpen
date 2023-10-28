using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace ObjectOpen.GH
{
    public class ObjectOpen_GHInfo : GH_AssemblyInfo
    {
        public override string Name => "ObjectOpen.GH";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => ObjectOpen.GH.Properties.Resources.icon_small_24x24;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";
        public override Guid Id => new Guid("9cd47bdf-440e-4055-99c9-35d919d3d8f0");

        //Return a string identifying you or your company.
        public override string AuthorName => "Object";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}