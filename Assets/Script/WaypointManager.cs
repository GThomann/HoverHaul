using UnityEngine;
using System.Collections;

//STEPS:
//1. Adicione este script a um GameObject com Collider (o seu waypoint).
//2. Garanta que os seus waypoints tenham um Collider. (Sphere Collider-melhor- ou qualquer outro).
//3. Selecione "is Trigger" nos objetos waypoint para detectar a colisao.
//4. Ajuste o tamanho desejado para a deteccao (raio do Sphere Collider ou tamanho do Box Collider).


public class WaypointManager : MonoBehaviour {
	//para ver no ambiente de cena
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 1.5f);
	}
	//*
	//atributos iniciais de cada waypoint
	public bool reduz; //determina se precisa diminuir a velocidade
	public float accel = 0.5f; //taxa de aceleracao	para vir do anterior (porcentagem da maxima do carro)
	public float giroNeeded; //velocidade que o objeto gira para o proximo waypoint
    public int prev;
    public Waypoint alvo;

    //classe de waypoint
    public class Waypoint {
        private bool desacelera; //necessidade de aceleracao para ir para o proximo waypoint (do atual para o proximo)
        private float agressividade; //quanto sera necessario girar o carro para chegar neste waypoint (a partir do anterior)
		private float aceleracao;
        public int prevIndex;

        //construtor
		public Waypoint(bool red, float giro, float acelNeeded) {
			this.desacelera = red;
            this.agressividade = giro;
			this.aceleracao = acelNeeded;
        }
        //metodos get
        public float getGiro() {
            return this.agressividade;
        }
		public float getAcel() {
			return this.aceleracao;
		}
		public bool getSlow() {
            return this.desacelera;
        }
        public int getPrev() {
            return this.prevIndex;
        }

        //para ajustes em tempo de execucao
        //metodos set
        public void setGiro(float giro) {
            this.agressividade = giro;
        }
		public void setAcel(float acel){
			this.aceleracao = acel;
		}
        public void setPrev(int prev) {
            this.prevIndex = prev;
        }
    }

	//executa uma unica vez antes de tudo
	void Start (){
        alvo = new Waypoint(reduz, giroNeeded, accel); //instancia o waypoint
	}
    void Update() {
        prev = alvo.prevIndex;
    }
   
	//A funcao "OnTriggerEnter" eh executada quando uma colisao ocorrer
	public void OnTriggerEnter (Collider other){
        if (other.gameObject.GetComponentInParent<IADriver>()) { //verifica se eh a IA que passa pelo Waypoint
            IADriver outro = other.gameObject.GetComponentInParent<IADriver>(); //atributos que guiam a IA
            if (other.CompareTag("CarBaseCollider")) {
                outro.WPindexPointer++;
                //reseta o indice
                if (outro.WPindexPointer == outro.waypoints.Length) {
                    outro.WPindexPointer = 0;
                }
                Debug.Log(string.Format("Next way point {0} at time {1}", outro.WPindexPointer, Time.time));
            }
        }
        

	} //fim de OnTriggerEnter()
}//fim de WaypointManager
