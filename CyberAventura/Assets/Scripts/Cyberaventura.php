<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>CyberAventura</title>
    <script type='text/javascript' src='https://ssl-webplayer.unity3d.com/download_webplayer-3.x/3.0/uo/jquery.min.js'></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<script type="text/javascript">
    
    <!--
		var unityObjectUrl = "http://webplayer.unity3d.com/download_webplayer-3.x/3.0/uo/UnityObject2.js";
		if (document.location.protocol == 'https:')
			unityObjectUrl = unityObjectUrl.replace("http://", "https://ssl-");
		document.write('<script type="text\/javascript" src="' + unityObjectUrl + '"><\/script>');
		-->


</script>
<title>CyberAventura</title>
<script type="text/javascript">
		<!--
			var config = {
				width: 1366, 
				height: 768,
				params: { enableDebugging:"0" }
				
			};
			var u = new UnityObject2(config);
                        
			jQuery(function() {

				var $missingScreen = jQuery("#unityPlayer").find(".missing");
				var $brokenScreen = jQuery("#unityPlayer").find(".broken");
				$missingScreen.hide();
				$brokenScreen.hide();
				
				u.observeProgress(function (progress) {
					switch(progress.pluginStatus) {
						case "broken":
							$brokenScreen.find("a").click(function (e) {
								e.stopPropagation();
								e.preventDefault();
								u.installPlugin();
								return false;
							});
							$brokenScreen.show();
						break;
						case "missing":
							$missingScreen.find("a").click(function (e) {
								e.stopPropagation();
								e.preventDefault();
								u.installPlugin();
								return false;
							});
							$missingScreen.show();
						break;
						case "installed":
							$missingScreen.remove();
						break;
						case "first":
						break;
					}
				});
				u.initPlugin(jQuery("#unityPlayer")[0], "canapro.unity3D");
			});
function login( arg, arg1)
{
    http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirLogin", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=login&datas="+arg+"&datass="+arg1+"", true);
    http.send(null);
    
}
function guardarcuestionario( IDBulk, IDCuenta, Puntuacion, Tiempo )
{
    http=new XMLHttpRequest();
    var params = "data=guardarcuestionario&datas="+IDBulk+"&datass="+IDCuenta+"&datasss="+Puntuacion+"&datassss="+Tiempo;
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirCuestionario", respuesta);
        }
        
    }
    http.open("POST", "http://50.62.166.14:81/script.php");
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    //http.setRequestHeader("Content-length", params.length);
    //http.setRequestHeader("Connection", "close");
    http.send(params);
}
function pediravance( arg)
{
    http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirAvance", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=pediravance&datas="+arg+"", false);
    http.send(null);
}
function pedirtop20()
{
    http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirtop20", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=pedirtop20", false);
    http.send(null);
}
function pedirpromdesvypor()
{
    http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirDesvy", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=pedirpromdesvypor", true);
    http.send(null);
}
function crearnuevorun( arg )
{
    http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirRun", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=crearnuevorun&datas="+arg+"", true);
    http.send(null);
}
function buscarparticipante( arg )
{
    http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirParticipante", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=buscarparticipante&datas="+arg+"", true);
    http.send(null);
}
function esprimerbulk( arg )
{
	http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirPrimerBulk", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=primerbulk&datas="+arg+"", true);
    http.send(null);
}
function guardarpregunta(arg)
{
	http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirPrimerBulk", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=guardarpregunta&datas="+arg+"", true);
    http.send(null)
}
function pedirpreguntas( IDCuenta )
{
    http=new XMLHttpRequest();
    var params = "data=pediravancepregunta&datas="+IDCuenta+"";
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirPreguntas", respuesta);
        }
        
    }
	console.log("parametros: "+params);
    http.open("POST", "http://50.62.166.14:81/script.php");
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    //http.setRequestHeader("Content-length", params.length);
    //http.setRequestHeader("Connection", "close");
    http.send(params);
}
function pedirconteo(arg)
{
	http=new XMLHttpRequest();
    http.onreadystatechange = function()
    {
        if (http.readyState===4 && http.status===200)
        {
            var respuesta = this.responseText;
            u.getUnity().SendMessage("AdminSQL", "recibirConteo", respuesta);
        }
        
    }
    http.open("GET", "http://50.62.166.14:81/script.php?data=pedirconteo", true);
    http.send(null)
}
function debug(arg)
{
	console.log(arg);
}
		-->
		</script>
<style type="text/css">
		<!--
		body {
			font-family: Helvetica, Verdana, Arial, sans-serif;
			background-color: white;
			color: black;
			text-align: center;
		}
		a:link, a:visited {
			color: #000;
		}
		a:active, a:hover {
			color: #666;
		}
		p.header {
			font-size: small;
		}
		p.header span {
			font-weight: bold;
		}
		p.footer {
			font-size: x-small;
		}
		div.content {
			margin: auto;
			width: 1366px;
		}
		div.broken,
		div.missing {
			margin: auto;
			position: relative;
			top: 50%;
			width: 193px;
		}
		div.broken a,
		div.missing a {
			height: 63px;
			position: relative;
			top: -31px;
		}
		div.broken img,
		div.missing img {
			border-width: 0px;
		}
		div.broken {
			display: none;
		}
		div#unityPlayer {
			cursor: default;
			height: 768px;
			width: 1366px;
		}
		-->
		</style>

</head>
    	<body>
		<p class="header"><span>Unity Web Player | </span>CyberAventura</p>
		<div class="content">
			<div id="unityPlayer">
				<div class="missing">
					<a href="http://unity3d.com/webplayer/" title="Unity Web Player. Install now!">
						<img alt="Unity Web Player. Install now!" src="http://webplayer.unity3d.com/installation/getunity.png" width="193" height="63" />
					</a>
				</div>
				<div class="broken">
					<a href="http://unity3d.com/webplayer/" title="Unity Web Player. Install now! Restart your browser after install.">
						<img alt="Unity Web Player. Install now! Restart your browser after install." src="http://webplayer.unity3d.com/installation/getunityrestart.png" width="193" height="63" />
					</a>
				</div>
			</div>
		</div>
		<p class="footer">&laquo; created with <a href="http://unity3d.com/unity/" title="Go to unity3d.com">Unity</a> &raquo;</p>
	</body>
</html>
