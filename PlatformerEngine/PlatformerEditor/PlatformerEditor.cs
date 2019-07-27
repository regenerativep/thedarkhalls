using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using PlatformerEngine;
using PlatformerEngine.UserInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlatformerEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PlatformerEditor : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public AssetManager Assets;
        public Dictionary<int, WorldLayer> WorldLayers;
        public WorldLayerListElement WorldLayerListElement;
        public WorldItemListElement ObjectListElement;
        public WorldItemListElement TileListElement;
        public WorldItemType CurrentWorldItemType;
        public WorldLayer CurrentWorldLayer;
        public UIManager UIManager;
        public PlatformerEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += (object s, PreparingDeviceSettingsEventArgs args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            Content.RootDirectory = "Content";
            Assets = new AssetManager(null);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            CurrentWorldItemType = null;
            CurrentWorldLayer = null;
            WorldLayers = new Dictionary<int, WorldLayer>();
            UIManager = new UIManager(this, Assets);
            //TabbedElement mainTabs = new TabbedElement(UIManager, new Vector2(0, 0), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), 0.4f, "tabs_main", 24);
            //mainTabs.AddTab("level", new LevelTab(UIManager, new Vector2(0, 0), new Vector2(mainTabs.CurrentTabContainer.Size.X, mainTabs.CurrentTabContainer.Size.Y), 0.4f, "tab_level"));
            UIManager.TopUINode.Elements.Add(new LevelTab(UIManager, new Vector2(0, 0), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), 0.4f, "tab_level"));

            LoadWorldItemTypes("types.json");

            base.Initialize();
        }
        public void AddWorldLayer(int layer)
        {
            AddWorldLayer(new WorldLayer(layer));
        }
        public void AddWorldLayer(WorldLayer worldLayer)
        {
            if(WorldLayers.ContainsKey(worldLayer.Layer))
            {
                System.Diagnostics.Debug.WriteLine("tried to add a layer that already exists");
                return;
            }
            WorldLayerListElement.AddLayer(worldLayer.Layer);
            WorldLayers[worldLayer.Layer] = worldLayer;
        }
        public WorldLayer GetWorldLayer(int layer)
        {
            if(WorldLayers.ContainsKey(layer))
            {
                return WorldLayers[layer];
            }
            return null;
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Content = Content;
            Assets.LoadFont("main", "mainFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UIManager.Update();
            base.Update(gameTime);
        }
        public void LoadWorldItemTypes(string filename)
        {
            string json = File.ReadAllText(filename, Encoding.UTF8);
            JObject obj = JObject.Parse(json);
            JArray objectTypeArray = (JArray)obj.GetValue("objectTypes").ToObject(typeof(JArray));
            foreach(JToken assetToken in objectTypeArray)
            {
                JObject assetObject = (JObject)assetToken.ToObject(typeof(JObject));
                string name = (string)assetObject.GetValue("name").ToObject(typeof(string));
                string internalName = (string)assetObject.GetValue("internalName").ToObject(typeof(string));
                int width = (int)assetObject.GetValue("width").ToObject(typeof(int));
                int height = (int)assetObject.GetValue("height").ToObject(typeof(int));
                ObjectListElement.AddWorldItem(new WorldItemType(name, internalName, new Vector2(width, height), false));
            }
            JArray tileTypeArray = (JArray)obj.GetValue("tileTypes").ToObject(typeof(JArray));
            foreach (JToken assetToken in tileTypeArray)
            {
                JObject assetObject = (JObject)assetToken.ToObject(typeof(JObject));
                string name = (string)assetObject.GetValue("name").ToObject(typeof(string));
                string internalName = (string)assetObject.GetValue("internalName").ToObject(typeof(string));
                int width = (int)assetObject.GetValue("width").ToObject(typeof(int));
                int height = (int)assetObject.GetValue("height").ToObject(typeof(int));
                TileListElement.AddWorldItem(new WorldItemType(name, internalName, new Vector2(width, height), true));
            }
        }
        public void LoadLevel(string filename)
        {
            string json;
            try
            {
                json = File.ReadAllText(filename, Encoding.UTF8);
            }
            catch
            {
                return;
            }
            JObject levelObject = JObject.Parse(json);
            WorldLayers.Clear();
            WorldLayerListElement.ClearLayers();
            CurrentWorldLayer = null;
            LevelElement levelElement = (LevelElement)UIManager.GetUIElement("level");
            int levelWidth = (int)levelObject.GetValue("width").ToObject(typeof(int));
            int levelHeight = (int)levelObject.GetValue("height").ToObject(typeof(int));
            levelElement.LevelSize = new Vector2(levelWidth, levelHeight);
            if (levelObject.ContainsKey("physics"))
            {
                JObject physicsObject = (JObject)levelObject.GetValue("physics").ToObject(typeof(JObject));
                float gravityX = (float)physicsObject.GetValue("gravityx").ToObject(typeof(float));
                float gravityY = (float)physicsObject.GetValue("gravityy").ToObject(typeof(float));
                levelElement.Gravity = new Vector2(gravityX, gravityY);
            }
            JArray layerArray = (JArray)levelObject.GetValue("layers").ToObject(typeof(JArray));
            foreach(JToken token in layerArray)
            {
                JObject layerObject = (JObject)token.ToObject(typeof(JObject));
                int layer = (int)layerObject.GetValue("layer").ToObject(typeof(int));
                WorldLayer worldLayer = new WorldLayer(layer);
                JArray objectArray = (JArray)layerObject.GetValue("objects").ToObject(typeof(JArray));
                JArray tileArray = (JArray)layerObject.GetValue("tiles").ToObject(typeof(JArray));
                List<WorldItem> objectList = LoadWorldItemsFromJArray(objectArray, worldLayer.DrawLayer);
                List<WorldItem> tileList = LoadWorldItemsFromJArray(tileArray, worldLayer.DrawLayer);
                worldLayer.WorldItems.AddRange(objectList);
                worldLayer.WorldItems.AddRange(tileList);
                AddWorldLayer(worldLayer);
            }
        }
        public List<WorldItem> LoadWorldItemsFromJArray(JArray list, float drawLayer)
        {
            List<WorldItem> worldItemList = new List<WorldItem>();
            foreach(JToken token in list)
            {
                JObject itemObject = (JObject)token.ToObject(typeof(JObject));
                int x = (int)itemObject.GetValue("x").ToObject(typeof(int));
                int y = (int)itemObject.GetValue("y").ToObject(typeof(int));
                string internalName = (string)itemObject.GetValue("name").ToObject(typeof(string));
                WorldItem item = new WorldItem(UIManager, GetWorldItemTypeFromName(internalName), new Vector2(x, y), drawLayer);
                worldItemList.Add(item);
            }
            return worldItemList;
        }
        public WorldItemType GetWorldItemTypeFromName(string internalName)
        {
            WorldItemType type = ObjectListElement.GetWorldItemTypeFromName(internalName);
            if(type != null)
            {
                return type;
            }
            type = TileListElement.GetWorldItemTypeFromName(internalName);
            return type;
        }
        public void SaveLevel(string filename)
        {
            LevelElement levelElement = (LevelElement)UIManager.GetUIElement("level");
            JObject levelObject = new JObject();
            levelObject.Add("width", JToken.FromObject((int)levelElement.LevelSize.X));
            levelObject.Add("height", JToken.FromObject((int)levelElement.LevelSize.Y));
            if(levelElement.Gravity.X != 0 || levelElement.Gravity.Y != 0)
            {
                JObject physicsObject = new JObject();
                physicsObject.Add("gravityx", JToken.FromObject(levelElement.Gravity.X));
                physicsObject.Add("gravityy", JToken.FromObject(levelElement.Gravity.Y));
                levelObject.Add("physics", JToken.FromObject(physicsObject));
            }
            JArray layerArray = new JArray();
            foreach(KeyValuePair<int, WorldLayer> pair in WorldLayers)
            {
                WorldLayer worldLayer = pair.Value;
                JObject layerObject = new JObject();
                layerObject.Add("layer", JToken.FromObject(worldLayer.Layer));
                JArray objectArray = new JArray();
                JArray tileArray = new JArray();
                for(int j = 0; j < worldLayer.WorldItems.Count; j++)
                {
                    WorldItem item = worldLayer.WorldItems[j];
                    JObject itemObject = new JObject();
                    itemObject.Add("name", JToken.FromObject(item.ItemType.InternalName));
                    itemObject.Add("x", JToken.FromObject((int)item.Position.X));
                    itemObject.Add("y", JToken.FromObject((int)item.Position.Y));
                    if(item.ItemType.IsTile)
                    {
                        tileArray.Add(JToken.FromObject(itemObject));
                    }
                    else
                    {
                        objectArray.Add(JToken.FromObject(itemObject));
                    }
                }
                layerObject.Add("objects", JToken.FromObject(objectArray));
                layerObject.Add("tiles", JToken.FromObject(tileArray));
                layerArray.Add(JToken.FromObject(layerObject));
            }
            levelObject.Add("layers", JToken.FromObject(layerArray));
            string json = levelObject.ToString();
            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            UIManager.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
