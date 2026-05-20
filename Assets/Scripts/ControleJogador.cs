using UnityEngine;

public class ControleJogador : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidadeCaminhada = 5.0f;
    public float velocidadeCorrida = 10.0f;
    private float velocidadeAtual;

    [Header("Pulo e Física")]
    public float forcaPulo = 5.0f;
    private Rigidbody rb;
    private bool estaNoChao;

    [Header("Câmera e Mouse")]
    public float sensibilidadeMouse = 2.0f;
    private float rotacaoX = 0f;

    void Start()
    {
        // Pega o componente de física da cápsula
        rb = GetComponent<Rigidbody>();

        // Trava o mouse no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 1. ROTAÇÃO (Olhar para os lados e cima/baixo) ---
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        transform.Rotate(Vector3.up * mouseX);

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);


        // --- 2. MECÂNICA DE CORRER (Shift Esquerdo) ---
        // Se pressionar Shift, a velocidade atual vira a de corrida, senão, caminhada.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadeAtual = velocidadeCorrida;
        }
        else
        {
            velocidadeAtual = velocidadeCaminhada;
        }


        // --- 3. MECÂNICA DE PULO (Espaço) ---
        // Só permite pular se apertar Espaço E o personagem estiver tocando o chão
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
            estaNoChao = false; // Bloqueia o pulo duplo no ar
        }
    }

    void FixedUpdate()
    {
        // --- 4. MOVIMENTO (Aplicado no FixedUpdate para melhor precisão física) ---
        float moverFrenteTras = Input.GetAxis("Vertical");
        float moverEsquerdaDireita = Input.GetAxis("Horizontal");

        Vector3 direcao = transform.forward * moverFrenteTras + transform.right * moverEsquerdaDireita;

        // Move preservando a velocidade vertical do pulo/gravidade
        Vector3 velocidadeMovimento = direcao * velocidadeAtual;
        rb.linearVelocity = new Vector3(velocidadeMovimento.x, rb.linearVelocity.y, velocidadeMovimento.z);
    }

    // --- 5. DETECÇÃO DE CHÃO ---
    // Verifica se a cápsula está colidindo com o chão para liberar o pulo novamente
    private void OnCollisionStay(Collision collision)
    {
        // Se colidir com qualquer objeto (como o seu chão), diz que está no chão
        estaNoChao = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Quando sai do chão (ao pular ou cair de uma plataforma)
        estaNoChao = false;
    }
}