using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Neva.BeatEmUp.Pathfinding.AStart
{
    internal sealed class AStarPathfinder : IPathfinder<AStarNode>
    {
        #region Vars
        private INodeGrid<AStarNode> grid;

        private bool skipCorners;
        private bool foundPath;
        #endregion

        #region Properties
        public bool SkipCorners
        {
            get
            {
                return skipCorners;
            }
            set
            {
                skipCorners = value;
            }
        }
        #endregion

        public AStarPathfinder()
        {
        }

        private List<Point> InternalFindPath(ref bool foundPath)
        {
            if (grid != null)
            {
                if (!grid.HasGoal)
                {
                    throw new PathfinderException(this.GetType(), "Goal is not set");
                }
                if (!grid.HasStart)
                {
                    throw new PathfinderException(this.GetType(), "Start is not set");
                }
            }
            else
            {
                throw new PathfinderException(this.GetType(), "Grid cant be null.");
            }

            List<AStarNode> openList = new List<AStarNode>();
            List<AStarNode> closedList = new List<AStarNode>();

            int indexX = grid.StartPosition.X / grid.NodeSize.X;
            int indexY = grid.StartPosition.Y / grid.NodeSize.Y;

            AStarNode current = grid.NodeAtIndex(indexY, indexX);
            openList.Add(current);

            while (true)
            {
                if (openList.Count == 0 || (current.Position.X == grid.GoalPosition.X && current.Position.Y == grid.GoalPosition.Y))
                {
                    break;
                }

                closedList.Add(current);

                indexX = current.Position.X / grid.NodeSize.X;
                indexY = current.Position.Y / grid.NodeSize.Y;

                List<AStarNode> newNodes = new List<AStarNode>();

                // Ylä.
                if (grid.IsWalkableAt(indexX, indexY - 1))
                {
                    newNodes.Add(grid.NodeAtIndex(indexX, indexY - 1));
                }
                // Ala.
                if (grid.IsWalkableAt(indexX, indexY + 1))
                {
                    newNodes.Add(grid.NodeAtIndex(indexX, indexY + 1));
                }
                // Oikea.
                if (grid.IsWalkableAt(indexX + 1, indexY))
                {
                    newNodes.Add(grid.NodeAtIndex(indexX + 1, indexY));
                }
                // Vasen.
                if (grid.IsWalkableAt(indexX - 1, indexY))
                {
                    newNodes.Add(grid.NodeAtIndex(indexX - 1, indexY));
                }

                // Otetaan viistosta nodet jos kulmat halutaan skipata.
                if (skipCorners)
                {
                    // Oikea ylä.
                    if (grid.IsWalkableAt(indexX + 1, indexY + 1))
                    {
                        newNodes.Add(grid.NodeAtIndex(indexX + 1, indexY + 1));
                    }
                    // Vasen ylä.
                    if (grid.IsWalkableAt(indexX - 1, indexY + 1))
                    {
                        newNodes.Add(grid.NodeAtIndex(indexX - 1, indexY + 1));
                    }
                    // Oikea ala.
                    if (grid.IsWalkableAt(indexX + 1, indexY - 1))
                    {
                        newNodes.Add(grid.NodeAtIndex(indexX + 1, indexY - 1));
                    }
                    // Vasen ala.
                    if (grid.IsWalkableAt(indexX - 1, indexY - 1))
                    {
                        newNodes.Add(grid.NodeAtIndex(indexX - 1, indexY - 1));
                    }
                }

                openList.Remove(current);

                for (int i = 0; i < newNodes.Count; i++)
                {
                    // Jos node on jo closedlistissä, skipataan.
                    if (closedList.Contains(newNodes[i]))
                    {
                        continue;
                    }

                    // Jos node on avoimessa listassa, katsotaan olisiko sen kautta nopeampi kulkea 
                    // tavoitteeseen.
                    if (openList.Contains(newNodes[i]))
                    {
                        if (newNodes[i].G > current.G)
                        {
                            newNodes[i].Parent = current;
                            newNodes[i].Update();
                        }
                    }
                    else
                    {
                        // Asetetaan uudelle nodelle parentti joka on tämän hetkinen node
                        // ja lasketaan sen arvot uudelleen. Lisätään se myös open listaan.
                        newNodes[i].Parent = current;
                        newNodes[i].Update();

                        openList.Add(newNodes[i]);
                    }
                }

                if (openList.Count == 0)
                {
                    break;
                }

                // Haetaan seuraava paras node johon voidaan liikkua.
                int minF = openList.Min(n => n.F);
                current = openList.FirstOrDefault(n => n.F == minF);
            }

            // Katostaan ollaako goalissa.
            foundPath = current.Position.X == grid.GoalPosition.X && current.Position.Y == grid.GoalPosition.Y;

            // Haetaan pisteet goaliin.
            List<Point> path = new List<Point>();
            INode node = current;

            while (node != null)
            {
                path.Add(node.Position);
                node = node.Parent;
            }

            path.Reverse();

            return path;
        }

        public void SetGrid(INodeGrid<AStarNode> grid)
        {
            this.grid = grid;
        }
        public List<Point> FindPath(ref bool foundPath)
        {
            return InternalFindPath(ref foundPath);
        }
        public List<Point> FindPath()
        {
            return InternalFindPath(ref foundPath);
        }
    }
}
