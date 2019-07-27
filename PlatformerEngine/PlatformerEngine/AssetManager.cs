using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// manages all game assets
    /// (textures, animated textures, sounds)
    /// </summary>
    public class AssetManager
    {
        private Dictionary<string, Texture2D> textureAssets = new Dictionary<string, Texture2D>();
        private Dictionary<string, Texture2D[]> framedTextureAssets = new Dictionary<string, Texture2D[]>();
        private Dictionary<string, SoundEffect> soundAssets = new Dictionary<string, SoundEffect>();
        private Dictionary<string, SpriteFont> fontAssets = new Dictionary<string, SpriteFont>();
        private List<KeyValuePair<string, Action<Texture2D>>> textureAssetRequests = new List<KeyValuePair<string, Action<Texture2D>>>();
        private List<KeyValuePair<string, Action<Texture2D[]>>> framedTextureAssetRequests = new List<KeyValuePair<string, Action<Texture2D[]>>>();
        private List<KeyValuePair<string, Action<SoundEffect>>> soundAssetRequests = new List<KeyValuePair<string, Action<SoundEffect>>>();
        private List<KeyValuePair<string, Action<SpriteFont>>> fontAssetRequests = new List<KeyValuePair<string, Action<SpriteFont>>>();
        /// <summary>
        /// The content manager to load data from.
        /// Set this before you load with the asset manager.
        /// </summary>
        public ContentManager Content;
        /// <summary>
        /// creates a new assetmanager
        /// </summary>
        /// <param name="content">the content manager to use</param>
        public AssetManager(ContentManager content)
        {
            Content = content;
        }
        /// <summary>
        /// loads a texture of multiple frames
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <param name="frameCount">number of frames to load</param>
        /// <returns>a texture array with each loaded frame</returns>
        public Texture2D[] LoadFramedTexture(string internalName, string location, int frameCount)
        {
            Texture2D[] frames = new Texture2D[frameCount];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = Content.Load<Texture2D>(location + i.ToString());
            }
            for (int i = framedTextureAssetRequests.Count - 1; i >= 0; i--)
            {
                var req = framedTextureAssetRequests[i];
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(frames);
                    framedTextureAssetRequests.RemoveAt(i);
                }
            }
            ConsoleManager.WriteLine("loaded framed texture \"" + internalName + "\" from " + location, "load");
            framedTextureAssets[internalName] = frames;
            return frames;
        }
        /// <summary>
        /// loads a texture
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <returns>the loaded texture</returns>
        public Texture2D LoadTexture(string internalName, string location)
        {
            Texture2D texture = Content.Load<Texture2D>(location);
            for(int i = textureAssetRequests.Count - 1; i >= 0; i--)
            {
                var req = textureAssetRequests[i];
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(texture);
                    textureAssetRequests.RemoveAt(i);
                }
            }
            ConsoleManager.WriteLine("loaded texture \"" + internalName + "\" from " + location, "load");
            textureAssets[internalName] = texture;
            return texture;
        }
        /// <summary>
        /// loads a sound
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <returns>the loaded sound</returns>
        public SoundEffect LoadSound(string internalName, string location)
        {
            SoundEffect sound = Content.Load<SoundEffect>(location);
            for (int i = soundAssetRequests.Count - 1; i >= 0; i--)
            {
                var req = soundAssetRequests[i];
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(sound);
                    soundAssetRequests.RemoveAt(i);
                }
            }
            ConsoleManager.WriteLine("loaded sound \"" + internalName + "\" from " + location, "load");
            soundAssets[internalName] = sound;
            return sound;
        }
        /// <summary>
        /// loads a font
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <returns>the loaded font</returns>
        public SpriteFont LoadFont(string internalName, string location)
        {
            SpriteFont font = Content.Load<SpriteFont>(location);
            for (int i = fontAssetRequests.Count - 1; i >= 0; i--)
            {
                var req = fontAssetRequests[i];
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(font);
                    fontAssetRequests.RemoveAt(i);
                }
            }
            ConsoleManager.WriteLine("loaded font \"" + internalName + "\" from " + location, "load");
            fontAssets[internalName] = font;
            return font;
        }
        /// <summary>
        /// requests a framed texture
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the frames</param>
        public void RequestFramedTexture(string assetName, Action<Texture2D[]> callback)
        {
            if(framedTextureAssets.ContainsKey(assetName))
            {
                callback.Invoke(framedTextureAssets[assetName]);
            }
            else
            {
                framedTextureAssetRequests.Add(new KeyValuePair<string, Action<Texture2D[]>>(assetName, callback));
            }
        }
        /// <summary>
        /// requests a texture
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the texture</param>
        public void RequestTexture(string assetName, Action<Texture2D> callback)
        {
            if (textureAssets.ContainsKey(assetName))
            {
                callback.Invoke(textureAssets[assetName]);
            }
            else
            {
                textureAssetRequests.Add(new KeyValuePair<string, Action<Texture2D>>(assetName, callback));
            }
        }
        /// <summary>
        /// requests a sound
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the sound</param>
        public void RequestSound(string assetName, Action<SoundEffect> callback)
        {
            if (soundAssets.ContainsKey(assetName))
            {
                callback.Invoke(soundAssets[assetName]);
            }
            else
            {
                soundAssetRequests.Add(new KeyValuePair<string, Action<SoundEffect>>(assetName, callback));
            }
        }
        /// <summary>
        /// requests a font
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the font</param>
        public void RequestFont(string assetName, Action<SpriteFont> callback)
        {
            if (fontAssets.ContainsKey(assetName))
            {
                callback.Invoke(fontAssets[assetName]);
            }
            else
            {
                fontAssetRequests.Add(new KeyValuePair<string, Action<SpriteFont>>(assetName, callback));
            }
        }
        /// <summary>
        /// loads the assets listed in the json file
        /// </summary>
        /// <param name="filepath">the path to the json file</param>
        public void LoadAssetsFromFile(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);
            string json = "";
            for (int i = 0; i < lines.Length; i++)
            {
                json += lines[i] + "\n";
            }
            JObject obj = JObject.Parse(json);
            JArray assetList = (JArray)obj.GetValue("assets").ToObject(typeof(JArray));
            foreach (JToken assetToken in assetList)
            {
                JObject assetObject = (JObject)assetToken.ToObject(typeof(JObject));
                string internalName = (string)assetObject.GetValue("name").ToObject(typeof(string));
                string typeName = (string)assetObject.GetValue("type").ToObject(typeof(string));
                string path = (string)assetObject.GetValue("path").ToObject(typeof(string));
                if(typeName.Equals("texture"))
                {
                    LoadTexture(internalName, path);
                }
                else if(typeName.Equals("font"))
                {
                    LoadFont(internalName, path);
                }
                else if(typeName.Equals("framedTexture"))
                {
                    int frameCount = (int)assetObject.GetValue("frames").ToObject(typeof(int));
                    LoadFramedTexture(internalName, path, frameCount);
                }
                else if(typeName.Equals("sound"))
                {
                    LoadSound(internalName, path);
                }
                else
                {
                    ConsoleManager.WriteLine("could not find asset type \"" + typeName + "\"", "err");
                }
            }
        }
    }
}
