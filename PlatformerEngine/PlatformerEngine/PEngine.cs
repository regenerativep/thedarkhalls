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
    /// manages all parts of the platformer engine
    /// </summary>
    public class PEngine
    {
        /// <summary>
        /// gets a game tile or object type from its corresponding string name
        /// </summary>
        public static Dictionary<string, Type> NameToType = new Dictionary<string, Type>();
        /// <summary>
        /// the current room
        /// </summary>
        public Room CurrentRoom;
        /// <summary>
        /// the asset manager
        /// </summary>
        public AssetManager Assets;
        /// <summary>
        /// the parent game
        /// </summary>
        public Game Game;
        /// <summary>
        /// creates a new platformer engine
        /// </summary>
        /// <param name="game">the parent game</param>
        public PEngine(Game game)
        {
            Game = game;
            Assets = new AssetManager(game.Content);
        }
        /// <summary>
        /// changes to a different room
        /// </summary>
        /// <param name="newRoom">the room to change to</param>
        /// <param name="trans">the transition to use</param>
        public void ChangeRoom(Room newRoom, ITransition trans = null)
        {
            if (trans != null)
            {
                Action roomComplete = () =>
                {
                    //CurrentRoom = newRoom;
                    ChangeRoom(newRoom);
                    CurrentRoom.ApplyTransition(trans.Perform(true));
                };
                if (CurrentRoom != null)
                {
                    CurrentRoom.ApplyTransition(trans.Perform(false, roomComplete));
                }
                else
                {
                    roomComplete();
                }
            }
            else
            {
                if (CurrentRoom == null)
                {
                    CurrentRoom = newRoom;
                }
                else
                {
                    for (int i = CurrentRoom.GameObjectList.Count - 1; i >= 0; i--)
                    {
                        GameObject obj = CurrentRoom.GameObjectList[i];
                        if (!obj.Persistent)
                        {
                            CurrentRoom.GameObjectList.RemoveAt(i);
                        }
                    }
                    CurrentRoom.GameObjectList.AddRange(newRoom.GameObjectList);
                    for(int i = 0; i < newRoom.GameObjectList.Count; i++)
                    {
                        GameObject obj = newRoom.GameObjectList[i];
                        obj.Room = CurrentRoom;
                        CurrentRoom.GameObjectList.Add(obj);
                    }
                    CurrentRoom.GameTileList.Clear();
                    CurrentRoom.GameTileList.AddRange(newRoom.GameTileList);
                    CurrentRoom.Width = newRoom.Width;
                    CurrentRoom.Height = newRoom.Height;
                    CurrentRoom.ViewPosition = newRoom.ViewPosition;
                    CurrentRoom.Physics = newRoom.Physics;
                }
            }
        }
        /// <summary>
        /// loads into a different room
        /// </summary>
        /// <param name="filePath">the file path to the room to change to</param>
        /// <param name="trans">the transition to use</param>
        public Room LoadRoom(string filePath, ITransition trans = null)
        {
            Room room = new Room(this);
            room.Load(filePath);
            ChangeRoom(room, trans);
            return room;
        }
        /// <summary>
        /// updates the engine
        /// </summary>
        public void Update()
        {
            CurrentRoom?.Update();
        }
        /// <summary>
        /// draws the engine
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentRoom?.Draw(spriteBatch);
        }
        /// <summary>
        /// gets a tile or object type given its corresponding name as a string
        /// </summary>
        /// <param name="name">name of the tile as a string</param>
        /// <returns>the corresonding tile object type</returns>
        public static Type GetTypeFromName(string name) //TODO: make this not static
        {
            if (NameToType.ContainsKey(name))
            {
                return NameToType[name];
            }
            return null;
        }
    }
}
