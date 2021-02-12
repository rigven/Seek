using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeCreating
{
    public class MazeGeneration : MonoBehaviour
    {
        //TEST
        [SerializeField]
        private GameObject testCube;

        /// <summary>
        /// Generating the main points of intersection of paths and dead ends.
        /// </summary>
        /// <param name="maze">The maze for which crossroads are generated.</param>
        public void GenerateCrossroads(Maze maze)
        {
            float x, y, z;
            // Number of crossroads to generate
            int crossroadsNumber = (int)(maze.sizeGoriz * maze.sizeGoriz * maze.sizeVert / maze.passageWidth / 120);
            for (int i = 0; i < crossroadsNumber; i++)
            {
                // Generating the coordinates of a new point
                NewCoordsGeneration:

                x = UnityEngine.Random.Range(0f, maze.sizeGoriz);
                y = UnityEngine.Random.Range(0f, maze.sizeVert);
                z = UnityEngine.Random.Range(0f, maze.sizeGoriz);
                Vector3 coords = new Vector3(x, y, z);

                // Generating coordinates again if the point is not close to the others
                for (int j = 0; j < maze.crossroads.Count; j++)
                {
                    if ((coords - maze.crossroads[j].coords).magnitude <= maze.passageWidth * 2f)
                    {
                        goto NewCoordsGeneration;
                    }
                }
                maze.crossroads.Add(new Crossroad(coords));
                //TEST
                Instantiate(testCube, coords, Quaternion.identity);
            }
        }

        /// <summary>
        /// Generating the passages between crossroads.
        /// </summary>
        /// <param name="maze">The maze for which passages are generated.</param>
        public void GeneratePassages(Maze maze)
        {
            // To reduce the number of paths at the forks, first the paths are laid only to those crossroads where there are no outgoing paths yet
            bool checkConnections = true;
            for (int i = 0; i < maze.crossroads.Count; i++)
            {
                int closestAvailableIndex = FindClosestAvailableCrossroad(maze, i, checkConnections);
                // If there are no crossroads left without outgoing paths, connection checking is disabled
                if (closestAvailableIndex == -1)
                {
                    checkConnections = false;
                    i--;
                    continue;
                }
                maze.crossroads[i].Connect(maze.crossroads[closestAvailableIndex]);
            }
        }

        /// <summary>
        /// Search for the nearest crossroad to which the path can be laid.
        /// </summary>
        /// <param name="maze">A maze whose crossroads are considered.</param>
        /// <param name="index">Index of the intersection for which the nearest available intersection is being searched.</param>
        /// <param name="checkConnections">Whether to consider intersections with outgoing paths available.</param>
        /// <returns>Index of the closest available crossroad. -1 if there are none.</returns>
        private int FindClosestAvailableCrossroad(Maze maze, int index, bool checkConnections)
        {
            float minDistance = float.MaxValue;
            int minIndex = -1;

            for (int j = 0; j < maze.crossroads.Count; j++)
            {
                // If the candidate is closer and is not the same crossroad for which the nearest one is being searched
                if ((maze.crossroads[index].coords - maze.crossroads[j].coords).magnitude < minDistance && j != index)
                {
                    // If the candidate has no connections yet, or if it doesn't needed to check the connections
                    if (maze.crossroads[j].outgoingConnections.Count == 0 || !checkConnections)
                    {
                        minIndex = j;
                        minDistance = (maze.crossroads[index].coords - maze.crossroads[j].coords).magnitude;
                    }

                }
            }
            //TEST
            if (minIndex != -1)
            {
                Debug.DrawRay(maze.crossroads[index].coords, maze.crossroads[minIndex].coords - maze.crossroads[index].coords, Color.green, 10);
            }
            return minIndex;
        }
    }
}
