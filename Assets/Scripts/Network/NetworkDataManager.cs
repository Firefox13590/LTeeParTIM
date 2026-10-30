using Lib.Entities;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataManager : NetworkBehaviour
{
    public static NetworkDataManager Instance;

    public NetworkList<PlayerData> playerDatas = new NetworkList<PlayerData>();

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
    }



    public void Init()
    {
        for (int i = 0; i < 4; i++)
        {
            playerDatas.Add(PlayerData.Empty);
        }
    }

    public void DebugData()
    {
        for (int i = 0; i < playerDatas.Count; i++)
        {
            Debug.Log(playerDatas[i]);
        }
    }
}
