using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour {

    public Text txtVoltas;

    //para ver no ambiente de cena
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 2.5f);
    }

    //A funcao "OnTriggerEnter" e executada quando existe uma colisao
    public void OnTriggerEnter (Collider other){
		//obtem o objeto de controle de quem atingiu o checkpoint
		Controle outro = other.GetComponentInParent<Controle> ();
		if (other.CompareTag ("CarBaseCollider")) { //colidiu com o objeto do tipo correto (carro)
			Debug.Log (string.Format ("Collided with {0}", other.name)); //verifica com quem houve a colisao
            //passou pelo checkpoint correto

            if (this.name == outro.getCheck().name) {
                if (this.name == "Largada") { //passando pela largada
                    outro.voltas++; //incrementa o numero de voltas de quem passou pela largada

                    if (!other.gameObject.GetComponentInParent<IADriver>())
                    {
                        txtVoltas.text = (outro.voltas + 1).ToString() + "/" + Settings.numVoltas.ToString();
                    }

                    //condicao de vitoria/derota
                    if (outro.voltas >= Settings.numVoltas) { //atingiu o numero de voltas da corrida
                        SceneManager.LoadScene("Menu"); //vai para a tela de fim de corrida (criar uma)
                    }
                }
				Debug.Log(string.Format("Passou pelo checkpoint {0} at time {1}", outro.getIndex(), Time.time));
				outro.setIndex(outro.getIndex()+1); //incrementa o indice
				
				//conta a volta e reseta o indice
				if (outro.getIndex() == outro.getCheckLenght())
				{
					outro.setIndex(0); //reseta o indice
				}
                Debug.Log(string.Format("Proximo checkpoint: {0}", outro.getIndex()));
            }
		}
    } //fim OnTriggerEnter

} //fim da classe Checkpoint
