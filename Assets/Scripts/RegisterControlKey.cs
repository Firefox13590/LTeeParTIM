using System;
using TMPro;
using UnityEngine;

public class RegisterControlKey : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameObject panelRegisterControlKey;

    // variables privées
    bool isListeningKey = false;
    public TextMeshProUGUI textControle;
    public KeyCode registeredKey = KeyCode.None;

    // événements publiques statiques
    public static event Action<int, string, KeyCode> OnControlKeyRegistered;
    void Awake()
    {
        textControle = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Affiche le panneau d'enregistrement de la touche de contrôle.
    /// </summary>
    public void DisplayPanel()
    {
        panelRegisterControlKey.SetActive(true);
        isListeningKey = true;
    }

    /// <summary>
    /// Utilise la méthode <see cref="OnGUI"/> pour détecter les entrées clavier.
    /// </summary>
    private void OnGUI()
    {
        Event e = Event.current;
        if(e.type == EventType.KeyDown && isListeningKey)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                if(e.keyCode != KeyCode.Escape)
                {
                    // enregistrer la touche appuyée si ce n'est pas Echap
                    textControle.text = e.keyCode.ToString();
                    registeredKey = e.keyCode;
                }
                // sortie du mode écoute
                //Debug.Log("Detected key code: " + e.keyCode);
                isListeningKey = false;
                panelRegisterControlKey.SetActive(false);

                // décomposer le tuple pour obtenir les paramètres manquants
                var (indexPlayer, nomControle) = GetMissingEventParameters();
                OnControlKeyRegistered?.Invoke(indexPlayer, nomControle, e.keyCode);
            }
        }
    }

    /// <summary>
    /// Récupère les paramètres manquants pour l'événement.
    /// </summary>
    /// 
    /// <returns>Un tuple contenant l'index du joueur et le nom du contrôle.</returns>
    (int, string) GetMissingEventParameters()
    {
        int indexPlayer;
        string nomControle = gameObject.name.Split('_')[1];

        indexPlayer = Int32.Parse(GetComponentsInParent<RectTransform>()[2].gameObject.name.Split('_')[1]);

        return (indexPlayer, nomControle);
    }
}
