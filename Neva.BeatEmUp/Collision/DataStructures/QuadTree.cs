using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Neva.BeatEmUp.Collision.Broadphase;
using Neva.BeatEmUp.GameObjects;

namespace Neva.BeatEmUp.Collision.DataStructures
{
    class Element<TType>
    {
        public AABB AABB;
        public TType Value;

        public Element(TType value, AABB span)
        {
            AABB = span;
            Value = value;
        }
            
    }

    class QuadTree<TType>
    {

        #region Vars

        private readonly int maxDepth;
        private readonly int maxObjects;

        private readonly int depth;
        private AABB bounds;
        #endregion

        #region Properties
        private QuadTree<TType>[] Nodes
        {
            get;
            set;
        }

        private List<Element<TType>> Objects
        {
            get;
            set;
        }

        public AABB Bounds
        {
            get { return bounds; }
            private set
            {
                bounds = value;
            }
        }
        #endregion

        public QuadTree(AABB bounds, int maxObjects = 5, int maxDepth = 5, int depth=0)
        {
            Bounds = bounds;
            Objects = new List<Element<TType>>();

            this.maxDepth = maxDepth;
            this.maxObjects = maxObjects;
            this.depth = depth;
            Nodes = new QuadTree<TType>[4];
        }

        public void Clear()
        {
            Objects.Clear();
            for (int index = 0; index < Nodes.Length; index++)
            {
                if (Nodes[index] == null) break;
                Nodes[index].Clear();
                Nodes[index] = null;
            }
        }

        private void Split()
        {
            float halfWidth = (Bounds.Width/2f);
            float halfHeight = (Bounds.Height / 2f);
            float x = Bounds.Lower.X;
            float y = Bounds.Lower.Y;

            
            Nodes[0] = new QuadTree<TType>(new AABB(x + halfWidth, y, halfWidth, halfHeight), maxObjects, maxDepth, depth + 1); // oikee
            Nodes[1] = new QuadTree<TType>(new AABB(x, y, halfWidth, halfHeight), maxObjects, maxDepth, depth + 1); // vasen
            Nodes[2] = new QuadTree<TType>(new AABB(x, y + halfHeight, halfWidth, halfHeight), maxObjects, maxDepth, depth + 1); // oikee ala
            Nodes[3] = new QuadTree<TType>(new AABB(x + halfWidth, y + halfHeight, halfWidth, halfHeight), maxObjects, maxDepth, depth + 1); // vasen ala
            
        }

        public void Insert(Element<TType> colliadable)
        {
            //if (!colliadable.AABB.Intersects(ref bounds)) return;
            
            if (Nodes[0] != null)
            {
                int index = GetIndex(ref colliadable.AABB);
                if (index != -1)
                {
                    Nodes[index].Insert(colliadable);
                    return;
                }
            }

            Objects.Add(colliadable);

            // jos tultiin max object limitti vastaan mutta ei olla tarpeeksi syvällä
            if (Objects.Count > maxObjects && depth < maxDepth)
            {
                // jos ei ole nodeja niin jaetaan neljään osaan
                if (Nodes[0] == null) 
                    Split();

                for (int i = 0; i < Objects.Count;)
                {
                    Element<TType> node = Objects[i];
                    int index = GetIndex(ref colliadable.AABB);
                    // Jos indeksi on -1 niin juttuja
                    if (index != -1)
                    {
                        Objects.RemoveAt(i);
                        Nodes[index].Insert(node);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }


        public void QueryAABB(ref AABB aabb, ref List<Element<TType>> result)
        {
            int index = GetIndex(ref aabb);
            if (index != -1 && Nodes[0] != null)
            {
                Nodes[index].QueryAABB(ref aabb, ref result);
            }
            result.AddRange(Objects);
        }

        private int GetIndex(ref AABB rect)
        {
            int index = -1;
            float hMidpoint = Bounds.Lower.X + Bounds.Width / 2f;
            float vMidpoint = Bounds.Lower.Y + Bounds.Height/2f;

            // mennäänkö ylös
            bool toTop = rect.Lower.Y < vMidpoint && rect.Lower.Y + rect.Height < vMidpoint;
            // vai alas
            bool toBottom = rect.Lower.Y > vMidpoint;

            // vasemmalle?
            if (rect.Lower.X < hMidpoint && rect.Lower.X + rect.Width < hMidpoint)
            {
                if (toTop)
                    index = 1;
                else if (toBottom)
                    index = 2;
            } 
            // oikeelle?
            else if (rect.Lower.X > hMidpoint)
            {
                if (toTop)
                    index = 0;
                else if (toBottom)
                    index = 3;
            }

            return index;
        }

        #region Debug

        private void DrawTree(SpriteBatch sb, QuadTree<TType> quadTree)
        {
            if (quadTree == null) return;

            if (quadTree.Nodes[0] == null)
            {
                sb.DrawRectangle(quadTree.Bounds.ToRectangle(), Color.Yellow, 0);
            }

            foreach (var tree in quadTree.Nodes)
            {
                DrawTree(sb, tree);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            DrawTree(sb, this);
        }

        #endregion
    }
}
