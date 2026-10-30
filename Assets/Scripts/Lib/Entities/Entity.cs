using System;
using Unity.Netcode;
using UnityEngine;

namespace Lib.Entities
{
    public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
    {
        public int CurrentPos;
        public int Currency;

        public static readonly PlayerData Empty = new PlayerData(-1, -1);

        public PlayerData(int currentPos, int currency)
        {
            CurrentPos = currentPos;
            Currency = currency;
        }
        public override string ToString()
        {
            return $"{{ CurrentPos={CurrentPos} Currency={Currency} }}";
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
        public static bool operator ==(PlayerData left, PlayerData right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(PlayerData left, PlayerData right)
        {
            return !(left == right);
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