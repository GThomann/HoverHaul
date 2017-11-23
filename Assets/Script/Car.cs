using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
    //atributos iniciais do carro
    public int potencia;
    public float velMax;
    public float angulo;
	public float maxAcel;

    public CarSpecs spec; //instancia de CarSpec

    public class CarSpecs {
        private int empuxo; //valor da potencia util do veiculo
        private float maxSpeed; //velocidade maxima permitida
        private float driveAngle; //angulo maximo de direcao
		private float aceleraMax; //maxima aceleracao possivel

        //construtor
		public CarSpecs(int empuxo, float maxSpeed, float angle, float acelera) {
			this.empuxo = empuxo;
            this.maxSpeed = maxSpeed;
            this.driveAngle = angle;
			this.aceleraMax = acelera;
        }

        //metodos get 
        public float GetAngle(){
            return this.driveAngle;
        }
        public float GetMaxSpeed() {
            return this.maxSpeed;
        }
		public float GetMaxAcel() {
			return this.aceleraMax;
		}
		public int GetPower() {
            return this.empuxo;
        }
        //metodos set
        public void SetMaxSpeed(float speed) {
            this.maxSpeed = speed;
        }
	}
	
	// Use this for initialization
	void Start () {
        spec = new CarSpecs(potencia, velMax, angulo, maxAcel);
	}

    //para debug
    void Update() {
        spec.SetMaxSpeed(velMax);
    }
}
