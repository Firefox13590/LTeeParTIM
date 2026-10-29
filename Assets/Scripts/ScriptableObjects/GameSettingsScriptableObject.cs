using UnityEngine;
using Lib.Entities;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GameSettingsScriptableObject", menuName = "Scriptable Objects/GameSettingsScriptableObject")]
public class GameSettingsScriptableObject : ScriptableObject
{

    private Player[] players = new Player[4];
    public Player[] Players
    {
        get
        {
            return players;
        }
    }

    private int playerCount = 1;
    private int botCount = 0;

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
        return $"{{ playerCount={playerCount} botCount={botCount} maxTurnCount={maxTurnCount} allowMinigames={allowMinigames} }}";
    }

    /// <summary>
    /// Remet tous les controles des joueurs aux valeurs par defaut.
    /// </summary>
    public void Init()
    {
        players = new Player[4];
        maxTurnCount = 10;
        allowMinigames = true;

        //ResetPlayerGameStats();
    }

    public void AddPlayer(bool isBot = true)
    {
        bool playerAdded = false;
        for (int i = 0; i < players.Length; i++)
        {
            if (playerAdded)
            {
                Debug.Log(players[i].Name + " has joined!");
                break;
            }

            if (players[i] == null)
            {
                players[i] = (isBot) ? new Bot($"Robot {players.Length + 1}") : new Player($"Joueur {players.Length + 1}");
                break;
            }
            else if (players[i] is Bot && !isBot) players[i] = new Player($"Joueur {i + 1}");
        }
    }

    public void RemovePlayer(int pos)
    {
        if (players[pos] != null)
        {
            Debug.Log(players[pos].Name + " has left...");
            players[pos] = null;
        }
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
        if (players.Length == 0) return;
        foreach (Player player in Players)
        {
            player.CurrentPos = 0;
            player.Currency = 0;
        }
    }
}
