using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class WorldLayer
    {
        public List<WorldItem> WorldItems;
        public int Layer;
        public float DrawLayer;
        public bool IsVisible;
        public WorldLayer(int layer)
        {
            Layer = layer;
            IsVisible = true;
            WorldItems = new List<WorldItem>();
            DrawLayer = layer / 1000f; //warning: any layer above 1000 is probably going to cause problems
        }
    }
}
