using UnityEngine;
using System.Collections;

/**
 * Clase que modela un Run del juego del usuario actual
 * */
public class Run{

	//--------------------------------------------------------------------------------------------
	// Atributos
	//--------------------------------------------------------------------------------------------

	public bool[] 	flagBulks;			//Arreglo de booleanos que modelan el estado de todos los bulks, true si ya se completaron
	public int		puntuacionTotal;	//Numero de preguntas correctas
	public float	tiempoTotal;		//Tiempo total empleado

	//--------------------------------------------------------------------------------------------
	// Constructor
	//--------------------------------------------------------------------------------------------

	public Run(){
		flagBulks = new bool[12];
		puntuacionTotal = 0;
		tiempoTotal = 0.0f;
	}

	public Run(bool[] flagsParcial, int puntosParcial, float tiempoParcial){
		flagBulks = flagsParcial;
		puntuacionTotal = puntosParcial;
		tiempoTotal = tiempoParcial;
	}

	//-------------------------------------------------------------------------------------------
	// Getters
	//-------------------------------------------------------------------------------------------

	public bool[] DarFlags(){
		return flagBulks;
	}

	public bool DarFlag(int idFlag){
		if(idFlag >= 0 && idFlag <= flagBulks.Length)
			return flagBulks[idFlag];
		else
			return false;
	}

	public int DarPuntuacionTotal(){
		return puntuacionTotal;
	}

	public float DarTiempoTotal(){
		return tiempoTotal;
	}

	public int DarBulksCompletos(){
		int total = 0;
		for(int i = 0; i < flagBulks.Length; i++){
			if(flagBulks[i])
				total++;
		}
		return total;
	}

	//-------------------------------------------------------------------------------
	// Setters
	//-------------------------------------------------------------------------------

	public void SumarPuntos(int puntos){
		puntuacionTotal+=puntos;
	}
	
	public void SumarTiempo(float tiempo){
		tiempoTotal+= tiempo;
	}

	public void completarBulk(int idBulk){
		if(idBulk >= 0 && idBulk <= flagBulks.Length){
			flagBulks[idBulk] = true;
		}
	}
}