using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.Physics
{
    public class PhysicsBasedObject
    {
        protected Vector2[] vertices;
        public int VertexCount { get { return vertices.Length; } }
        public Vector2 Position;
        public Vector2 Center { get { return center; } }
        private Vector2 center;
        public PhysicsBasedObject(Vector2[] vertices, Vector2 pos)
        {
            this.vertices = vertices;
            center = new Vector2(0, 0);
            foreach (Vector2 vertex in vertices)
            {
                center += vertex;
            }
            center /= vertices.Length;
            Position = pos;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 lastVertex = GetVertex(VertexCount - 1) + Position;
            for(int i = 0; i < VertexCount; i++)
            {
                Vector2 vertex = GetVertex(i) + Position;
                spriteBatch.DrawLine(lastVertex, vertex, Color.Black);
                lastVertex = vertex;
            }
        }
        public virtual Vector2 GetVertex(int ind)
        {
            return vertices[ind];
        }
        public virtual void Update()
        {

        }
        public Vector2[] GetAllVertices()
        {
            Vector2[] allVertices = new Vector2[VertexCount];
            for (int i = 0; i < allVertices.Length; i++)
            {
                allVertices[i] = GetVertex(i);
            }
            return allVertices;
        }
    }
}
