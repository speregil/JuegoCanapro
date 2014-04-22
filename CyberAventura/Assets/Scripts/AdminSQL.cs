using UnityEngine;
using System.Collections;

public class AdminSQL : MonoBehaviour {

	public 	static string 	LOGOUT = "LogOut";
	private CRUD			ConexionBD;
	private string 			UsuarioActual;
	private string			LlaveUsuario;
	private Run 			RunActual;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start () {
		UsuarioActual = LOGOUT;
		ConexionBD = (CRUD)GetComponent(typeof(CRUD));
	}

	public string DarUsuarioActual(){
		return UsuarioActual;
	}

	public Run DarRunActual(){
		return RunActual;
	}

	public string LogIn(string User, string Password){
		ConexionBD.hacerLogin(User, Password);
		LlaveUsuario = ConexionBD.login;// Hay que parcear esta informacion
		// Prueba dummy para la presentacion
		if(User.Equals("123")){
			UsuarioActual = "Alexis";
			return "exito";
		}
		else if(User.Equals("456")){
			UsuarioActual = "Sonia";
			return "mora";
		}

		return "fallo";
	}

	public void LogOut(){
		UsuarioActual = LOGOUT;
	}

	public void NuevoRun(){
		ConexionBD.crearNuevoRun(int.Parse(LlaveUsuario));
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