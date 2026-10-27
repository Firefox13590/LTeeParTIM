using UnityEngine;
using Lib.Entities;
using System.Collections.Generic;

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

    /// <summary>
    /// Remet tous les controles des joueurs aux valeurs par defaut.
    /// </summary>
    public void Init()
    {
        players = new() { new Player("Joueur 1") };
        maxTurnCount = 10;
        allowMinigames = true;

        ResetPlayerGameStats();
    }

    public void AddPlayer(bool isBot = false)
    {
        if (players.Count < 4)
        {
            if (isBot) players.Add(new Bot());
            else players.Add(new Player());
        }
    }

    public void RemovePlayer(int pos)
    {
        players.RemoveAt(pos);
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
