using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal;
    float speed = 5.0f;  //velocidade
    float accuracy = 1f; // ver a distancia do objeto
    float rotSpeed = 2f;// velocidade da rotação

    public GameObject wpManager;//Script para pegar componentes do outro Script
    GameObject[] wp;// lista de de array wp
    GameObject currentNode;
    int currentWP = 0; // Reseta array
    Graph gra;

      void Start()
    {

        wp = wpManager.GetComponent<WpManager>().waypoints;//pegou o componente waypoints do WpManager
        gra = wpManager.GetComponent<WpManager>().graph; //pegou o componente graph do WpManager


        currentNode = wp[0]; //Iniciou array zerado
    }

    public void GotoHeliport()//Classe que dispara evento para ir ao helioporto
    {
        gra.AStar(currentNode, wp[1]);
        currentWP = 0;
    }
    public void GotoRuins()//Classe que dispara evento para ir as ruinas
    {
        gra.AStar(currentNode, wp[6]);
        currentWP = 0;
    }
    public void GotoTanks()// Classe que dispara evento para ir aos tanques
    {
        gra.AStar(currentNode, wp[7]);
        currentWP = 0;
    }


    private void LateUpdate()
    {
        // Caso o valor seja igual 0 ou o maior da lista, retorna o valor inicial. 
        if (gra.getPathLength() == 0 || currentWP == gra.getPathLength()) 
            return;

        //Pega o node mais proximo 
        currentNode = gra.getPathPoint(currentWP);

        //Caso o tanque esteja próximo o bastante de um nó, o tanque se moverá para o próximo
        if (Vector3.Distance(gra.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        { currentWP++; }

        if (currentWP < gra.getPathLength())
        {
            goal = gra.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}