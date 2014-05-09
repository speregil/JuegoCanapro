using UnityEngine;
using System.Collections;

/**
 * Clase que modela la informacion de ejecucion de un
 * Bulk de preguntas dentro del juego**/
public class Bulk{

	//---------------------------------------------------------------------------------
	// Atributos
	//---------------------------------------------------------------------------------

	private string 	ID;				//Cadena que identifica el bulk
	private int 	puntuacion;		//Total de preguntas correctas
	private float 	tiempoTotal;	//Total de tiempo empleado
	private	string respuestas;		//Cadena con la secuencia de respuestas dadas

	//---------------------------------------------------------------------------------
	// Constructor
	//---------------------------------------------------------------------------------

	public Bulk(string nID){
		ID = nID;
		puntuacion = 0;
		respuestas = "";
	}

	//---------------------------------------------------------------------------------
	// Metodos
	//---------------------------------------------------------------------------------

	/**
	 * Suma una respuesta correcta al marcador total
	 * */
	public void SumarPunto(){
		puntuacion++;
	}

	/**
	 * Asigna el tiempo total empleado
	 * */
	public void AsignarTiempo(float nTiempo){
		tiempoTotal = nTiempo;
	}

	/**
	 * Getters
	 * */
	public string DarID(){
		return ID;
	}

	public int DarPuntuacion(){
		return puntuacion;
	}

	public float DarTiempo(){
		return tiempoTotal;
	}

	public void concatenar(string a)
	{
		respuestas += a;
	}

	public string darRespuestas()
	{
		return respuestas;
	}
}