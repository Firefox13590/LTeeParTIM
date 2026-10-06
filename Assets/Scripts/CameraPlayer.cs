using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameObject[] players = new GameObject[4];
    public Vector3 ajustement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Suivi du joueur actif
        transform.position = players[GameManager.playerTurn].transform.position + ajustement;
        transform.LookAt(players[GameManager.playerTurn].transform.position);
    }
}
