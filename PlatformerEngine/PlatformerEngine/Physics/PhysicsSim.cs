using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.Physics
{
    public class PhysicsSim
    {
        public List<ImmobileObject> ImmobileObjects;
        public List<MovingObject> MovingObjects;
        public List<GameObjectLink> GameObjectLinks;
        public Vector2 Gravity;
        public Vector2 AirFriction;
        public PhysicsSim()
        {
            ImmobileObjects = new List<ImmobileObject>();
            MovingObjects = new List<MovingObject>();
            GameObjectLinks = new List<GameObjectLink>();
            Gravity = new Vector2(0, 0.2f);
            AirFriction = new Vector2(0.4f, 0);
        }
        public void Update()
        {
            foreach (MovingObject obj in MovingObjects)
            {
                obj.Velocity += Gravity;
                obj.Update();
            }
            for (int i = 0; i < MovingObjects.Count; i++)
            {
                MovingObject obj = MovingObjects[i];
                obj.TouchingSurface = false;
                for (int j = 0; j < ImmobileObjects.Count; j++)
                {
                    ImmobileObject targetObj = ImmobileObjects[j];
                    Vector2[][] allLinePoints = GetCollision(obj, targetObj);
                    Vector2[] closest = null;
                    float? closestDist = null;
                    foreach (Vector2[] linePoints in allLinePoints)
                    {
                        float dist = (ClosestPointToLine(obj.Center + obj.Position, linePoints[2], linePoints[3]) - (obj.Center + obj.Position)).LengthSquared();
                        if (closest == null || dist < closestDist)
                        {
                            closest = linePoints;
                            closestDist = dist;
                        }
                    }
                    if (closest != null)
                    {
                        Collide(obj, targetObj, closest);
                        obj.TouchingSurface = true;
                    } 
                }
                if (!obj.TouchingSurface)
                {
                    Vector2 normalizedVelocity = new Vector2(obj.Velocity.X, obj.Velocity.Y);
                    normalizedVelocity.Normalize();
                    if (obj.Velocity.LengthSquared() > AirFriction.LengthSquared())
                    {
                        obj.Velocity -= normalizedVelocity * AirFriction;
                    }
                    else
                    {
                        obj.Velocity.X = 0;
                        obj.Velocity.Y = 0;
                    }
                }
            }
            for(int i = 0; i < GameObjectLinks.Count; i++)
            {
                GameObjectLink link = GameObjectLinks[i];
                link.Update();
            }
        }
        public bool CheckCollision(MovingObject a)
        {
            for(int i = 0; i < ImmobileObjects.Count; i++)
            {
                ImmobileObject obj = ImmobileObjects[i];
                if(GetCollision(a, obj).Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void Collide(MovingObject a, ImmobileObject b, Vector2[] linePoints)
        {
            Vector2[] aVertices = a.GetAllVertices();
            for (int i = 0; i < aVertices.Length; i++) aVertices[i] += a.Position;
            Vector2[] bVertices = b.GetAllVertices();
            for (int i = 0; i < bVertices.Length; i++) bVertices[i] += b.Position;
            Vector2 closestOnB = ClosestPointToLine(a.Center + a.Position, linePoints[2], linePoints[3]);
            Vector2 closestOnA = GetClosestPoint(closestOnB, aVertices);
            a.Position -= closestOnA - closestOnB;
            Vector2 bVector = linePoints[3] - linePoints[2];
            bVector.Normalize();
            bVector *= a.Velocity.Length();
            a.Velocity = ClosestPointToLine(a.Velocity, -bVector, bVector);
            Vector2 normalizedVelocity = new Vector2(a.Velocity.X, a.Velocity.Y);
            normalizedVelocity.Normalize();
            if (a.Velocity.LengthSquared() > b.Friction * b.Friction)
            {
                a.Velocity -= normalizedVelocity * b.Friction;
            }
            else
            {
                a.Velocity.X = 0;
                a.Velocity.Y = 0;
            }
        }
        public Vector2 GetVectorInDirection(Vector2 vec, float dir)
        {
            return new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir)) * vec;
        }
        public float AngleBetweenVectors(Vector2 a, Vector2 b)
        {
            return (float)Math.Acos((a.X * b.X + a.Y * b.Y) / (a.Length() * b.Length()));
        }
        public Vector2 ProjectVector(Vector2 a, Vector2 b)
        {
            return (a / a.Length()) * (float)Math.Cos(AngleBetweenVectors(a, b));
        }
        private static float mPi = (float)Math.PI;
        public float AngleDifference(float a, float b)
        {
            while (a > mPi) a -= mPi * 2;
            while (b > mPi) b -= mPi * 2;
            while (a < -mPi) a += mPi * 2;
            while (b < -mPi) b += mPi * 2;
            return (b - a + mPi) % (mPi * 2) - mPi;
        }
        public Vector2 GetClosestPoint(Vector2 targetPoint, Vector2[] vertices)
        {
            Vector2? closestPoint = null;
            float? closestDistSqr = null;
            Vector2 lastPoint = vertices[vertices.Length - 1];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 point = vertices[i];
                Vector2 testPoint = ClosestPointToLine(targetPoint, lastPoint, point);
                float dist = (testPoint - targetPoint).LengthSquared();
                if(closestDistSqr == null || closestDistSqr > dist)
                {
                    closestPoint = testPoint;
                    closestDistSqr = dist;
                }
                lastPoint = point;
            }
            return closestPoint ?? Vector2.Zero;
        }
        public Vector2 GetFarthestPoint(Vector2 targetPoint, Vector2[] vertices)
        {
            Vector2? farthestPoint = null;
            float? farthestDistSqr = null;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 point = vertices[i];
                float dist = (point - targetPoint).LengthSquared();
                if (farthestDistSqr == null || farthestDistSqr < dist)
                {
                    farthestPoint = point;
                    farthestDistSqr = dist;
                }
            }
            return farthestPoint ?? Vector2.Zero;
        }
        public Vector2[][] GetCollision(MovingObject a, ImmobileObject b)
        {
            List<Vector2[]> collisions = new List<Vector2[]>();
            Vector2 lastVertexA = a.GetVertex(a.VertexCount - 1) + a.Position;
            for(int i = 0; i < a.VertexCount; i++)
            {
                Vector2 vertexA = a.GetVertex(i) + a.Position;
                Vector2 lastVertexB = b.GetVertex(b.VertexCount - 1) + b.Position;
                for(int j = 0; j < b.VertexCount; j++)
                {
                    Vector2 vertexB = b.GetVertex(j) + b.Position;
                    if (CheckLineIntersection(lastVertexA, vertexA, lastVertexB, vertexB))
                    {
                        collisions.Add(new Vector2[] {
                            lastVertexA,
                            vertexA,
                            lastVertexB,
                            vertexB
                        });
                    }
                    lastVertexB = vertexB;
                }
                lastVertexA = vertexA;
            }
            return collisions.ToArray();
        }
        public Vector2 ClosestPointToLine(Vector2 point, Vector2 la, Vector2 lb)
        {
            Vector2 v = lb - la;
            Vector2 u = la - point;
            float vu = v.X * u.X + v.Y * u.Y;
            float vv = v.LengthSquared();
            float t = -vu / vv;
            if(t >= 0 && t <= 1)
            {
                return vectorToSegment(t, Vector2.Zero, la, lb);
            }
            float g0 = vectorToSegment(0, point, la, lb).LengthSquared();
            float g1 = vectorToSegment(1, point, la, lb).LengthSquared();
            return g0 <= g1 ? la : lb;
        }
        private Vector2 vectorToSegment(float t, Vector2 point, Vector2 la, Vector2 lb)
        {
            return new Vector2(1 - t, 1 - t) * la + new Vector2(t, t) * lb - point;
        }
        //http://jsfiddle.net/justin_c_rounds/Gd2S2/light/
        public static bool CheckLineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float denominator = ((b2.Y - b1.Y) * (a2.X - a1.X)) - ((b2.X - b1.X) * (a2.Y - a1.Y));
            if (denominator == 0)
            {
                return false;
            }
            float a = a1.Y - b1.Y;
            float b = a1.X - b1.X;
            float num1 = ((b2.X - b1.X) * a) - ((b2.Y - b1.Y) * b);
            float num2 = ((a2.X - a1.X) * a) - ((a2.Y - a1.Y) * b);
            a = num1 / denominator;
            b = num2 / denominator;
            /*
            float x = a1.X + (a * (a2.X - a1.X));
            float y = a1.Y + (a * (a2.Y - a1.Y));*/
            return a > 0 && a < 1 && b > 0 && b < 1;
        }
        public static Vector2[] GenerateRectangleVertices(Vector2 size)
        {
            return GenerateRectangleVertices(size.X, size.Y);
        }
        public static Vector2[] GenerateRectangleVertices(float width, float height)
        {
            width /= 2;
            height /= 2;
            return new Vector2[]
            {
                new Vector2(-width, -height),
                new Vector2(width, -height),
                new Vector2(width, height),
                new Vector2(-width, height)
            };
        }
    }
}
