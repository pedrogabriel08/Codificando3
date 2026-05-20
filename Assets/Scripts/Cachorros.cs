using UnityEngine;

public class Cachorros : MonoBehaviour
{
    public string nome;
    public bool sono = false;

    private void Start()
    {
       Cachorros pitbull = new Cachorros();
         pitbull.nome = "Rex";
         pitbull.sono = true;
    }
}
