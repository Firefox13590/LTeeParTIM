using UnityEngine;
using TMPro;
using System;
using System.Reflection;
using Lib.Globals;

public class MenuParametres : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameSettingsScriptableObject gameSettings;

    // variables privées
    GameObject[] conteneursControlesPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitDefaultPlayerControls();

        // abonnement aux évènements
        RegisterControlKey.OnControlKeyRegistered += UpdateControlsKey;
    }

    void OnDestroy()
    {
        // désabonnement aux évènements
        RegisterControlKey.OnControlKeyRegistered -= UpdateControlsKey;
    }



    /// <summary>
    /// Met à jour la touche d'un contrôle pour un joueur donné.
    /// </summary>
    /// <param name="indexPlayer">Index du joueur dont on souhaite mettre à jour le contrôle.</param>
    /// <param name="nomControle">Nom du contrôle à mettre à jour.</param>
    /// <param name="key">Nouvelle touche à assigner au contrôle.</param>
    void UpdateControlsKey(int indexPlayer, string nomControle, KeyCode key)
    {
        Debug.Log($"Event output (int, string, KeyCode): {indexPlayer}    {nomControle}    {key}");

        // Utilisation de la réflexion pour accéder dynamiquement à la propriété du contrôle
        Type typeControle = typeof(Controls);
        PropertyInfo propinfoControle = typeControle.GetProperty(nomControle);
        Debug.Log("property info: " + propinfoControle);
        Debug.Log($"Type: {typeControle}    PropertyInfo: {propinfoControle.Name}");
        if (propinfoControle != null )
        {
            //propinfoControle.SetValue(gameSettings.Players[indexPlayer].Controls, key);
            object obj = gameSettings.Players[indexPlayer].Controls;
            propinfoControle.SetValue(obj, key);
            gameSettings.Players[indexPlayer].Controls = (Controls)obj;

            Debug.Log(propinfoControle.GetValue(gameSettings.Players[indexPlayer].Controls));
        }
        else
        {
            Debug.LogError("Cant set property value");
        }

        Debug.Log(gameSettings.Players[0].Controls);
    }

    /// <summary>
    /// Initialise les contrôles des joueurs avec les valeurs par défaut.
    /// </summary>
    void InitDefaultPlayerControls()
    {
        //Debug.Log(gameSettings.Players.Length);

        conteneursControlesPlayer = GameObject.FindGameObjectsWithTag("ConteneurControlesPlayer");
        //conteneursControlesPlayer.OrderBy(conteneur => conteneur.name).ToArray();
        Array.Sort(conteneursControlesPlayer, (a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

        for (int i = 0; i < conteneursControlesPlayer.Length; i++)
        {
            //Debug.Log(conteneursControlesPlayer[i].name);
            conteneursControlesPlayer[i].GetComponentInChildren<TextMeshProUGUI>().text = gameSettings.Players[i].Name;
            for (int j = 0; j < 5; j++)
            {
                conteneursControlesPlayer[i].GetComponentsInChildren<RegisterControlKey>()[j].textControle.text =
                    gameSettings.Players[i].Controls.AllControls[j].ToString();
            }
        }
    }
}
