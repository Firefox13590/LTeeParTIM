using UnityEngine;
using System;

namespace Lib.Globals
{
    /// <summary>
    /// Struct representant les controles d'un joueur.
    /// </summary>
    [Serializable]
    public struct Controls
    {
        public KeyCode Up { get; set; }
        public KeyCode Right { get; set; }
        public KeyCode Down { get; set; }
        public KeyCode Left { get; set; }
        public KeyCode Action { get; set; }
        public readonly KeyCode[] AllControls
        {
            get
            {
                return new KeyCode[5] { Up, Right, Down, Left, Action };
            }
        }

        public Controls(KeyCode up, KeyCode right, KeyCode down, KeyCode left, KeyCode action)
        {
            Up = up;
            Right = right;
            Down = down;
            Left = left;
            Action = action;
        }

        public override readonly string ToString()
        {
            return $"Up: {Up}, Right: {Right}, Down: {Down}, Left: {Left}, Action: {Action}";
        }
    }
}