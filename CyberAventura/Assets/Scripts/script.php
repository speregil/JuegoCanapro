<?php
    
    if(isset($_GET["data"]))
    {
		$yeah = $_GET["data"];
    }
	else
	{
		$yeah = $_POST["data"];
	}
    //echo $yeah;
    $connection = mysqli_connect("localhost", "root", "", "canapro",3306);
    mysqli_autocommit($connection, FALSE);
    if($yeah === "login")
    {
		$id = $_GET["datas"];
		$pas = $_GET["datass"];
		$servicio="http://190.144.186.146/Aqueron-WServices/wsTarjeta.asmx?wsdl"; //url del servicio
		$parametros=array(); //parametros de la llamada
		$parametros['Identificacion']=$id;
		$parametros['Clave']=$pas;
		$parametros['Id_Sistema']="100202";
		$parametros['Token']="HihjELIX63523782177347c52416754f3e64Atamai";
		try {  
			$client = new SoapClient($servicio);
			$result = $client->Consultar_Estado_Cliente($parametros);
		} catch(Exception $e)
		{
			echo $e->getMessage();
		}
		$result = obj2array($result);
		$respuesta = $result['Consultar_Estado_ClienteResult'];
		$cadena = split(",", $respuesta);
		$respuesta = "";
		//echo $cadena[0].";";
		//echo $cadena[1].";";
		if($cadena[0] === "0")
		{
			$query = "SELECT ID, LOGIN, NOMBRE FROM CUENTA WHERE NOMBRE = '".$cadena[1]."'";
			$resultado = mysqli_query($connection, $query);
			if(mysqli_num_rows($resultado) > 0)
			{
				while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
				{
					$respuesta .= $numero[0];
				}
				mysqli_close($connection);
				//echo "paso en la primera;";
				echo $respuesta;
			}
			else
			{
				$query = "INSERT INTO CUENTA (LOGIN, NOMBRE, PUNTUACION, PROMEDIOTIEMPO, ROL) VALUES('".$id."', '".$cadena[1]."', 0, 0, 'E')";
				$resultado = mysqli_query($connection, $query);
				mysqli_commit($connection);
				$query = "SELECT ID, LOGIN, NOMBRE FROM CUENTA WHERE NOMBRE = '".$cadena[1]."'";
				$resultado = mysqli_query($connection, $query);
				while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
				{
					$respuesta .= $numero[0];
				}
				mysqli_close($connection);
				//echo "paso en la segunda;";
				echo $respuesta;
			}
			exit(0);
		}
		else
		{
			echo $cadena[0];
			exit(0);
		}
    }
    //Hecho y sin probar
    else if($yeah === "guardarcuestionario")
    {
        $bulk = $_POST["datas"];
        $cuenta = $_POST["datass"];
        $puntuacion = $_POST["datasss"];
        $tiempo = $_POST["datassss"];
        $query = "SELECT IDRUN, PUNTUACION, TIEMPO FROM RUN WHERE IDCUENTA = ".$cuenta." ORDER BY IDRUN DESC LIMIT 1";
        $resultado = mysqli_query($connection, $query);
        if(!$resultado)
        {
            mysqli_close($connection);
            echo "Fracaso";
			exit(0);
        }
        $numero = mysqli_fetch_array($resultado, MYSQL_NUM);
        $IDRun = $numero[0];
        $puntuacionRun = $numero[1];
        $puntuacionRun += $puntuacion;
        $tiempoRun = $numero[2];
        $tiempoRun += $tiempo;
        $query = "UPDATE RUN SET PUNTUACION =".$puntuacionRun.", TIEMPO = ".$tiempoRun." WHERE IDCUENTA = ".$cuenta." AND IDRUN = ".$IDRun;
        $resultado = mysqli_query($connection, $query);
        if(!$resultado)
        {
            mysqli_close($connection);
            echo "Fracaso";
			exit(0);
        }
        if($IDRun === "1")
        {
            $query = "UPDATE CUENTA SET PUNTUACION = ".$puntuacionRun.", PROMEDIOTIEMPO = ".$tiempoRun." WHERE ID = ".$cuenta;
            $resultado = mysqli_query($connection, $query);
            if(!$resultado)
            {
                mysqli_close($connection);
                echo "Fracaso";
				exit(0);
            }
        }
        
        $query = "INSERT INTO BULK (IDRUN, IDBULK, IDCUENTA, TIEMPO, PUNTUACION) VALUES (".$IDRun.", ".$bulk.", ".$cuenta.", ".$tiempo.", ".$puntuacion.")";
        $resultado = mysqli_query($connection, $query);
        if(!$resultado)
        {
            //$arreglo = split(";", $formato);
            //$cantidad = count($arreglo);
            //$indice = 0;
            //while($cantidad > $indice )
            //{
            //    $query = "INSERT INTO REGISTRO (IDCUENTA, IDPREGUNTA, IDRESPUESTA, TIEMPOCONSUMIDO, IDRUN) VALUES (".$cuenta.",".$arreglo[$indice].", '".$arreglo[$indice+1]."', ".$arreglo[$indice+2].", ".$IDRun.")";
			//	$resultado = mysqli_query($connection, $query);
            //    $indice += 3;
            //    if(!$resultado)
            //   {
            //        //mysqli_close($connection);
					mysqli_close($connection);
                    echo "Fracaso";
					exit(0);
            //    }
            //}
        }
        else
        {
            mysqli_commit($connection);
            echo "Exito";
			exit(0);
        }
    }
    //Hecho y sin probar
    else if($yeah === "pediravance")
    {
        $cuenta = $_GET["datas"];
        $query = "SELECT IDRUN, PUNTUACION, TIEMPO FROM RUN WHERE IDCUENTA = ".$cuenta." ORDER BY IDRUN ASC LIMIT 1";
        $resultado = mysqli_query($connection, $query);
        $numero = mysqli_fetch_array($resultado, MYSQL_NUM);
        $IDRun = $numero[0];
        $puntuacionActual = $numero[1];
        $tiempoActual = $numero[2];
        
        $query = "SELECT LOGIN, NOMBRE, PUNTUACION, PROMEDIOTIEMPO FROM CUENTA WHERE ID = ".$cuenta."";
        $resultado = mysqli_query($connection, $query);
        $numero = mysqli_fetch_array($resultado, MYSQL_NUM);
        $login = $numero[0];
        $nombre = $numero[1];
        $puntuacion = $numero[2];
        $promedioTiempo = $numero[3];
        
        $query = "SELECT IDBULK, TIEMPO, PUNTUACION FROM BULK WHERE IDRUN = ".$IDRun." AND IDCUENTA = ".$cuenta." ORDER BY IDBULK DESC";
        $resultado = mysqli_query($connection, $query);
        $formato = $nombre.";".$puntuacion.";".$promedioTiempo.";".$IDRun.";".$tiempoActual.";".$puntuacionActual;
        $formato .= ";bulks";
		if($resultado)
		{
			while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
			{
				$formato .= ";".$numero[0].";".$numero[1].";".$numero[2];
			}
		}
        echo $formato;
    }
    //Hecho
    else if($yeah === "pedirtop20")
    {
        $query = "SELECT ID, LOGIN, NOMBRE, PUNTUACION FROM CUENTA ORDER BY PUNTUACION DESC";
        $resultado = mysqli_query($connection, $query);
        $respuesta = "top20";
        while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
        {
            $respuesta .= ";".$numero[0].";".$numero[1].";".$numero[2].";".$numero[3];
        }
        //mysql_close($connection);
        echo $respuesta;
    }
    //Hecho
    else if($yeah === "pedirpromdesvypor")
    {
        $query = "SELECT PUNTUACION, TIEMPO, COMPLETADO FROM RUN WHERE IDRUN = 1";
        $resultado = mysqli_query($connection, $query);
        $puntuaciones = 0.0;
        $tiempos = 0.0;
        $porcentaje = 0.0;
        $cantidad = 0.0;
        $valoresT = array();
        $valoresP = array();
        while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
        {
            if($numero[2] === "S")
            {
                $puntuaciones += $numero[0];
                $valoresP[] = $numero[0];
                $tiempos += $numero[1];
                $valoresT[] = $numero[1];
                $porcentaje++;
            }
            $cantidad++;
        }
        $puntuaciones = $puntuaciones/$porcentaje;
        $tiempos = $tiempos/$porcentaje;
        $numeroS = $porcentaje;
        $porcentaje = $porcentaje/$cantidad*100;
        
        //desviacion estandar T
        $sumaCuadrados = 0;
        foreach($valoresT as $valor)
        {
            $sumaCuadrados += pow(($valor-$puntuaciones),2);
        }
        $desviacionT = sqrt(($sumaCuadrados/$numeroS));
        
        //desviacion estandar P
        $sumaCuadrados = 0;
        foreach($valoresP as $valor)
        {
            $sumaCuadrados += pow(($valor-$tiempos),2);
        }
        $desviacionP = pow(($sumaCuadrados/$numeroS), 0.5);
        
        echo ("pedirpromdesvypor;". $puntuaciones.";".$desviacionP.";".$tiempos.";".$desviacionT.";".$porcentaje.";".$cantidad);
        mysqli_close($connection);
    }
    //Hecho
    else if($yeah === "crearnuevorun")
    {
        $yeah = $_GET["datas"];
        $q = "SELECT IDRUN FROM RUN WHERE IDCUENTA = '".$yeah."' ORDER BY IDRUN DESC LIMIT 1";
        $resultado = mysqli_query($connection, $q);
        $numero = mysqli_fetch_array($resultado, MYSQL_NUM);
        if (mysqli_num_rows($resultado) > 0)
        {
            $query = "UPDATE RUN SET COMPLETADO = 'S' WHERE IDRUN = ".$numero[0]." AND IDCUENTA = ".$yeah."";
            $resultado = mysqli_query($connection, $query);
            if ($resultado)
            {
                $id_run = $numero[0]+1;
                $query = "INSERT INTO RUN (IDCUENTA, IDRUN, TIEMPO, PUNTUACION, COMPLETADO) VALUES (".$yeah.",".$id_run.", 0, 0, 'N')";
                $resultado = mysqli_query($connection, $query);
                if ($resultado)
                {
                    mysqli_commit($connection);
                    echo "Exito";
                }
                else
                {
                    echo "Fracaso";
                }
            }
            else
            {
                echo "Fracaso";
            }
        }
        else
        {
            $id_run = 1;
            $query = "INSERT INTO RUN (IDCUENTA, IDRUN, TIEMPO, PUNTUACION, COMPLETADO) VALUES (".$yeah.",".$id_run.", 0, 0, 'N')";
            $resultado = mysqli_query($connection, $query);
            if ($resultado)
            {
                mysqli_commit($connection);
                echo "Exito";
            }
            else
            {
                echo "Fracaso";
            }
        }
        mysqli_close($connection);
    }
    //Hecho
    else if($yeah === "buscarparticipante")
    {
        $yeah = $_GET["datas"];
        $query = "SELECT ID, NOMBRE, LOGIN, PUNTUACION, ROL FROM CUENTA WHERE LOGIN = '".$yeah."'";
        $resultado = mysqli_query($connection, $query);
        $respuesta = "buscarparticipante";
        while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
        {
            $respuesta .= ";".$numero[0].";".$numero[1].";".$numero[2].";".$numero[3].";".$numero[4];
        }
        mysqli_close($connection);
        echo $respuesta;
    }
	else if($yeah === "primerbulk")
	{
		$yeah = $_GET["datas"];
		$query = "SELECT IDRUN FROM BULK WHERE IDCUENTA = ".$yeah."";
        $resultado = mysqli_query($connection, $query);
        if(mysqli_num_rows($resultado) > 0)
		{
			echo "no";
		}
		else
		{
			$query = "SELECT IDCUENTA FROM RUN WHERE IDCUENTA = ".$yeah."";
			$resultado = mysqli_query($connection, $query);
			if(mysqli_num_rows($resultado) > 0)
			{
				echo "si";
			}
			echo "si";
		}
	}
	else if($yeah === "guardarpregunta")
	{
		$yeah = $_GET["datas"];
		$arreglo = split(";", $yeah);
        $cantidad = count($arreglo);
            $query = "INSERT INTO REGISTRO (IDCUENTA, IDPREGUNTA, IDRESPUESTA, TIEMPOCONSUMIDO, IDRUN) VALUES (".$arreglo[0].",".$arreglo[1].", '".$arreglo[2]."', ".$arreglo[3].", 1)";
			$resultado = mysqli_query($connection, $query);
            if(!$resultado)
            {
                    //mysqli_close($connection);
                    echo "Fracaso";
					exit(0);
            }
         mysqli_commit($connection);
         echo "Exito";
		 exit(0);
	}
	else if($yeah === "pediravancepregunta")
	{
		$IDCUENTA = $_POST["datas"];
		$query = "SELECT IDPREGUNTA FROM REGISTRO WHERE IDCUENTA = ".$IDCUENTA;
		$resultado = mysqli_query($connection, $query);
		$respuesta = "";
		if(mysqli_num_rows($resultado) > 0)
		{
			while($numero = mysqli_fetch_array($resultado, MYSQL_NUM))
			{
				$respuesta .= $numero[0].";";
			}
			mysqli_close($connection);
			echo $respuesta;
			exit(0);
		}
	}
	function obj2array($obj) {
	  $out = array();
	  foreach ($obj as $key => $val) {
		switch(true) {
			case is_object($val):
			 $out[$key] = obj2array($val);
			 break;
		  case is_array($val):
			 $out[$key] = obj2array($val);
			 break;
		  default:
			$out[$key] = $val;
		}
	  }
	  return $out;
	}
			