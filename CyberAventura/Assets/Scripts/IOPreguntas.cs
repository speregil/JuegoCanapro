using UnityEngine;
using System.Collections;
using System.IO;

/*
 * Clase que permite acceder al texto de las preguntas guardadas en un archivo de texto
 * */
public class IOPreguntas{
	//------------------------------------------------------------------------------------------
	// Atributos
	//------------------------------------------------------------------------------------------

	private string 		archivo;		//Nombre del archivo que se va a cargar
	private int 		numPreguntas;	//Numero de preguntas en el archivo
	private Pregunta[]	listaPreguntas;	//Lista de las preguntas cargadas
	private Pregunta	preguntaActual;	//Pregunta de la que actualmente setiene informacion
	private int			indiceActual;	//Indice de la pregunta actual

	//------------------------------------------------------------------------------------------
	// Constructor
	//------------------------------------------------------------------------------------------

	public IOPreguntas(string nArchivo){
		archivo = nArchivo;
		CargarPreguntas();
		indiceActual = 0;
		preguntaActual = listaPreguntas[indiceActual];
	}

	// Parsea el archivo de texto y recupera las preguntas
	private void CargarPreguntas(){
		/*StreamReader sr = new StreamReader(Application.dataPath + "/Data/" + archivo + ".txt");
		string contenido  = sr.ReadToEnd();
		sr.Close();**/
		TextAsset request = (TextAsset)Resources.Load(archivo, typeof(TextAsset));
		string[] data = request.text.Split('\n');
		numPreguntas = System.Int32.Parse(data[0]);
		listaPreguntas = new Pregunta[numPreguntas];
		for(int i = 0; i < numPreguntas;i++){
			string[] dataPregunta = data[i+1].Split(';');
			Pregunta nueva = new Pregunta(System.Int32.Parse(dataPregunta[0]),dataPregunta[1],dataPregunta[2], dataPregunta[3],dataPregunta[4],dataPregunta[5],dataPregunta[6],float.Parse(dataPregunta[7]));
			listaPreguntas[i] = nueva;
		}
	}

	//-------------------------------------------------------------------------------------------
	// Metodos
	//-------------------------------------------------------------------------------------------

	// Metodos para recuperar la informacion dela pregunta actual
	public int DarID(){
		return preguntaActual.DarID();
	}

	public string DarTexto(){
		return preguntaActual.DarTexto();
	}

	public string DarOpcionA(){
		return preguntaActual.DarOpcionA();
	}

	public string DarOpcionB(){
		return preguntaActual.DarOpcionB();
	}

	public string DarOpcionC(){
		return preguntaActual.DarOpcionC();
	}

	public string DarOpcionD(){
		return preguntaActual.DarOpcionD();
	}

	public float DarTiempo(){
		return preguntaActual.DarTiempo();
	}

	public string DarRespuesta(){
		string opcion = preguntaActual.DarRespuesta();
		if(opcion.Equals("A"))
			return "A. " + preguntaActual.DarOpcionA();
		else if(opcion.Equals("B"))
			return "B. " + preguntaActual.DarOpcionB();
		else if(opcion.Equals("C"))
			return "C. " + preguntaActual.DarOpcionC();
		else
			return "D. " + preguntaActual.DarOpcionD();
	}

	//Cambia la pregunta actual a la siguiente en la lista y retorna si pudo realizar la operacion o no
	public bool AvanzarPregunta(){
		indiceActual++;
		if(indiceActual < numPreguntas){
			preguntaActual = listaPreguntas[indiceActual];
			return true;
		}
		return false;
	}

	//Valida una respuesta dada por el jugador
	public bool ValidarRespuesta(string Respuesta){
		return preguntaActual.ValidarRespuesta(Respuesta);
	}
}