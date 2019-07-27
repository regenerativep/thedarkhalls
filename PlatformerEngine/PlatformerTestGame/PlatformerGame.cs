using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using PlatformerEngine.Physics;
using PlatformerTestGame.GameObjects;
using System;

namespace PlatformerTestGame
{
    /// <summary>
    /// main area for the platformer game
    /// </summary>
    public class PlatformerGame : Game
    {
        /// <summary>
        /// game's graphics device manager
        /// </summary>
        public GraphicsDeviceManager Graphics;
        public Room[,] RoomMap;
        public int RoomMapWidth, RoomMapHeight;
        public int CurrentRoomX, CurrentRoomY;
        public PEngine Engine;
        private SpriteBatch spriteBatch;
        public int MaxTimerLength;
        public int Timer;
        public Boolean BossAlreadySpawned;
        /// <summary>
        /// creates a new instance of the platformer game
        /// </summary>
        public PlatformerGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// initializes the platformer game
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            Window.Title = "The Dark Halls";
            ChangeResolution(1024, 768);

            Engine = new PEngine(this);
            PEngine.NameToType["obj_block"] = typeof(BlockObject); //if there is a better way to go about doing this please tell
            PEngine.NameToType["obj_player"] = typeof(PlayerObject);
            PEngine.NameToType["obj_item"] = typeof(ItemObject);
            PEngine.NameToType["obj_upgrade"] = typeof(UpgradeObject);
            PEngine.NameToType["obj_enemy"] = typeof(EnemyObject);
            PEngine.NameToType["obj_boss"] = typeof(BossObject);
            PEngine.NameToType["obj_win"] = typeof(GameWinObject);
            PEngine.NameToType["obj_lose"] = typeof(GameOverObject);
            PEngine.NameToType["tle_stonebrick"] = typeof(StoneBrickTile);


            CurrentRoomX = -1;
            CurrentRoomY = -1;
            RoomMapWidth = 12;
            RoomMapHeight = 12;
            RoomMap = new Room[RoomMapWidth, RoomMapHeight];
            for(int i = 0; i < RoomMapWidth; i++)
            {
                for(int j = 0; j < RoomMapHeight; j++)
                {
                    RoomMap[i, j] = GetRandomDungeonRoom();
                }
            }
            Engine.LoadRoom("Levels\\startingroom.json");
            MaxTimerLength = 60 * 40;
            Timer = MaxTimerLength;
            BossAlreadySpawned = false;
            base.Initialize();
        }
        public Room GetRandomDungeonRoom()
        {
            string filename = "Levels\\dungeon";
            filename += PlatformerMath.Choose("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10") + ".json";
            return (new Room(Engine)).Load(filename);
        }
        public void GoToRoom(int x, int y)
        {
            if (x >= RoomMapWidth) x = RoomMapWidth - 1;
            if (y >= RoomMapHeight) y = RoomMapHeight - 1;
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            BossObject bossObject = ((BossObject)Engine.CurrentRoom.FindObject("obj_boss"));
            if (bossObject != null)
            {
                bossObject.Position += new Vector2((CurrentRoomX - x) * 1024, (CurrentRoomY - y) * 768);
            }
            CurrentRoomX = x;
            CurrentRoomY = y;
            ConsoleManager.WriteLine("room position x: " + CurrentRoomX + ", y: " + CurrentRoomY);
            Engine.ChangeRoom(RoomMap[x, y]);
            MaxTimerLength -= 60;
            Timer = MaxTimerLength;
        }
        /// <summary>
        /// sets the game to a state of fullscreen
        /// </summary>
        /// <param name="full">whether to make fullscreen</param>
        public void SetFullscreen(bool full)
        {
            Graphics.IsFullScreen = full;
            Graphics.ApplyChanges();
        }
        /// <summary>
        /// changes the resolution of the game
        /// </summary>
        /// <param name="width">the new width</param>
        /// <param name="height">the new height</param>
        public void ChangeResolution(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
        }
        /// <summary>
        /// loads game content
        /// LoadContent will be called once per game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Engine.Assets.LoadAssetsFromFile("Data\\assets.json");
            Engine.CurrentRoom.LightEffect = Content.Load<Effect>("lighteffect"); //dont want to add another part to the asset manager just for this
        }

        /// <summary>
        /// unloads game content
        /// UnloadContent will be called once per game
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// called every tick event
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Timer--;
            if(Timer < 0 && !BossAlreadySpawned)
            {
                ((BossObject)Engine.CurrentRoom.FindObject("obj_boss")).IsAggro = true;
                Engine.Assets.RequestSound("msc_boss", (snd) =>
                {
                    Engine.CurrentRoom.Sounds.PlayMusic(snd, 0.5f);
                });
                Engine.CurrentRoom.GameObjectList.Add(new PreparedMaObject(Engine.CurrentRoom, new Vector2(0, 0)));
                BossAlreadySpawned = true;
            }
            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                Exit();
            Engine.Update();
            base.Update(gameTime);
        }
        
        /// <summary>
        /// called every frame draw event
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Engine.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
