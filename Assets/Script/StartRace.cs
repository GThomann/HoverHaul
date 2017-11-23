using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartRace : MonoBehaviour {
    public int timeLeft = 6; //segundos para iniciar a corrida
    public bool go;
    public Text info;
    public Text conta;
    public Text txtVoltas;

	// Use this for initialization
	void Start () {
        go = false;
        StartCoroutine("countDown", timeLeft); //inicia a contagem para a largada
        txtVoltas.text = "1/" + Settings.numVoltas.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (timeLeft < 0) {
            info.enabled = false;//remove a informacao da largada, mas mantem o texto
            StopAllCoroutines();//para todas as corrotinas
        }
        if (timeLeft == 0) {
            go = true;
            //mensagem acelerar
            //info.color = Color.green;
            //info.text = "Acelerar!";
            ChangeText(info, "Acelerar!", new Color(0f, 1f, 0f, 1f)); //cor RGBA
            conta.enabled = false;//remove a indicacao da contagem, mas mantem o texto
        }
        else {
            
            if (timeLeft >= 4) {
                //mensagem ligar motores
                //info.color = Color.red;
                //info.text = "Atenção!";
                ChangeText(info, "Atenção!", new Color(1f, 0.02f, 0f, 1f)); //cor RGBA
            }
            else {
                //mensagem preparar
                //info.color = Color.yellow;
                //info.text = "Em seus postos!";
                ChangeText(info, "Em seus postos!", new Color(1f, 0.92f, 0.01f, 1f)); //cor RGBA
            }
        }
	}

    //corrotina para contar o tempo
    IEnumerator countDown(int time) {
        Color cor = new Color(1, 0, 0, 1); //cor inicial RGBA
        while (time >= -1) {
            if (timeLeft >= 4) {
                cor = new Color(1f, 0.4f, 0.03f, 1);
            }
            else if (timeLeft <= 1) {
                cor = new Color(0.01f, 0.9f, 0.1f, 1f);
            }
            else {
                cor = new Color(0.9f, 0.8f, 0.07f, 1f);
            }
            ChangeText(conta, timeLeft.ToString(), cor);
            //conta.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
    }

    private IEnumerator Countdown2(int time) {
        while (time >= 0) {
            Debug.Log(time--);
            timeLeft--;
            yield return new WaitForSeconds(1);
        }
        //Debug.Log("Countdown Complete!");
    }

    void ChangeText(Text texto, string message, Color cor) {
        texto.color = cor;
        texto.text = message;
    }
}
