using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotControls : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public CardManager cardManager;
    public GameManager gameManager;

    [Header("Acces publique pour autres scripts")]
    public string inputType = "";

    // variables privées
    PlayerControls playerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = GetComponent<PlayerControls>();
    }

    /// <summary>
    /// Controle du bot pour choisir une carte.
    /// </summary>
    public void SelectionCarte()
    {
        int cardChoice = Random.Range(0, cardManager.listeCartes.Count);
        //Debug.Log("bot card choice: " + cardChoice);
        cardManager.ChoisirCarte(cardChoice);
    }

    /// <summary>
    /// Controle du bot pour choisir une case.
    /// </summary>
    public void ChooseTile()
    {
        if (Convert.ToBoolean(Random.Range(0, 2)))
        {
            //Debug.Log("bot changed tile selection");
            gameManager.ChangeTileSelection();
        }

        playerControls.TileSelected();
    }
}
