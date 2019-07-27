using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEditor
{
    public class WorldItemType
    {
        public string Name;
        public string InternalName;
        public Vector2 Size;
        public bool IsTile;
        public WorldItemType(string name, string internalName, Vector2 size, bool isTile)
        {
            Name = name;
            InternalName = internalName;
            Size = size;
            IsTile = isTile;
        }
    }
}
