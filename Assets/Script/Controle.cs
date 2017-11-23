using UnityEngine;
using System.Collections;

public class Controle : MonoBehaviour {
	//atributos publicos
	private Transform[] checkpoints; //array de checkpoint
	public int voltas; //numero de voltas feitas
    
	//atributos privados
	private int checkIndex; //indice do checkpoint (controle de checkpoint)

    // Use this for initialization
    void Start () {
		voltas = 0; //inicializando a corrida
		checkIndex = 1; //proximo checkpoint a atingir
        //adiciona todos os checkpoints do circuito

        Transform check = GameObject.FindGameObjectWithTag("Track").transform.Find("Checkpoints"); //GameObject que tem os checkpoints 
		checkpoints = new Transform[check.childCount]; //cria um array com todos os checkpoints, na ordem que aparecem na hierarquia
		int i = 0; //reinicia a variavel
		//percorre toda a hierarquia e popula o array com os checkpoints
		foreach (Transform t in check.transform) {
			checkpoints[i++] = t;
		}
	} //fim de Start()
	
	//metodos get e set
	public int getIndex(){
		return this.checkIndex;
	}

	public Transform getCheck(){
		return this.checkpoints [this.checkIndex];
	}

	public int getCheckLenght(){
		return this.checkpoints.Length;
	}

	public void setIndex(int indice){
		this.checkIndex = indice;
	}
}
