using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput; //usa os inputs padroes

public class UserInput : MonoBehaviour
{
    //atributos publicos
    public float altMax; //distancia maxima permitida do terreno
    public float torque; //valor da forca para alterar a direçao
    public float AccelInput { get; private set; } //propriedade que torna o atributo acessivel fora da classe (aceleracao)
    public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 3.6f; } } //velocidade atual em Km/h
    public AudioSource efeitos;
    public AudioSource music;
    public Transform helice, leme;

    //atributos privados
    private Rigidbody m_Rigidbody; //rigidBody do veiculo
    private Car carro; // caracteristicas do carro
    private StartRace largada;

    // Inicializacoes
    void Start()
    {
        //troca amusica de fundo
        music = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        music.clip = GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.getMusic("Menu", Random.Range(0, GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.getTotalMusic("Menu")));
        music.Play(); //toca a musica selecionada

        m_Rigidbody = GetComponent<Rigidbody>(); // obtem o rigidBody do veiculo
        carro = GetComponent<Car>(); // obtem as caracteristicas do carro
        //pegando o script de largada da pista
        largada = GameObject.FindGameObjectWithTag("Track").GetComponent<StartRace>(); //gerenciamento da corrida
        efeitos = GetComponentInChildren<AudioSource>(); //obtem a fonte do audio
        efeitos.clip = GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.getFx(0); //seleciona o efeito
        efeitos.volume = GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.volSFX;
        efeitos.Play();
    }

    // executa uma vez por frame
    void Update()
    {
		//*
        // passa o comando para o veiculo do teclado
        float virar = CrossPlatformInputManager.GetAxis("Horizontal"); //eixo de direcao
        float acelerar = CrossPlatformInputManager.GetAxis("Vertical"); //eixo de aceleracao
		//*/

        /*
        //comandos no controle
        float virar, acelerar;

        //verificar e remover assim que acertar o lado do controle analogico
        if (Input.GetAxis("Axis 1") > 0f){
            virar = 1;
        }
        else if (Input.GetAxis("Axis 1") < 0f){
            virar = -1;
            }
            else{
                virar = 0;
            }

        //virar = Input.GetAxis("Axis 1"); //comando do controle analogico
        acelerar = Input.GetKey(KeyCode.JoystickButton0) ? 1 : 0;
        //*/
        if ((Input.GetKey(KeyCode.JoystickButton7) || Input.GetKey(KeyCode.Escape)) == true && Time.timeScale == 1){
            Time.timeScale = 0;
        }

        efeitos.pitch = acelerar + 0.3f;
        helice.transform.Rotate(360 * Vector3.right * Time.deltaTime * (9 * acelerar + 1)); //visual da rotacao da helice
        //leme.transform.Rotate(Vector3.forward * virar * carro.spec.GetAngle()); //visual da inclinacao do leme

        if (largada.go) { //se a contagem da largada terminou
            //chama a funcao move para movimentar o veiculo
            Move(virar, acelerar); //verificar a aplicacao do freio
        }
        //se nao estiver tocando, troca a musica e comeca a tocar
        if (!music.isPlaying) {
            music.clip = GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.getMusic("Menu", Random.Range(0, GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.getTotalMusic("Menu")));
            music.Play(); //toca a musica selecionada
        }
    } //fim de Update()

    // estabelece os valores de aceleracao e rotacao para a movimentacao do veiculo
    public void Move(float steering, float accel){
		//limita os valores de entrada
		steering = Mathf.Clamp(steering, -1, 1); //limita os valores possiveis entre -1 e 1 (esquerda e direita)
		accel = Mathf.Clamp(accel, 0, 1); //limita os valores possiveis entre 0 e 1 (stick para cima)
		AccelInput = accel; //
        torque = Mathf.Sin(steering * carro.spec.GetAngle()) * carro.spec.GetPower(); //forca para rotacionar o veiculo independe da aceleracao

        ApplyForce(accel, torque); //acelera o veiculo
	} //fim de Move()

    //calcula e aplica a velocidade maxima e a rotacao do veiculo
    private void ApplyForce(float acelera, float giro){
        //variaveis locais
        RaycastHit altura; //raio que vai determinar a altura do carro em relacao ao terreno
        Physics.Raycast(this.transform.position, -Vector3.up, out altura); //faz a projecao e identifica onde o raio se chocou (para baixo do carro)
        //Debug.Log(string.Format("Altura em relacao ao terreno {0}", altura.distance));
        if (altura.distance >= altMax) { //esta iniciando a decolagem
            Debug.Log(string.Format("Altura em relacao ao terreno {0}: Reduzindo ação dos comandos.", altura.distance));
            // acelera aplicando o valor do empuxo proporcional a posicao do stick suavizado para manter o controle do carro
            m_Rigidbody.AddRelativeForce(Vector3.forward * (acelera * 0.3f) * carro.spec.GetPower());
            //rotacionar o veiculo reduzindo o torque para nao perder o controle do veiculo
            m_Rigidbody.AddRelativeTorque(Vector3.up * (giro * 0.8f));
        }
        else {
            m_Rigidbody.AddRelativeForce(Vector3.forward * acelera * carro.spec.GetPower()); //aplica o valor do empuxo proporcional a posicao do stick
            //rotacionar o veiculo
            m_Rigidbody.AddRelativeTorque(Vector3.up * giro);
        }

        //verifica se atingiu a velocidade maxima
        if (CurrentSpeed > carro.spec.GetMaxSpeed())
        {
            m_Rigidbody.velocity = (carro.spec.GetMaxSpeed() / 3.6f) * m_Rigidbody.velocity.normalized; //limita a velocidade maxima
        }
    } //fim de ApplyForce()
} //fim da classe UserInput
