using UnityEngine;
using System.Collections;

public class AdminSQL : MonoBehaviour {

	public 	static string 	LOGOUT = "LogOut";
	private string 			UsuarioActual;
	private Run 			RunActual;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start () {
		UsuarioActual = LOGOUT;
	}

	public string DarUsuarioActual(){
		return UsuarioActual;
	}

	public Run DarRunActual(){
		return RunActual;
	}

	public void LogIn(string User, string Password){
		// Llamados para hacer el login
		UsuarioActual = "Mario";
	}

	public void LogOut(){
		UsuarioActual = LOGOUT;
	}

	public void NuevoRun(){
		// Crear y guardar un nuevo run, guardar el identificador en la 
		// variable respectiva
		RunActual = new Run();
	}

	public void CargarRun(string IDCuenta){
		// Cargar los datos del run de la bd
		bool[] flag = new bool[12];
		flag[0] = true;
		flag[1] = true;
		flag[2] = true;
		flag[3] = true;
		flag[4] = false;
		flag[5] = false;
		flag[6] = false;
		flag[7] = false;
		flag[8] = false;
		flag[9] = false;
		flag[10] = false;
		flag[11] = false;
		RunActual = new Run(flag,32,8.2f);
	}
}