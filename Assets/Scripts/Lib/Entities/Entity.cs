using System;
using Unity.Netcode;
using UnityEngine;

namespace Lib.Entities
{
    public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
    {
        public int CurrentPos;
        public int Currency;

        public PlayerData(int currentPos, int currency)
        {
            CurrentPos = currentPos;
            Currency = currency;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref CurrentPos);
            serializer.SerializeValue(ref Currency);
        }

        public bool Equals(PlayerData other)
        {
            return CurrentPos == other.CurrentPos
                && Currency == other.Currency;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrentPos, Currency);
        }
    }



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
    public class Player : Entity, IEquatable<Player>
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

        public bool Equals(Player other)
        {
            return other != null
                   && CurrentPos == other.CurrentPos
                   && Currency == other.Currency;
        }
        public override bool Equals(object obj) => Equals(obj as Player);

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrentPos, Currency);
        }
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