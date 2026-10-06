using UnityEngine;
using UnityEngine.SceneManagement;

public class Recommencer : MonoBehaviour
{
    /// <summary>
    /// Ramène à la scène d'accueil (menu).
    /// </summary>
    /// <param name="indexScene">L'index de la scène d'accueil dans la liste de scènes.</param>
    public void RecommencerPartie(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}
