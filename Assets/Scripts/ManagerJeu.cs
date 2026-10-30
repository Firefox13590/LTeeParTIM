using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ManagerJeu : NetworkBehaviour 
{
    public static ManagerJeu instance; 
    public bool partieEnCours { private set; get; } 
    public bool partieTerminee { private set; get; } 

    // NOUVEAU : Références pour les boutons et les points d'apparition
    [Header("Points d'apparition")]
    public Transform[] pointsDeSpawn; // Glissez vos Empty (SpawnJ1, SpawnJ2, J3, J4) ici dans l'inspecteur
    
    [HideInInspector]
    public int indexJoueurLocal = 0; // Mémorise le choix du bouton (0=Hôte, 1=J2, 2=J3, 3=J4)

    public GameObject myButtonHote;
    public GameObject myButtonClientJ2;
    public GameObject myButtonClientJ3;
    public GameObject myButtonClientJ4;
    public GameObject myMenu;

    void Awake()
    {
        myMenu.gameObject.SetActive(true);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // Seul l'hôte gère la logique de début et de fin de partie
        if (!IsHost) return;

        // Si la partie n'a pas encore commencé
        if (!partieEnCours)
        {
            if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2)
                NouvellePartie();
            
            return; // On s'arrête ici tant que la partie n'est pas lancée
        }

        // Si la partie est en cours, on vérifie continuellement s'il reste un seul joueur
        if (partieEnCours && !partieTerminee)
        {
            VerifierFinDePartie();
        }
    }

    private void VerifierFinDePartie()
    {
        // Trouve tous les objets dans la scène qui possèdent le tag "Player"
        GameObject[] joueursRestants = GameObject.FindGameObjectsWithTag("Player");

        // S'il reste 1 seul joueur (ou 0 si deux joueurs meurent en même temps)
        if (joueursRestants.Length <= 1)
        {
            FinPartie();

            // Récupère dynamiquement le nom de la scène située à l'index 3 dans les Build Settings
            string cheminScene = SceneUtility.GetScenePathByBuildIndex(3);
            string nomSceneIndex3 = System.IO.Path.GetFileNameWithoutExtension(cheminScene);

            // Demande au NetworkManager de charger la scène pour tout le monde
            NetworkManager.Singleton.SceneManager.LoadScene(nomSceneIndex3, LoadSceneMode.Single); //[cite: 34]
        }
    }

    // --- FONCTIONS POUR LES BOUTONS ---

    public void LanceCommeHote() 
    {
        indexJoueurLocal = 0; // J1 (Hôte)
        NetworkManager.Singleton.StartHost(); 
        CacherMenu();
    }

    public void LanceCommeClientJ2() 
    {
        indexJoueurLocal = 1; // J2
        LanceCommeClient();
    }

    public void LanceCommeClientJ3() 
    {
        indexJoueurLocal = 2; // J3
        LanceCommeClient();
    }

    public void LanceCommeClientJ4() 
    {
        indexJoueurLocal = 3; // J4
        LanceCommeClient();
    }

    private void LanceCommeClient() 
    {
        NetworkManager.Singleton.StartClient(); 
        CacherMenu();
    }

    private void CacherMenu()
    {
        myButtonHote.gameObject.SetActive(false); //[cite: 52]
        myButtonClientJ2.gameObject.SetActive(false); //[cite: 52]
        myButtonClientJ3.gameObject.SetActive(false); //[cite: 52]
        myButtonClientJ4.gameObject.SetActive(false); //[cite: 52]
    }

    public void NouvellePartie() { partieEnCours = true; }
    public void FinPartie() { partieTerminee = true; }
}