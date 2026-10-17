using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public AudioClip sonCarte, sonBrique;

    // variables privées
    AudioSource musique;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // abonnement aux évènements
        CardManager.OnCardSelect += JouerSonCarte;
        PlayerControls.OnTurnEnd += JouerSonBrique;

        musique = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        // désabonnement aux évènements
        CardManager.OnCardSelect -= JouerSonCarte;
        PlayerControls.OnTurnEnd -= JouerSonBrique;
    }

    /// <summary>
    /// Joue le son pour la sélection de carte.
    /// </summary>
    void JouerSonCarte()
    {
        musique.PlayOneShot(sonCarte);
    }

    /// <summary>
    /// Joue le son pour la fin d'un tour (gain de briques).
    /// </summary>
    void JouerSonBrique()
    {
        musique.PlayOneShot(sonBrique);
    }
}
