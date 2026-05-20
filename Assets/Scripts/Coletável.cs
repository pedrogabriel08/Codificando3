using UnityEngine;

public class Coletável : MonoBehaviour
{
    public string nome;
    public int valor;
    public bool raro;
    public string descrição;
    void Start()
    {
        Coletável moeda = new Coletável();
        moeda.nome = "Moeda de Ouro";
        moeda.valor = 100;
        moeda.raro = false;
        moeda.descrição = "Uma moeda de ouro brilhante, valiosa mas comum.";

        Coletável gema = new Coletável();
        gema.nome = "Gema Mística";
        gema.valor = 500;
        gema.raro = true;
        gema.descrição = "Uma gema rara e poderosa, cobiçada por aventureiros.";


    }

}
