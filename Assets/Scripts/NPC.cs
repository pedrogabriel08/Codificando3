using UnityEngine;

public class NPC : MonoBehaviour
{

    public string nome;
    public bool podeFalar;
    public int idade;

    void Start()
    {
        NPC jonas = new NPC();
        jonas.nome = "Jonas";
        jonas.podeFalar = true;
        jonas.idade = 30;

        NPC juana = new NPC();
        juana.nome = "Juana";
        juana.podeFalar = false;
        juana.idade = 25;

        NPC ronaldo = new NPC();
        ronaldo.nome = "Ronaldo";   
        ronaldo.podeFalar = true;
        ronaldo.idade = 40;

    }

  
}
