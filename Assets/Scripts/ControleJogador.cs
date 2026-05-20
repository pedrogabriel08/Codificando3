using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Gojo - Vazio Roxo")]
    public GameObject prefabVazioRoxo;
    public UnityEngine.UI.Image imagemClarao;
    private bool estaConjurando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        if (imagemClarao != null)
        {
            Color c = imagemClarao.color;
            c.a = 0f;
            imagemClarao.color = c;
        }
    }

    void Update()
    {
        if (estaConjurando) return;

        // --- 1. ROTAÇÃO ---
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        transform.Rotate(Vector3.up * mouseX);
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);

        // --- 2. CORRER ---
        velocidadeAtual = Input.GetKey(KeyCode.LeftShift) ? velocidadeCorrida : velocidadeCaminhada;

        // --- 3. PULO ---
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
            estaNoChao = false;
        }

        // --- 4. ATIVAR O VAZIO ROXO ---
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(SequenciaVazioRoxo());
        }
    }

    void FixedUpdate()
    {
        if (estaConjurando)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        float moverFrenteTras = Input.GetAxis("Vertical");
        float moverEsquerdaDireita = Input.GetAxis("Horizontal");

        Vector3 direcao = transform.forward * moverFrenteTras + transform.right * moverEsquerdaDireita;
        Vector3 velocidadeMovimento = direcao * velocidadeAtual;

        rb.linearVelocity = new Vector3(velocidadeMovimento.x, rb.linearVelocity.y, velocidadeMovimento.z);
    }

    // --- 5. SEQUÊNCIA GOTY EXPANDIDA ---
    IEnumerator SequenciaVazioRoxo()
    {
        estaConjurando = true;

        Vector3 posInicialVermelha = transform.position + (transform.forward * -1f) + (transform.right * -2f) + (Vector3.up * 1.2f);
        Vector3 posInicialAzul = transform.position + (transform.forward * -1f) + (transform.right * 2f) + (Vector3.up * 1.2f);

        // Esfera Vermelha
        GameObject bolaVermelha = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bolaVermelha.transform.position = posInicialVermelha;
        bolaVermelha.transform.localScale = Vector3.one * 0.45f;
        Material matVermelho = new Material(Shader.Find("Unlit/Color"));
        matVermelho.color = Color.red;
        bolaVermelha.GetComponent<Renderer>().material = matVermelho;
        Destroy(bolaVermelha.GetComponent<Collider>());

        // Esfera Azul
        GameObject bolaAzul = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bolaAzul.transform.position = posInicialAzul;
        bolaAzul.transform.localScale = Vector3.one * 0.45f;
        Material matAzul = new Material(Shader.Find("Unlit/Color"));
        matAzul.color = Color.blue;
        bolaAzul.GetComponent<Renderer>().material = matAzul;
        Destroy(bolaAzul.GetComponent<Collider>());

        // Rastros
        TrailRenderer rastroVermelho = bolaVermelha.AddComponent<TrailRenderer>();
        rastroVermelho.time = 0.5f; rastroVermelho.startWidth = 0.4f; rastroVermelho.endWidth = 0.0f;
        rastroVermelho.material = new Material(Shader.Find("Sprites/Default"));
        rastroVermelho.startColor = Color.red; rastroVermelho.endColor = new Color(1, 0, 0, 0);

        TrailRenderer rastroAzul = bolaAzul.AddComponent<TrailRenderer>();
        rastroAzul.time = 0.5f; rastroAzul.startWidth = 0.4f; rastroAzul.endWidth = 0.0f;
        rastroAzul.material = new Material(Shader.Find("Sprites/Default"));
        rastroAzul.startColor = Color.blue; rastroAzul.endColor = new Color(0, 0, 1, 0);

        // Vórtex de Pedras
        int quantidadePedras = 15;
        List<GameObject> pedras = new List<GameObject>();
        List<Vector3> eixosRotacao = new List<Vector3>();
        List<float> velocidadesOrbita = new List<float>();

        Vector3 pontoImpactoChao = transform.position + (transform.forward * 3f);
        pontoImpactoChao.y = -0.5f;
        float raioDispersao = 3.0f;

        for (int i = 0; i < quantidadePedras; i++)
        {
            GameObject pedra = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Mesh meshCubo = pedra.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = meshCubo.vertices;
            for (int v = 0; v < vertices.Length; v++) vertices[v] = vertices[v] * Random.Range(0.7f, 1.3f);
            meshCubo.vertices = vertices; meshCubo.RecalculateNormals();

            Vector2 pontoNoCirculo = Random.insideUnitCircle * raioDispersao;
            pedra.transform.position = pontoImpactoChao + new Vector3(pontoNoCirculo.x, Random.Range(-0.1f, -0.3f), pontoNoCirculo.y);
            pedra.transform.localScale = new Vector3(Random.Range(0.15f, 0.4f), Random.Range(0.3f, 0.6f), Random.Range(0.15f, 0.4f));

            Material matPedra = new Material(Shader.Find("Unlit/Color"));
            matPedra.color = new Color(0.2f, 0.2f, 0.2f);
            pedra.GetComponent<Renderer>().material = matPedra;
            Destroy(pedra.GetComponent<Collider>());

            pedras.Add(pedra); eixosRotacao.Add(Random.onUnitSphere); velocidadesOrbita.Add(Random.Range(2.5f, 6f));
        }

        StartCoroutine(TremerCamera(0.2f, 0.08f));

        // Movimento da Espiral
        float tempoMeta = 1.8f; float tempoPassado = 0f;
        while (tempoPassado < tempoMeta)
        {
            tempoPassado += Time.deltaTime;
            float progresso = tempoPassado / tempoMeta;

            Vector3 pontoFocoAtras = transform.position + (transform.forward * -0.3f) + (Vector3.up * 1.3f);
            Vector3 pontoFocoFrente = Camera.main.transform.position + (Camera.main.transform.forward * 3.5f);
            Vector3 centroAtual = Vector3.Lerp(pontoFocoAtras, pontoFocoFrente, progresso);

            float velocidadeGiroEsferas = 18f; float anguloEsferas = progresso * velocidadeGiroEsferas; float raioAtual = Mathf.Lerp(2.0f, 0f, progresso);

            Vector3 offsetVermelho = (transform.right * Mathf.Cos(anguloEsferas) + transform.up * Mathf.Sin(anguloEsferas)) * raioAtual;
            Vector3 offsetAzul = (transform.right * Mathf.Cos(anguloEsferas + Mathf.PI) + transform.up * Mathf.Sin(anguloEsferas + Mathf.PI)) * raioAtual;

            bolaVermelha.transform.position = centroAtual + offsetVermelho;
            bolaAzul.transform.position = centroAtual + offsetAzul;

            for (int i = 0; i < pedras.Count; i++)
            {
                if (pedras[i] != null)
                {
                    float forcaSuccao = progresso * velocidadesOrbita[i];
                    pedras[i].transform.position = Vector3.MoveTowards(pedras[i].transform.position, centroAtual, Time.deltaTime * forcaSuccao);
                    pedras[i].transform.RotateAround(centroAtual, transform.up, (50f + velocidadesOrbita[i] * 12f) * Time.deltaTime);
                    pedras[i].transform.Rotate(eixosRotacao[i] * 180f * Time.deltaTime);

                    float distanciaAoCentro = Vector3.Distance(pedras[i].transform.position, centroAtual);
                    if (distanciaAoCentro < 0.4f) pedras[i].transform.localScale = Vector3.MoveTowards(pedras[i].transform.localScale, Vector3.zero, Time.deltaTime * 3f);
                    if (distanciaAoCentro < 1.2f)
                    {
                        float t = 1f - (distanciaAoCentro / 1.2f);
                        pedras[i].GetComponent<Renderer>().material.color = Color.Lerp(new Color(0.2f, 0.2f, 0.2f), new Color(0.5f, 0f, 0.8f), t);
                    }
                }
            }
            yield return null;
        }

        Destroy(bolaVermelha); Destroy(bolaAzul);
        foreach (GameObject pedra in pedras) if (pedra != null) Destroy(pedra);

        // --- IMPACTO DA FUSÃO (CÂMERA LENTA) ---
        Time.timeScale = 0.05f; Time.fixedDeltaTime = 0.02f * Time.timeScale;
        if (imagemClarao != null) { Color c = imagemClarao.color; c.a = 1f; imagemClarao.color = c; }

        // ONDA DE CHOQUE EXPANSIVA
        Vector3 posicaoFusao = Camera.main.transform.position + (Camera.main.transform.forward * 3.5f);
        StartCoroutine(CriarOndaChoque(posicaoFusao));

        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1.0f; Time.fixedDeltaTime = 0.02f;

        // Instancia o Vazio Roxo
        GameObject vazio = Instantiate(prefabVazioRoxo, posicaoFusao, Camera.main.transform.rotation);
        vazio.transform.localScale = Vector3.one * 3.0f;

        Renderer rendVazio = vazio.GetComponent<Renderer>();
        if (rendVazio != null)
        {
            Material matVazio = new Material(Shader.Find("Unlit/Color"));
            matVazio.color = new Color(0.4f, 0f, 0.7f);
            rendVazio.material = matVazio;
        }

        // APLICA FORÇA DE DESTRUIÇÃO CONTÍNUA AO VAZIO ROXO
        StartCoroutine(MecanicaDestruicaoProjetil(vazio));

        StartCoroutine(TremerCamera(0.7f, 0.4f));
        StartCoroutine(DesvanecerClarao());

        estaConjurando = false;
    }

    // GERADOR DA ONDA DE CHOQUE GEOMÉTRICA
    IEnumerator CriarOndaChoque(Vector3 centro)
    {
        GameObject onda = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        onda.transform.position = centro;
        onda.transform.localScale = new Vector3(1f, 0.02f, 1f);
        Destroy(onda.GetComponent<Collider>());

        Material matOnda = new Material(Shader.Find("Unlit/Color"));
        matOnda.color = new Color(0.6f, 0.2f, 1f, 0.5f);
        onda.GetComponent<Renderer>().material = matOnda;

        float tempo = 0f;
        while (tempo < 0.4f)
        {
            tempo += Time.deltaTime / Time.timeScale;
            float tamanho = Mathf.Lerp(1f, 15f, tempo / 0.4f);
            onda.transform.localScale = new Vector3(tamanho, 0.02f, tamanho);
            yield return null;
        }
        Destroy(onda);
    }

    // MECÂNICA GOTY: O VAZIO ROXO DESTRÓI O CENÁRIO DE VERDADE
    IEnumerator MecanicaDestruicaoProjetil(GameObject projetil)
    {
        while (projetil != null)
        {
            Collider[] objetosAtingidos = Physics.OverlapSphere(projetil.transform.position, 6f);
            foreach (Collider col in objetosAtingidos)
            {
                Rigidbody rbObjeto = col.GetComponent<Rigidbody>();
                if (rbObjeto != null && col.gameObject != gameObject)
                {
                    Vector3 direcaoExplosao = col.transform.position - projetil.transform.position;
                    rbObjeto.AddForce(direcaoExplosao.normalized * 40f, ForceMode.Impulse);
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator DesvanecerClarao()
    {
        if (imagemClarao == null) yield break;
        float tempo = 0f; Color c = imagemClarao.color;
        while (tempo < 0.6f)
        {
            tempo += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, tempo / 0.6f);
            imagemClarao.color = c;
            yield return null;
        }
    }

    IEnumerator TremerCamera(float duracao, float magnitude)
    {
        Vector3 posicaoOriginal = Camera.main.transform.localPosition;
        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.localPosition = new Vector3(x, y, posicaoOriginal.z);
            yield return null;
        }
        Camera.main.transform.localPosition = posicaoOriginal;
    }

    private void OnCollisionStay(Collision collision) => estaNoChao = true;
    private void OnCollisionExit(Collision collision) => estaNoChao = false;
}