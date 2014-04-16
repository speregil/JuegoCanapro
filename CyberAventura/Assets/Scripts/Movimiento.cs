using UnityEngine;
using System.Collections;

/*
 * Mueve al personaje por el mapa
 * */
public class Movimiento : MonoBehaviour {

	//-----------------------------------------------------------------------------------
	// Atributos
	//-----------------------------------------------------------------------------------

	public	GameObject	Nivel;			// Conexion con el administrador del nivel
	public 	float 		velocidad; 		// Velocidad de movimiento del personaje. Asignar en inspector
	private	AdminNivel	admin;			// Referencia la script del administrador
	private Vector3 	posActual; 		// Posicion actual del personaje
	private Vector3 	posDestino;		// Posicion destino de un edificio
	private bool		enMovimiento;	// Define si el personje se esta moviendo o no
	private bool		enPosicion;		// Define si el personaje esta en la posicion destino


	//-----------------------------------------------------------------------------------
	// Constructor
	//-----------------------------------------------------------------------------------

	void Start () {
		admin = (AdminNivel)Nivel.GetComponent(typeof(AdminNivel));
		posDestino = transform.position;
		enMovimiento = false;
		enPosicion = false;
	}

	//-----------------------------------------------------------------------------------
	// Metodos
	//-----------------------------------------------------------------------------------

	void Update () {
		if(enMovimiento){
			posActual = transform.position;
			transform.position = Vector3.MoveTowards(posActual,posDestino,velocidad*Time.deltaTime);
		}

		if(posActual.Equals(posDestino)){
			if(!enPosicion){
				enMovimiento = false;
				admin.IniciarPreguntas();
				enPosicion = true;
			}
		}
	}

	// Fija una posicion destino e inicia el movimiento
	public void IniciarMovimiento(Vector3 destino){
		posActual = transform.position;
		posDestino = destino;
		enMovimiento = true;
		enPosicion = false;
	}
}