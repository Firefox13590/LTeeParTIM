using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameSettingsScriptableObject gameSettings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        gameSettings = new();
    }


    public void ChangeTurnCount(int newTurnCount)
    {
        gameSettings.ChangeTurnCount(newTurnCount);
    }

    public void ToggleMinigames()
    {
        gameSettings.ToggleMinigames();
    }
}
