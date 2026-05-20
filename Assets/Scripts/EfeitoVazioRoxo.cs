using UnityEngine;

public class EfeitoVazioRoxo : MonoBehaviour
{
    public float velocidadeProjetil = 25f;
    public float tempoVida = 3f;

    void Start()
    {
        // Destrói o objeto após 3 segundos para poupar o PC
        Destroy(gameObject, tempoVida);
    }

    void Update()
    {
        // Move o Vazio Roxo sempre para frente
        transform.Translate(Vector3.forward * velocidadeProjetil * Time.deltaTime);
    }
}