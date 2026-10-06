using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public RectTransform[] listeMenus;
    [Range(0, 2)]
    public int menuCible = 1;

    // variables privées
    RectTransform rtrCanvas;
    int menuActuel = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rtrCanvas = GetComponent<RectTransform>();
        Debug.Log(rtrCanvas.rect);

        for(int i = 0; i < listeMenus.Length; i++)
        {
            listeMenus[i].sizeDelta = new Vector2(rtrCanvas.rect.width, rtrCanvas.rect.height);
            listeMenus[i].anchoredPosition = new Vector2(rtrCanvas.rect.width * (i - 1), 0);
        }

        GoToMenu(menuCible);
    }



    /// <summary>
    /// Gère la navigation entre les menus.
    /// </summary>
    /// <param name="targetMenuIndex">L'index du menu cible</param>
    public void GoToMenu(int targetMenuIndex)
    {
        menuCible = targetMenuIndex;
        Vector2 movement = listeMenus[menuActuel].anchoredPosition - listeMenus[menuCible].anchoredPosition;
        //Debug.Log("movement: " + movement);

        foreach (RectTransform menu in listeMenus)
        {
            menu.anchoredPosition += movement;
        }
        menuActuel = menuCible;
    }

    /// <summary>
    /// Charge une scène en fonction de son index.
    /// </summary>
    /// <param name="sceneIndex">Index de la scène à charger</param>
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
