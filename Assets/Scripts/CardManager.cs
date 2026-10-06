using Lib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Lib.ArrayMovement;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public Material[] matCartes = new Material[8];
    public GameObject glow, cartePrefab;
    public PlayerControls playerControls;

    [Header("Acces publique pour autres scripts")]
    public List<GameObject> listeCartes;
    public float halfPoint;

    // variables privées
    int glowPos = 0;

    // évènements publiques statiques
    public static event Action OnCardSelect;

    // Use this for initialization
    void Start()
    {
        RemplirListeCartes();

        // obtention du point milieu
        halfPoint = listeCartes.Count / 2f;
        //Debug.Log(halfPoint);
    }



    /// <summary>
    /// Remplit le paquet de cartes
    /// </summary>
    void RemplirListeCartes()
    {
        for(int i = 0; i < 10; i++)
        {
            // instanciation
            GameObject instanceCarte = Instantiate(cartePrefab);

            // paramétrage de l'instance
            instanceCarte.name = "Carte_" + i;
            instanceCarte.GetComponent<Image>().material = matCartes[Random.Range(0, matCartes.Length)];
            //Debug.Log("valeur carte: " + instanceCarte.GetComponent<Image>().material.name[4]);
            instanceCarte.transform.SetParent(gameObject.transform, false);

            // ajout de l'instance à la liste
            listeCartes.Add(instanceCarte);
        }

        //halfPoint = listeCartes.Count / 2f;
        PositionnerCartes();
    }

    /// <summary>
    /// Positionne les cartes à l'écran. Varie selon le nombre de cartes restantes.
    /// </summary>
    void PositionnerCartes()
    {
        halfPoint = listeCartes.Count / 2f;

        int ajustedIndex, nbCartesRangee, row, ajustedX, gap = 100;
        const int width = 200;
        glowPos = ajustedX = 0;

        for (int i = 0; i < listeCartes.Count; i++)
        {
            ajustedIndex = i;
            nbCartesRangee = (int)Math.Ceiling(halfPoint);
            //Debug.Log("float halfPoint: " + halfPoint);
            //Debug.Log("int halfPoint: " + (int)Math.Ceiling(halfPoint));

            // détermination de la rangée
            if (i < Math.Ceiling(halfPoint))
            {
                // rangée du haut
                row = 0;
                //Debug.Log("rangee haut: " + i);
            }
            else
            {
                // rangée du bas
                row = 1;
                ajustedIndex -= (int)Math.Ceiling(halfPoint);
                //Debug.Log($"rangee bas: {i}    ajustedIndex: {ajustedIndex}");
                nbCartesRangee = (int)Math.Floor(halfPoint);
            }

            // ajuste le postonnement et l'espace entre les cartes selon de nombre de cartes dans la rangée
            switch (nbCartesRangee)
            {
                case 5:
                    gap = 100;
                    ajustedX = 0;
                    break;
                case 4:
                    gap = 150;
                    ajustedX = 75;
                    break;
                case 3:
                    gap = 300;
                    ajustedX = 100;
                    break;
                case 2:
                    gap = 400;
                    ajustedX = 300;
                    break;
                case 1:
                    gap = 0;
                    ajustedX = 600;
                    break;
            }

            listeCartes[i].GetComponent<RectTransform>().anchoredPosition = new Vector2
                ((-600 + ajustedX) + ajustedIndex * (width + gap)/* - 600*/,
                200 - 400 * row);
            //Debug.Log($"new vector2 for {listeCartes[i].name}: {listeCartes[i].GetComponent<RectTransform>().anchoredPosition}");
        }

        // remet l'indice de sélection à la première carte
        glow.GetComponent<RectTransform>().anchoredPosition = listeCartes[glowPos].GetComponent<RectTransform>().anchoredPosition;
    }

    /// <summary>
    /// Tourne la carte sélectonnée afin d'affcher sa valeur.
    /// </summary>
    public void ChoisirCarte()
    {
        //Debug.Log("avant Rotate(): " + listeCartes[glowPos].GetComponent<RectTransform>().rotation);
        listeCartes[glowPos].GetComponent<RectTransform>().Rotate(0, 180, 0);
        //Debug.Log("apres Rotate(): " + listeCartes[glowPos].GetComponent<RectTransform>().rotation);

        OnCardSelect.Invoke();

        Invoke(nameof(GiveMovesLeft), 1);
    }
    public void ChoisirCarte(int pos)
    {
        glowPos = pos;
        glow.GetComponent<RectTransform>().anchoredPosition = listeCartes[glowPos].GetComponent<RectTransform>().anchoredPosition;
        ChoisirCarte();
    }

    /// <summary>
    /// Donne la valeur de déplacement de la carte sélectionnée au joueur.
    /// </summary>
    void GiveMovesLeft()
    {
        //Debug.Log(listeCartes[glowPos].GetComponent<Image>().material.name[4]);
        //Debug.Log(Char.GetNumericValue(listeCartes[glowPos].GetComponent<Image>().material.name[4]));
        playerControls.GetMovesLeft((int) Char.GetNumericValue(listeCartes[glowPos].GetComponent<Image>().material.name[4]));

        Destroy(listeCartes[glowPos]);
        listeCartes.RemoveAt(glowPos);
        //Debug.Log(listeCartes.Count);

        if (listeCartes.Count == 0)
        {
            RemplirListeCartes();
        }
        else
        {
            PositionnerCartes();
        }
    }

    /// <summary>
    /// Gère le mouvement de l'indice de sélection de carte.
    /// </summary>
    /// <param name="move">La quantité de positions du déplacement</param>
    /// <param name="isReverse">Le sens du déplacement (vers le max ou le min).</param>
    public void BougerSelecteurCarte(int move, bool isReverse = false)
    {
        ComparaisonType comparaison = ComparaisonType.GreaterThan;
        if (isReverse)
        {
            comparaison = (ComparaisonType)(-(int)comparaison);
        }

        //Debug.Log("glow pos avant: " + glowPos);
        glowPos = ArrayMovement.CheckForLoopback(glowPos, listeCartes.Count - 1, move, comparaison: comparaison, reverse: isReverse);
        glow.GetComponent<RectTransform>().anchoredPosition = listeCartes[glowPos].GetComponent<RectTransform>().anchoredPosition;
        //Debug.Log("glow pos apres: " + glowPos);
    }
}
