using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PlatformerTestGame
{
    public class FadeTransition : ITransition
    {
        public bool IsActive { get; private set; }
        private float progress;
        private int direction;
        private Action completionCallback;
        private static float progressRate = 0.02f;
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(Vector2.Zero, new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.Black * progress, new LayerData(99));
        }

        public ITransition Perform(bool isOpening, Action completionCallback = null)
        {
            IsActive = true;
            progress = isOpening ? 1 : 0;
            direction = isOpening ? -1 : 1;
            this.completionCallback = completionCallback;
            return this;
        }

        public void Update()
        {
            progress += progressRate * direction;
            if(progress <= 0 || progress >= 1)
            {
                IsActive = false;
                direction = 0;
                completionCallback?.Invoke();
            }
        }
    }
}
