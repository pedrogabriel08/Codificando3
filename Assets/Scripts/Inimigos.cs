using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Inimigos : MonoBehaviour
{
    public int dano;
    public int vida;
    public bool special;

    private void Awake()
    {
        dano = Random.Range(1, 21);
        special = dano > 10;
    }


    public void Atacar()
    {

        if (special)
        {
            Debug.Log("Ataque Especial de " + dano);
        }
        else
        {
            Debug.Log("Ataque Normal de " + dano);
        }
    }
    void Start()
    {

    }

}
    



