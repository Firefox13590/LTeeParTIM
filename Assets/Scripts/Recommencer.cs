using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Recommencer : NetworkBehaviour
{
    /// <summary>
    /// Ramène à la scène d'accueil (menu).
    /// </summary>
    /// <param name="indexScene">L'index de la scène d'accueil dans la liste de scènes.</param>
    public void RecommencerPartie(int indexScene)
    {
        if (IsHost) { NetworkManager.Shutdown(); }
        else
        { NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId); }
        SceneManager.LoadScene(indexScene);
    }
}
