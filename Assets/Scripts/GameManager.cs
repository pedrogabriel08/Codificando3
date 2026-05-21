using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List <Inimigos> inimigoList = new();

    private void Start()
    {
        for (int i = 0; i < inimigoList.Count; i++)
        {
            inimigoList[i].Atacar();
        }
    }

}

