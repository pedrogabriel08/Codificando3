using UnityEngine;

public class Inimigos : MonoBehaviour
{
    public int dano;
    public int vida;
    public bool Special;

    void Atacar()
    {
     
        if (Special) {
            Debug.Log("Ataque Especial");
        }
        else
        {
            Debug.Log("Ataque Normal");
        }   
    void Start()
    {
        
    }

}
