using PlatformerEngine.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PlatformerEditor
{
    public class LevelTab : HardGroupElement
    {
        public LevelTab(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            PlatformerEditor actualGame = (PlatformerEditor)UIManager.Game;
            Elements.Add(new LevelElement(UIManager, new Vector2(0, 0), new Vector2(Size.X, Size.Y), 0.3f, "level"));
            actualGame.WorldLayerListElement = new WorldLayerListElement(UIManager, new Vector2(0, 0), new Vector2(128, 256), 0.4f, "list_layers");
            Elements.Add(actualGame.WorldLayerListElement);
            actualGame.ObjectListElement = new WorldItemListElement(UIManager, new Vector2(0, 0), new Vector2(128, 240), 0.4f, "list_objects");
            actualGame.TileListElement = new WorldItemListElement(UIManager, new Vector2(0, 0), new Vector2(128, 240), 0.4f, "list_tiles");
            TabbedElement worldItemTabs = new TabbedElement(UIManager, new Vector2(0, 256), new Vector2(128, 256), 0.4f, "tabs_worlditems", 16);
            worldItemTabs.AddTab("objects", actualGame.ObjectListElement, 64);
            worldItemTabs.AddTab("tiles", actualGame.TileListElement, 64);
            Elements.Add(worldItemTabs);
            TextInputElement filenameInputElement = new TextInputElement(UIManager, new Vector2(0, 512), new Vector2(128, 24), 0.4f, "input_filename");
            ButtonElement loadButton = new ButtonElement(UIManager, new Vector2(0, 536), new Vector2(48, 24), 0.4f, "button_load", "load");
            loadButton.Click = () =>
            {
                string filename = filenameInputElement.Text;
                if(filename.Length == 0)
                {
                    return;
                }
                actualGame.LoadLevel(filename);
            };
            ButtonElement saveButton = new ButtonElement(UIManager, new Vector2(48, 536), new Vector2(48, 24), 0.4f, "button_save", "save");
            saveButton.Click = () =>
            {
                string filename = filenameInputElement.Text;
                if(filename.Length == 0)
                {
                    return;
                }
                actualGame.SaveLevel(filename);
            };
            Elements.Add(filenameInputElement);
            Elements.Add(loadButton);
            Elements.Add(saveButton);
            TextInputElement snapXInput = new TextInputElement(UIManager, new Vector2(0, 560), new Vector2(56, 24), 0.4f, "input_snap_x");
            TextInputElement snapYInput = new TextInputElement(UIManager, new Vector2(56, 560), new Vector2(56, 24), 0.4f, "input_snap_y");
            ButtonElement setSnapButton = new ButtonElement(UIManager, new Vector2(0, 584), new Vector2(64, 20), 0.4f, "button_snap_set", "set snap");
            setSnapButton.Click = () =>
            {
                LevelElement levelElement = (LevelElement)UIManager.GetUIElement("level");
                levelElement.Snap = new Vector2(int.Parse(snapXInput.Text), int.Parse(snapYInput.Text));
            };
            Elements.Add(snapXInput);
            Elements.Add(snapYInput);
            Elements.Add(setSnapButton);

            TextInputElement gravityXInput = new TextInputElement(UIManager, new Vector2(0, 604), new Vector2(56, 24), 0.4f, "input_gravity_x");
            TextInputElement gravityYInput = new TextInputElement(UIManager, new Vector2(56, 604), new Vector2(56, 24), 0.4f, "input_gravity_y");
            ButtonElement setGravityButton = new ButtonElement(UIManager, new Vector2(0, 628), new Vector2(64, 20), 0.4f, "button_gravity_set", "set gravity");
            setGravityButton.Click = () =>
            {
                LevelElement levelElement = (LevelElement)UIManager.GetUIElement("level");
                string xText = gravityXInput.Text;
                if (xText.Length == 0) xText = "0";
                string yText = gravityYInput.Text;
                if (yText.Length == 0) yText = "0";
                levelElement.Gravity = new Vector2(int.Parse(xText), int.Parse(yText));
            };
            Elements.Add(gravityXInput);
            Elements.Add(gravityYInput);
            Elements.Add(setGravityButton);

            TextInputElement roomWidthInput = new TextInputElement(UIManager, new Vector2(0, 648), new Vector2(56, 24), 0.4f, "input_room_height");
            TextInputElement roomHeightInput = new TextInputElement(UIManager, new Vector2(56, 648), new Vector2(56, 24), 0.4f, "input_room_width");
            ButtonElement setRoomSizeButton = new ButtonElement(UIManager, new Vector2(0, 672), new Vector2(64, 20), 0.4f, "button_room_size_set", "set room size");
            setRoomSizeButton.Click = () =>
            {
                LevelElement levelElement = (LevelElement)UIManager.GetUIElement("level");
                string xText = roomWidthInput.Text;
                if (xText.Length == 0) xText = "0";
                string yText = roomHeightInput.Text;
                if (yText.Length == 0) yText = "0";
                levelElement.LevelSize = new Vector2(int.Parse(xText), int.Parse(yText));
            };
            Elements.Add(roomWidthInput);
            Elements.Add(roomHeightInput);
            Elements.Add(setRoomSizeButton);
        }
    }
}
