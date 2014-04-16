using UnityEngine;
using System.Collections;

/*
 * Clase que modela una pregunta 
 * */
public class Pregunta{

	//---------------------------------------------------------------------------------------------
	// Constantes
	//---------------------------------------------------------------------------------------------

	public const string OPCIONA = "A";
	public const string OPCIONB = "B";
	public const string OPCIONC = "C";
	public const string OPCIOND = "D";

	//----------------------------------------------------------------------------------------------
	// Atributos
	//----------------------------------------------------------------------------------------------

	private int 	ID;				//Identificador de la pregunta
	private string 	textoPregunta; 	//Enunciado de la pregunta
									// Opciones de respuesta
	private string 	opcionA;
	private string 	opcionB;
	private string 	opcionC;
	private string 	opcionD;
	private string 	respuesta; 		//Respuesta a la pregunta
	private float	tiempo;			//Marca de tiempo para responder la pregunta

	//----------------------------------------------------------------------------------------------
	// Constructor
	//----------------------------------------------------------------------------------------------

	public Pregunta(int id, string texto, string opA, string opB, string opC, string opD, string nRespuesta, float nTiempo){
		ID = id;
		textoPregunta = texto;
		opcionA = opA;
		opcionB = opB;
		opcionC = opC;
		opcionD = opD;
		respuesta = nRespuesta;
		tiempo = nTiempo;
	}

	//----------------------------------------------------------------------------------------------
	// Metodos
	//----------------------------------------------------------------------------------------------

	// Getters
	public int DarID(){
		return ID;
	}

	public string DarTexto(){
		return textoPregunta;
	}

	public string DarOpcionA(){
		return opcionA;
	}

	public string DarOpcionB(){
		return opcionB;
	}

	public string DarOpcionC(){
		return opcionC;
	}

	public string DarOpcionD(){
		return opcionD;
	}

	public string DarRespuesta(){
		return respuesta;
	}

	public float DarTiempo(){
		return tiempo;
	}

	// Valida si el char pasado por parametro coincide con la respuesta correcta
	public bool ValidarRespuesta(string opcion){
		if(opcion.Equals(respuesta)){
			return true;
		}
		return false;
	}
}