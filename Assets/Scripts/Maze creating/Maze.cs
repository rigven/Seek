using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeCreating
{
    [RequireComponent(typeof(MazeGeneration))]
    public class Maze : MonoBehaviour
    {
        [SerializeField]
        public float sizeGoriz = 10f, sizeVert = 8f;
        [SerializeField]
        public float passageWidth = 2f;
        public List<Crossroad> crossroads = new List<Crossroad>();

        private MazeGeneration mazeGen;


        void Start()
        {
            mazeGen = gameObject.GetComponent<MazeGeneration>();

            mazeGen.GenerateCrossroads(this);
            mazeGen.GeneratePassages(this);
        }

        
    }
}
