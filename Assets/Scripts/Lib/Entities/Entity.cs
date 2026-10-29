using UnityEngine;
using System;

namespace Lib.Entities
{
    /// <summary>
    /// Représente une entité avec un nom et une position actuelle.
    /// </summary>
    [Serializable]
    public class Entity
    {
        private int currentPos = 0;

        public string Name { get; set; }
        public int CurrentPos
        {
            get { return currentPos; }
            set { currentPos = value; }
        }

        public Entity(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Représente un joueur.
    /// </summary>
    /// <remarks>Hérite de la classe <see cref="Entity"/>.</remarks>
    [Serializable]
    public class Player : Entity
    {
        private int currency = 0;
        public int Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        private GameObject playerModel = null;
        public GameObject PlayerModel
        {
            get { return playerModel; }
            set { playerModel = value; }
        }

        public Player() : base("Player") { }
        public Player(string name) : base(name) { }
    }

    /// <summary>
    /// Représente un bot.
    /// </summary>
    /// <remarks>Hérite de la classe <see cref="Player"/>.</remarks>
    [Serializable]
    public class Bot : Player
    {
        public Bot() : base("Bot") { }
        public Bot(string name) : base(name) { }
    }
}