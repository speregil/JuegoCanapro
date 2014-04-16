using UnityEngine;
using System.Collections;

/*
 * Detecta acciones en los iconos de los edificios
 * */
public class OnClick : MonoBehaviour {

	//----------------------------------------------------------------------------
	// Atributos
	//----------------------------------------------------------------------------

	public 	GameObject 	Admin;		//Conexion con el administrador del mapa. Asignar en Inspector
	public 	string		ID;			//Identificador del grupo de preguntas que este edificio carga 
	private AdminNivel 	adminNivel;	//Referencia al script de admnistracion

	//----------------------------------------------------------------------------
	// Constructor
	//----------------------------------------------------------------------------

	void Start () {
		adminNivel = (AdminNivel)Admin.GetComponent(typeof(AdminNivel));
	}
	
	void Update () {

	}

	// Detecta el click en el icono del edificio e inicia el movimieto del personaje
	// hacia el
	void OnMouseDown(){
		adminNivel.nuevoDestino(transform.position, ID);
	} 
}