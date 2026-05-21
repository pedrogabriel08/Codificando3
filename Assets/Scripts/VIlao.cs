using UnityEngine;

public class Vilao : Personagem
{
        void Start()
        {
            Vilao goblin = new Vilao();
            goblin.nome = "Goblin";
            goblin.tipo = "Fraco";
            goblin.vida = 50;
            goblin.dano = 10;
            goblin.velocidade = 2.5f;

            Vilao orc = new Vilao();
            orc.nome = "Orc";
            orc.tipo = "Médio";
            orc.vida = 100;
            orc.dano = 20;
            orc.velocidade = 2.0f;

            Vilao dragão = new Vilao();
            dragão.nome = "Dragão";
            dragão.tipo = "Forte";
            dragão.vida = 300;
            dragão.dano = 50;
            dragão.velocidade = 1.5f;
    }
    
       
}
