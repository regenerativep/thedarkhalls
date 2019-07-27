using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// a tile that can be put into the game world
    /// </summary>
    public abstract class GameTile
    {
        /// <summary>
        /// position of the tile
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// sprite of the tile
        /// </summary>
        public SpriteData Sprite;
        /// <summary>
        /// room with the tile
        /// </summary>
        public Room Room;
        /// <summary>
        /// initializes the tile
        /// </summary>
        /// <param name="room">the room with the tile</param>
        /// <param name="position">the position of the tile</param>
        public GameTile(Room room, Vector2 position)
        {
            Room = room;
            Position = position;
            Sprite = new SpriteData();
        }
        /// <summary>
        /// updates the tile
        /// </summary>
        public void Update()
        {
            Sprite?.Update();
        }
        /// <summary>
        /// draws the tile
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="viewPosition">the view offset correpsonding to the view position</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Sprite?.Draw(spriteBatch, Position - viewPosition);
        }
        /// <summary>
        /// loads required assets for this tile
        /// </summary>
        /// <param name="assets">the asset manager to load assets from</param>
        public virtual void Load(AssetManager assets)
        {

        }
    }
}
