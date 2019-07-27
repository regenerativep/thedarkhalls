using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// wrapper for easily separating layers of drawing
    /// </summary>
    public class LayerData
    {
        /// <summary>
        /// the layer (0 -> 100) to draw at
        /// </summary>
        public int Layer
        {
            get
            {
                return wholeLayer;
            }
            set
            {
                wholeLayer = value;
                ActualLayer = value / 100f;
            }
        }
        private int wholeLayer;
        public float ActualLayer { get; private set; }
        /// <summary>
        /// creates a new draw layer
        /// </summary>
        /// <param name="layer">the layer</param>
        public LayerData(int layer)
        {
            Layer = layer;
        }
        /// <summary>
        /// gets a whole layer at 100
        /// </summary>
        public static LayerData Default
        {
            get
            {
                return new LayerData(100);
            }
        }
    }
}
