using UnityEngine;
using System.Collections;

public class IADriver : MonoBehaviour
{
    //atributos para a IA
    private Rigidbody m_Rigidbody; //rigidBody do veiculo
    private Car carro; //caracteristicas do carro
    private Transform prevWaypoint; //indica qual o ponto alvo anterior.
    private WaypointManager prevPointSpec; //especificacoes do ponto anterior (o ponto onde se encontra)
    private Transform waypoint; //indica qual o ponto alvo.
    private WaypointManager pointSpec; //especificacoes do ponto atual (para onde vai)
    private StartRace largada;

    public Transform helice, leme;

    public float dificuldade;
    public AudioSource efeitos;
    public Transform[] waypoints; //array de pontos de referencia para a IA
    public int WPindexPointer; //monitora qual Waypoint, no array "waypoints", eh a atual.
    public float accel; //taxa de aceleracao
    public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * 3.6f; } } //velocidade atual em Km/h
    public bool slow = false;
    public bool chegou;
    public float rotacao;
    public float newMaxSpeed;

    // Use this for initialization
    void Start()
    {
        m_Rigidbody = this.GetComponent<Rigidbody>(); // captura o rigidBody do veiculo
        carro = this.GetComponent<Car>(); //obtem as caracteristicas do carro
        WPindexPointer = 1; //primeiro ponto a ser seguido (apos a linha de largada)
        chegou = false;
        //pegando o script de largada da pista
        largada = GameObject.FindGameObjectWithTag("Track").GetComponent<StartRace>(); //gerenciamento da corrida
        efeitos = GetComponentInChildren<AudioSource>();
        efeitos.clip = GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.getFx(1); //seleciona o efeito
        //efeitos.volume = Settings.valSfxVolume;
        efeitos.volume = GameObject.Find("SoundManager").GetComponent<SoundManager>().musicas.volSFX;
        efeitos.Play();

        if (Settings.valDificuldade == 0){ //nivel facil
            dificuldade = 0.85f;
        }
        else if (Settings.valDificuldade == 1){ //nivel medio
            dificuldade = 1f;
        }
        else{ //nivel dificil
            dificuldade = 1.15f;
        }

        newMaxSpeed = carro.spec.GetMaxSpeed() * dificuldade;//apenas para debug

        //obtem a lista dos waypoints da pista
        Transform pontos = GameObject.FindGameObjectWithTag("Track").transform.Find("Waypoints"); //obtem o gameObject que contem os waypoints
        waypoints = new Transform[pontos.childCount]; //cria um array com todos os waypoints, na ordem que aparecem na hierarquia
        Debug.Log(string.Format("Waypoints encontrados: {0}", pontos.childCount));
        int i = 0;
        //percorre toda a hierarquia e popula o array com os waypoints
        foreach (Transform t in pontos.transform){
            waypoints[i++] = t;
            if (i - 2 == -1)
            {
                t.gameObject.GetComponent<WaypointManager>().alvo.setPrev(waypoints.Length - 1); //especificacoes do waypoint anterior
            }
            else
            {
                t.gameObject.GetComponent<WaypointManager>().alvo.setPrev(i - 2); //especificacoes do waypoint anterior
            }
        }
    } //fim de Start()

    // Update is called once per frame
    void Update()
    {
        waypoint = waypoints[WPindexPointer]; //mira no waypoint adequado
        pointSpec = waypoint.GetComponentInParent<WaypointManager>(); //especificacoes do waypoint
        prevWaypoint = waypoints[pointSpec.alvo.getPrev()];
        prevPointSpec = prevWaypoint.GetComponent<WaypointManager>(); //ponto atual (onde o carro esta no momento)

        rotacao = pointSpec.alvo.getGiro();
        slow = prevPointSpec.alvo.getSlow(); //para ver no inspector
        accel = pointSpec.alvo.getAcel() * carro.spec.GetMaxAcel(); //aceleracao necessaria para o ponto
        efeitos.pitch = 1.3f * accel; //altera a velocidade do efeito de acordo com a aceleracao, que varia entre 0 e 1

        if (slow)
        { //Se o waypoint atual precisar de desaceleracao
            float distance = Vector3.Distance(prevWaypoint.position, this.transform.position); //distancia ate o ponto
            //Debug.Log(string.Format("Distancia ao ponto {0}", distance)); //verificacoes
            //desacelera quando se aproximar do ponto definido
            if (distance <= prevPointSpec.GetComponentInParent<SphereCollider>().radius - 10)
            { //area de desaceleracao
              //while (distance <= prevPointSpec.GetComponentInParent<SphereCollider>().radius - 5){ //area de desaceleracao
                Slow();
                this.chegou = true; //desabilita a aceleracao
            }
            else
            {
                this.chegou = false; //habilita a aceleracao
            }
        }
        //Se o waypoint nao precisar de desaceleracao
        if (!chegou)
        {
            Accell();
        }
    } //fim de Update()
    //*/
    //acelera o veiculo
    private void Accell()
    {
        //Debug.Log(string.Format("Forca IA {0}", accel * carro.spec.GetPower()));
        if (largada.go)
        { //se a contagem da largada terminou
            if (waypoint)
            { //Se existir um waypoint
              //direciona para o waypoint adequado.
                Quaternion rotation = Quaternion.LookRotation(waypoint.position - transform.position);
                //faz uma mudanca de direcao suave, de acordo com o especificado no waypoint
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * pointSpec.alvo.getGiro());
            }
            m_Rigidbody.AddRelativeForce(Vector3.forward * accel * carro.spec.GetPower()); //aplica o valor do empuxo proporcional

            //quando a velocidade maxima foi atingida ou ultrapassada ...
            if (this.CurrentSpeed >= (carro.spec.GetMaxSpeed() * dificuldade))
            {
                // ... limita a velocidade ao maximo permitido
                m_Rigidbody.velocity = ((carro.spec.GetMaxSpeed() * dificuldade) / 3.6f) * m_Rigidbody.velocity.normalized; //limita a velocidade maxima
            }
        }

    } //fim de Accel()

    private void Slow()
    {
        Debug.Log("Reduzindo a velocidade...");
        //direciona para o waypoint adequado.
        Quaternion rotation = Quaternion.LookRotation(waypoint.position - transform.position);
        //faz uma mudanca de direcao suave, de acordo com o especificado no waypoint
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * pointSpec.alvo.getGiro());
        //Se atingir uma velocidade minima antes de sair da area de frenagem ...
        if (CurrentSpeed <= (carro.spec.GetMaxSpeed() * 0.5)) //determina a velocidade minima
        {
            Debug.Log("Aguardando reaceleracao...");
            StartCoroutine("delay"); //aguarda um tempo antes de voltar a acelerar
        }
    } //fim de Slow()

    //corrotina para voltar a acelerar
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1); //aguarda o tempo de "stopTime" antes de comecar a celerar para o proximo waypoint.
        Accell();
    }
    //*/
}
