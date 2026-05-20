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

    }

  
}
