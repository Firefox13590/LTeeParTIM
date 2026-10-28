using UnityEngine;
using Lib.Entities;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GameSettingsScriptableObject", menuName = "Scriptable Objects/GameSettingsScriptableObject")]
public class GameSettingsScriptableObject : ScriptableObject
{

    private List<Player> players;
    public List<Player> Players
    {
        get
        {
            return players;
        }
    }

    private int maxTurnCount = 10;
    public int MaxTurnCount
    {
        get { return maxTurnCount; }
        private set { maxTurnCount = value; }
    }

    private bool allowMinigames = true;
    public bool AllowMinigames
    {
        get { return allowMinigames; }
        private set { allowMinigames = value; }
    }



    public GameSettingsScriptableObject()
    {
        Init();
    }

    public override string ToString()
    {
        return $"{{ playerCount={players.Count} maxTurnCount={maxTurnCount} allowMinigames={allowMinigames} }}";
    }

    /// <summary>
    /// Remet tous les controles des joueurs aux valeurs par defaut.
    /// </summary>
    public void Init()
    {
        players = new();
        maxTurnCount = 10;
        allowMinigames = true;

        ResetPlayerGameStats();
    }

    public void AddPlayer(bool isBot = true)
    {
        if (players.Count < 4)
        {
            players.Add((isBot) ? new Bot($"Robot {players.Count + 1}") : new Player($"Joueur {players.Count + 1}"));
            return;
        }
        else if (!isBot)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] is Bot) players[i] = new Player($"Joueur {i + 1}");
                return;
            }
        }
    }

    public void RemovePlayer(int pos = -1)
    {
        if (pos == -1 && players.Count > 1) players.RemoveAt(players.Count - 1);
        else players.RemoveAt(pos);
    }

    public void ChangeTurnCount(int newTurnCount)
    {
        maxTurnCount = newTurnCount;
    }

    public void ToggleMinigames()
    {
        allowMinigames = !allowMinigames;
    }

    /// <summary>
    /// Réinitialise les statistiques de jeu de tous les joueurs.
    /// </summary>
    public void ResetPlayerGameStats()
    {
        foreach (Player player in Players)
        {
            player.CurrentPos = 0;
            player.Currency = 0;
        }
    }
}
