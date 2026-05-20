using UnityEngine;

public class ControleJogador : MonoBehaviour
{
    public float velocidade = 5.0f;
    public float sensibilidadeMouse = 2.0f;

    private float rotacaoX = 0f;

    void Start()
    {
        // Trava o mouse no centro da tela e o esconde
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 1. ROTAÇÃO (Olhar para os lados com o Mouse) ---
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        // Gira o corpo do jogador para os lados (Eixo Y)
        transform.Rotate(Vector3.up * mouseX);

        // Gira apenas a câmera para cima e para baixo (Eixo X) com limite para não dar uma cambalhota
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        // Busca a câmera que está dentro do jogador e aplica a rotação vertical
        Camera.main.transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);


        // --- 2. MOVIMENTO (Teclado WASD ou Setas) ---
        float moverFrenteTras = Input.GetAxis("Vertical"); // W, S ou Setas Cima/Baixo
        float moverEsquerdaDireita = Input.GetAxis("Horizontal"); // A, D ou Setas Esquerda/Direita

        // Calcula a direção baseada para onde o jogador está olhando
        Vector3 direcao = transform.forward * moverFrenteTras + transform.right * moverEsquerdaDireita;

        // Aplica o movimento
        transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);
    }
}