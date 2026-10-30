using Lib.Entities;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(0)]
public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance;

    [Header("Valeurs a ajuster dans l'inspecteur")]
    public TMP_Dropdown dropdown;
    public GameObject[] prefabsPlayer = new GameObject[4];
    public Vector3[] spawnsJoueurs = new Vector3[4];
    public GameSettingsScriptableObject gameSettings;

    //NetworkVariable<Player[]> sharedPlayerArray = new NetworkVariable<Player[]>();
    //NetworkList<PlayerData> playerDatas = new NetworkList<PlayerData>();

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

        if (gameSettings == null) gameSettings = (GameSettingsScriptableObject)ScriptableObject.CreateInstance(nameof(GameSettingsScriptableObject));
        Debug.Log(gameSettings);
    }
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {

        }
    }
    private void OnEnable()
    {
        if (dropdown != null) dropdown.onValueChanged.AddListener(ChangeTurnCount);
        NetworkManager.Singleton.OnConnectionEvent += ManagePlayers;
    }
    private void OnDisable()
    {
        if (dropdown != null) dropdown.onValueChanged.RemoveListener(ChangeTurnCount);
        NetworkManager.Singleton.OnConnectionEvent -= ManagePlayers;
    }



    void DebugGameSettings()
    {
        Debug.Log(gameSettings);
    }

    public void ChangeTurnCount(int dropdownIndex)
    {
        if (!IsHost) return;
        string optionText = dropdown.options[dropdownIndex].text;
        //Debug.Log(optionText);
        int newTurnCount = int.Parse(optionText);
        //Debug.Log(newTurnCount);
        gameSettings.ChangeTurnCount(newTurnCount);
        DebugGameSettings();
    }

    public void ToggleMinigames()
    {
        if (!IsHost) return;
        gameSettings.ToggleMinigames();
        DebugGameSettings();
    }

    void ManagePlayers(NetworkManager _, ConnectionEventData eventData)
    {
        if (!IsHost) return;
        Debug.Log("NetworkManager.OnConnectionEvent");
        //Debug.Log(eventData.EventType);
        if (eventData.EventType == ConnectionEvent.ClientConnected)
        {
            //Debug.Log("Player connected");
            //NetworkManager.Singleton.Instantiate(prefabsPlayer[eventData.ClientId], spawnsJoueurs[eventData.ClientId], new Quaternion());
            //Instantiate(prefabsPlayer[eventData.ClientId], spawnsJoueurs[eventData.ClientId], new Quaternion()).GetComponent<NetworkObject>().Spawn();
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                prefabsPlayer[eventData.ClientId].GetComponent<NetworkObject>(),
                eventData.ClientId,
                true,
                true,
                position: spawnsJoueurs[eventData.ClientId]);
            //NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
            //    prefabsPlayer[eventData.ClientId].GetComponent<NetworkObject>(),
            //    eventData.ClientId,
            //    isPlayerObject: true,
            //    position: spawnsJoueurs[eventData.ClientId]);

            ManagePlayers(false);
        }
    }
    public void ManagePlayers(bool isBot = true)
    {
        gameSettings.AddPlayer(isBot);

        DebugGameSettings();
    }
    public void ManagePlayers(int playerPos)
    {
        NetworkManager.DisconnectClient((ulong)playerPos);
        gameSettings.RemovePlayer(playerPos);

        DebugGameSettings();
    }


    public void LancerHote()
    {
        //NetworkManager.Singleton.StartHost();
        RelayManager.instance.StartCoroutine(RelayManager.instance.ConfigureTransportAndStartNgoAsHost());
    }
    public void LancerClient()
    {
        //NetworkManager.Singleton.StartClient();
        RelayManager.instance.StartCoroutine(RelayManager.instance.ConfigureTransportAndStartNgoAsConnectingPlayer());
    }
    public void LancerPartie(string nomScene)
    {
        NetworkManager.SceneManager.LoadScene(nomScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
