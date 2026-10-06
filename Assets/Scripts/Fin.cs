using TMPro;
using UnityEngine;

public class Fin : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameSettingsScriptableObject gameSettings;
    public TextMeshProUGUI texteVictoire;

    // variables publiques statiques
    public static int[] scoreJoueurs = new int[4];

    // variables privées
    int indexGagnant = -1, scoreGagant = -1;
    Vector3 rotationGagant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < scoreJoueurs.Length; i++)
        {
            if (scoreJoueurs[i] > scoreGagant)
            {
                scoreGagant = scoreJoueurs[i];
                indexGagnant = i;
            }
        }

        //Debug.Log($"gagant a pos {indexGagnant} avec {scoreGagant} points");
        transform.GetChild(indexGagnant).gameObject.SetActive(true);

        rotationGagant = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));

        texteVictoire.text = $"{gameSettings.Players[indexGagnant].Name}\nis win!\n\nAvec {scoreGagant}\nLégos!";
    }

    // Update is called once per frame
    void Update()
    {
        // la danse du siècle
        transform.GetChild(indexGagnant).gameObject.transform.Rotate(rotationGagant);
    }
}
