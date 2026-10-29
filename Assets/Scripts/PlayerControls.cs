using Lib.Entities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Random = UnityEngine.Random;

public class PlayerControls : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameSettingsScriptableObject gameSettings;
    public GameObject[] playerObjects = new GameObject[4];
    public Vector3 playerPosAjust = Vector3.zero;

    [Header("Variables de test")]
    public int testCurrentPos = 0;

    [Header("Acces publique pour autres scripts")]
    public bool allowTileChoice = false, allowInput = true;
    public Vector3 targetPos = Vector3.zero, plannedRedirect = Vector3.zero;

    // variables privées
    GameObject[] listeCases;
    int movesLeft;
    bool allowMove = false;
    BotControls botControls;
    TouchState touchInput;
    CardManager cardManager;

    // évènements statiques
    public static event Action OnTurnEnd;
    public static event Action OnTileChoiceEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if !UNITY_EDITOR
testCurrentPos = 0;
#endif

        gameSettings = GameManager.Instance.gameSettings;

        // abonnement aux évènements
        Case.OnTileChoiceStart += StartTileSelection;
        Case.OnTileRedirect += TileRedirect;

        listeCases = GameObject.FindGameObjectsWithTag("Case");
        Array.Sort(listeCases, (a, b) => string.CompareOrdinal(a.name, b.name));

        for (int i = 0; i < playerObjects.Length; i++)
        {
            gameSettings.Players[i].CurrentPos = testCurrentPos;
            playerObjects[i].transform.position = listeCases[gameSettings.Players[i].CurrentPos].transform.position + playerPosAjust;
        }

        botControls = GetComponent<BotControls>();

        Debug.Log(Touchscreen.current.primaryTouch.valueType);
        Debug.Log(Touchscreen.current.primaryTouch.ReadValue());

        cardManager = GameManager.Instance.canvas.transform.GetChild(1).GetComponent<CardManager>();
        cardManager.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        // désabonnement aux évènements
        Case.OnTileChoiceStart -= StartTileSelection;
        Case.OnTileRedirect -= TileRedirect;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput)
        {
            SelectionCarte();
        }

        if (allowMove)
        {
            MovePlayer();
        }

        if (allowTileChoice)
        {
            ChooseTile();
        }
    }



    /// <summary>
    /// Obtient un nombre aléatoire de mouvements restants de 0 à 7.
    /// </summary>
    public void GetMovesLeft(int number)
    {
        movesLeft = number;
        //Debug.Log("starting moves: " + movesLeft);

        allowMove = true;
        //allowInput = false;

        cardManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Déplace le joueur en fonction des mouvements restants.
    /// </summary>
    void MovePlayer()
    {
        if (movesLeft > 0)
        {
            // calcul de la destiination et la distance tant qu'il reste des mouvements
            if (targetPos == Vector3.zero)
            {
                targetPos = listeCases[gameSettings.Players[GameManager.playerTurn].CurrentPos + 1].transform.position + playerPosAjust;
            }
            float distance = Vector3.Distance(playerObjects[GameManager.playerTurn].transform.position, targetPos);

            if (distance > 0)
            {
                // tant que la destination n'est pas atteinte, le joueur continue de se déplacer
                playerObjects[GameManager.playerTurn].transform.position = Vector3.MoveTowards(playerObjects[GameManager.playerTurn].transform.position, targetPos, .1f);
            }
            else
            {
                // sinon, les mouvements restants sont décrémentés et la position actuelle du joueur est mise à jour
                gameSettings.Players[GameManager.playerTurn].CurrentPos++;
                //Debug.Log($"new CurrentPos: {gameSettings.Players[GameManager.playerTurn].CurrentPos}    Vector3: {playerObjects[GameManager.playerTurn].transform.position}");
                movesLeft--;
                //Debug.Log("current moves left: " + movesLeft);

                if (plannedRedirect != Vector3.zero)
                {
                    targetPos = plannedRedirect;
                    plannedRedirect = Vector3.zero;
                }
                else
                {
                    targetPos = Vector3.zero;
                }
            }
        }
        else
        {
            OnTurnEnd.Invoke();

            allowMove = false;
            allowInput = true;

            cardManager.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Commence la sélection de la case parmi les options données.
    /// </summary>
    /// <param name="options">Les options parmi lesquelles choisir</param>
    /// <remarks>Cette méthode attend un évènement <see cref="Case.OnTileChoiceStart"/>.</remarks>
    void StartTileSelection(Transform[] options)
    {
        //foreach(Transform option in options)
        //{
        //    Debug.Log($"pos {option.name}: {option.position}");
        //}

        allowMove = false;
        allowTileChoice = true;
    }

    /// <summary>
    /// Permet au joueur de choisir une case parmi les options disponibles.
    /// </summary>
    void ChooseTile()
    {
        if (gameSettings.Players[GameManager.playerTurn].GetType() == typeof(Bot))
        {
            //Debug.Log($"bot {gameSettings.Players[GameManager.playerTurn].Name} tile selection");
            allowTileChoice = false;
            botControls.Invoke(nameof(botControls.ChooseTile), Random.Range(.25f, 1f));

            //return;
        }
        else
        {
            //Debug.Log("player tile selection");

            touchInput = Touchscreen.current.primaryTouch.ReadValue();
            //Debug.Log(touchInput);
            if (touchInput.isPrimaryTouch && touchInput.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Debug.Log(touchInput);
                //Debug.Log(touchInput.position);

                if (Physics.Raycast(Camera.main.ScreenPointToRay(touchInput.position), out RaycastHit hit))
                {
                    //Debug.Log(hit.collider.name);
                    //Debug.Log(hit.collider.name.StartsWith("TileChoice"));
                    if (hit.collider.name.StartsWith("TileChoice"))
                    {
                        GameManager.Instance.ChangeTileSelection(hit.collider.gameObject);
                        allowTileChoice = false;
                        Invoke(nameof(TileSelected), 1);
                        //TileSelected();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Applique la sélection de case faite par le joueur.
    /// </summary>
    public void TileSelected()
    {
        //Debug.Log(GameManager.Instance.tileChoice[0].gameObject.GetComponent<Case>().indexCase);
        gameSettings.Players[GameManager.playerTurn].CurrentPos = GameManager.Instance.tileChoice[0].gameObject.GetComponent<Case>().indexCase - 1;
        targetPos = GameManager.Instance.tileChoice[0].transform.position + playerPosAjust;

        OnTileChoiceEnd.Invoke();

        allowTileChoice = false;
        allowMove = true;
    }

    /// <summary>
    /// Stock la position de redirection de la case.
    /// </summary>
    /// <param name="redirect">Le transform contenant la position de redirection</param>
    void TileRedirect(Transform redirect)
    {
        //Debug.Log("planning redirect at: " + redirect);
        plannedRedirect = redirect.position + playerPosAjust;
        // -2 au lieu de -1 sinon ca saute une case... je sais pas pourquoi
        gameSettings.Players[GameManager.playerTurn].CurrentPos = redirect.gameObject.GetComponent<Case>().indexCase - 2;
    }

    /// <summary>
    /// Contrôle la sélection de carte par le joueur.
    /// </summary>
    void SelectionCarte()
    {
        if (gameSettings.Players[GameManager.playerTurn].GetType() == typeof(Bot))
        {
            //Debug.Log($"bot {gameSettings.Players[GameManager.playerTurn].Name} card selection");
            allowInput = false;
            botControls.Invoke(nameof(botControls.SelectionCarte), Random.Range(0, 1f));
        }
        else
        {
            //Debug.Log("player card selection");

            touchInput = Touchscreen.current.primaryTouch.ReadValue();
            //Debug.Log(touchInput);
            if (touchInput.isPrimaryTouch && touchInput.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Debug.Log(touchInput);
                //Debug.Log(touchInput.position);
                cardManager.ChoisirCarte(touchInput.position);
            }
        }
    }
}
