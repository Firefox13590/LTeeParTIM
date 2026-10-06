using System;
using UnityEngine;

public class Case : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public bool choixChemin = false, forceChemin = false;
    public Transform[] optionsCase;
    public Transform forceProchaineCase;

    [Header("Acces publique pour autres scripts")]
    public int indexCase;

    // événements publiques statiques
    public static event Action<Transform[]> OnTileChoiceStart;
    public static event Action<Transform> OnTileRedirect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // permet de repositionner le joueur en fonction de l'index de la case
        indexCase = Int32.Parse(name[5..]);
    }

    private void OnTriggerEnter(Collider other)
    {
        // lance des évènements selon certianes conditions
        //Debug.Log("collision avec: " + other.name);
        if (choixChemin/* && other.gameObject.CompareTag("Player")*/)
        {
            OnTileChoiceStart.Invoke(optionsCase);
        }
        else if (forceChemin)
        {
            OnTileRedirect.Invoke(forceProchaineCase);
        }
    }
}
