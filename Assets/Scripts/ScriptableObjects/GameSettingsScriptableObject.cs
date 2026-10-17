using UnityEngine;
using Lib.Entities;

[CreateAssetMenu(fileName = "GameSettingsScriptableObject", menuName = "Scriptable Objects/GameSettingsScriptableObject")]
public class GameSettingsScriptableObject : ScriptableObject
{
    private Player player1 = new("Player1");
    private Player player3 = new Bot("Bot1");
    private Player player2 = new Bot("Bot2");
    private Player player4 = new Bot("Bot3");

    //private Player[] players;
    public Player[] Players
    {
        get
        {
            //return new Player[4] { player1, player2, player3, player4 };
            return new Player[1] { player1 };
        }
    }

    /// <summary>
    /// Remet tous les controles des joueurs aux valeurs par defaut.
    /// </summary>
    public void SetBackDefaultPlayerSettings()
    {
        player1 = new("Player1");
        player2 = new Bot("Bot1");
        player3 = new Bot("Bot2");
        player4 = new Bot("Bot3");
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
