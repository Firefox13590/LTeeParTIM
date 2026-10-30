using System;
using UnityEngine;
using UnityEngine.UI;
using Lib;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Lib.Entities;

[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameSettingsScriptableObject gameSettings;
    public PlayerControls playerControls;
    public GameObject[] tileChoiceIndocators = new GameObject[2];
    public Material[] playerMaterials = new Material[4], tileChoiceMaterials = new Material[2];
    public TextMeshProUGUI texteToursRestants;
    public Canvas canvas;

    [Header("Acces publique pour autres scripts")]
    public Transform[] tileChoice;

    // variables publiques statiques
    public static int playerTurn = 0;
    public static int turnsLeft = 10;

    GameObject popupTileChoice;
    GameObject[] playerStatsPanels = new GameObject[4];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        if (LobbyManager.Instance != null) gameSettings = LobbyManager.Instance.gameSettings;
        if (gameSettings == null) gameSettings = ScriptableObject.CreateInstance<GameSettingsScriptableObject>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start()
    {
        // abonnement aux évènements
        Case.OnTileChoiceStart += DisplayTileChoice;
        PlayerControls.OnTurnEnd += UpdatePlayerTurn;
        PlayerControls.OnTileChoiceEnd += HideTileChoice;


        popupTileChoice = canvas.transform.GetChild(0).gameObject;
        for (int i = 0; i < 4; i++)
        {
            playerStatsPanels[i] = canvas.transform.GetChild(2).GetChild(0).GetChild(i).gameObject;
        }

        //Debug.Log(playerMaterials[0].color);
        HighlightPlayerStatsPanel();

        texteToursRestants.text = "Tours restants: " + turnsLeft;
    }
    private void OnEnable()
    {
        if (NetworkDataManager.Instance != null) NetworkDataManager.Instance.playerDatas.OnListChanged += UpdatePlayerData;
    }
    private void OnDisable()
    {
        if (NetworkDataManager.Instance != null) NetworkDataManager.Instance.playerDatas.OnListChanged -= UpdatePlayerData;
    }

    private void OnDestroy()
    {
        // désabonnement aux évènements
        Case.OnTileChoiceStart -= DisplayTileChoice;
        PlayerControls.OnTurnEnd -= UpdatePlayerTurn;
        PlayerControls.OnTileChoiceEnd -= HideTileChoice;
    }



    /// <summary>
    /// Fait apparaitre le popup de choix de case.
    /// </summary>
    /// <param name="options">Les options de case</param>
    void DisplayTileChoice(Transform[] options)
    {
        tileChoice = options;
        popupTileChoice.SetActive(true);

        for (int i = 0; i < options.Length; i++)
        {
            tileChoiceIndocators[i].transform.position = options[i].transform.position + new Vector3(0, 10);
            tileChoiceIndocators[i].GetComponent<MeshRenderer>().material = tileChoiceMaterials[0];
            tileChoiceIndocators[i].SetActive(true);
        }
    }

    /// <summary>
    /// Cache le popup de choix de case ainsi que les indicateurs de sélection.
    /// </summary>
    void HideTileChoice()
    {
        popupTileChoice.SetActive(false);
        foreach (GameObject obj in tileChoiceIndocators)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Change la position des indicateurs de sélection.
    /// </summary>
    public void ChangeTileSelection()
    {
        Array.Reverse(tileChoice);
        for (int i = 0; i < tileChoiceIndocators.Length; i++)
        {
            tileChoiceIndocators[i].transform.position = tileChoice[i].transform.position + new Vector3(0, 10);
        }
        HighlightSelectedTile();
    }
    public void ChangeTileSelection(GameObject tileChoiceObject)
    {
        if (tileChoiceIndocators[0] != tileChoiceObject) ChangeTileSelection();
        else HighlightSelectedTile();
    }
    void HighlightSelectedTile()
    {
        tileChoiceIndocators[0].GetComponent<MeshRenderer>().material = tileChoiceMaterials[1];
    }

    /// <summary>
    /// Met à jour l'ordre du tour des joueurs.
    /// </summary>
    /// <remarks>À chaque fois que <see cref="playerTurn"/> revient à 0, décrémente <see cref="turnsLeft"/>.</remarks>
    void UpdatePlayerTurn()
    {
        if (gameSettings.Players[playerTurn] == null)
        {
            UpdateGameTurn();
        }
        else
        {
            AddCurrency();

            playerTurn = ArrayMovement.CheckForResetLoop(playerTurn + 1, gameSettings.Players.Length);
            //Debug.Log($"player type: {gameSettings.Players[playerTurn].GetType()}    player name: {gameSettings.Players[playerTurn].Name}");
            HighlightPlayerStatsPanel();

            if (playerTurn == 0)
            {
                UpdateGameTurn();
            }
        }
    }

    /// <summary>
    /// Met à jour le nombre de tours restants.
    /// </summary>
    void UpdateGameTurn()
    {
        turnsLeft--;
        texteToursRestants.text = "Tours restants: " + turnsLeft;

        if (turnsLeft == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    /// <summary>
    /// Met en évidence le panneau de statistiques du joueur dont c'est le tour actuel.
    /// </summary>
    void HighlightPlayerStatsPanel()
    {
        for (int i = 0; i < playerStatsPanels.Length; i++)
        {
            //Debug.Log(playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta);
            //Debug.Log(playerStatsPanels[i].GetComponent<RectTransform>().rect);
            if (i == playerTurn)
            {
                //playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta = new Vector2(450, 350);
                playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta = new Vector2(
                    playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta.x,
                    250);
                playerStatsPanels[i].GetComponentInChildren<Image>().color = playerMaterials[i].color;
            }
            else
            {
                //playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta = new Vector2(450, 300);
                playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta = new Vector2(
                    playerStatsPanels[i].GetComponent<RectTransform>().sizeDelta.x,
                    200);
                playerStatsPanels[i].GetComponentInChildren<Image>().color = new Color32(0, 0, 0, 100);
            }
        }
    }

    /// <summary>
    /// Incrémente la "monnaie" du joueur actuel.
    /// </summary>
    void AddCurrency()
    {
        //Debug.Log(playerStatsPanels[playerTurn].transform.childCount);
        TextMeshProUGUI textCurrency = playerStatsPanels[playerTurn].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        //Debug.Log(textCurrency.text);

        gameSettings.Players[playerTurn].Currency += Random.Range(2, 8);
        //Debug.Log("score de " + gameSettings.Players[playerTurn].Name + ": " + score);
        Fin.scoreJoueurs[playerTurn] = gameSettings.Players[playerTurn].Currency;
        textCurrency.text = "x  " + gameSettings.Players[playerTurn].Currency;
    }

    void UpdatePlayerData(NetworkListEvent<PlayerData> listEvent)
    {
        //Debug.Log(listEvent);
        gameSettings.Players[listEvent.Index].Currency = listEvent.Value.Currency;
        gameSettings.Players[listEvent.Index].CurrentPos = listEvent.Value.CurrentPos;
    }
}
