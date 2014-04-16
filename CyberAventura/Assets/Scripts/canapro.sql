-- phpMyAdmin SQL Dump
-- version 4.1.6
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 28-03-2014 a las 05:28:21
-- Versión del servidor: 5.6.16
-- Versión de PHP: 5.5.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de datos: `canapro`
--
CREATE DATABASE IF NOT EXISTS `canapro` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `canapro`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `bulk`
--

CREATE TABLE IF NOT EXISTS `bulk` (
  `IDRun` int(11) NOT NULL,
  `IDBulk` int(11) NOT NULL,
  `IDCuenta` int(11) NOT NULL,
  `Tiempo` float NOT NULL,
  `Puntuacion` int(100) NOT NULL,
  PRIMARY KEY (`IDRun`,`IDBulk`,`IDCuenta`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cuenta`
--

CREATE TABLE IF NOT EXISTS `cuenta` (
  `ID` int(10) NOT NULL AUTO_INCREMENT,
  `login` varchar(20) NOT NULL,
  `nombre` varchar(30) NOT NULL,
  `puntuacion` int(100) NOT NULL,
  `promediotiempo` float NOT NULL,
  `Rol` varchar(1) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `login` (`login`),
  FULLTEXT KEY `nombre` (`nombre`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=22 ;

--
-- Volcado de datos para la tabla `cuenta`
--

INSERT INTO `cuenta` (`ID`, `login`, `nombre`, `puntuacion`, `promediotiempo`, `Rol`) VALUES
(1, 'blah', 'leonardo', 78, 20, 'E'),
(2, 'blahblah', 'Chocaton', 90, 50, 'E'),
(3, '', 'El Ñerin', 30, 126, 'E'),
(4, '', 'Gil', 45, 54, 'A'),
(5, '', 'Gil', 45, 54, 'A'),
(6, '', 'Gil', 45, 54, 'A'),
(7, '', 'Gil', 45, 54, 'A'),
(8, '', 'Gil', 45, 54, 'A'),
(9, '', 'Gil', 45, 54, 'A'),
(10, '', 'Gil', 45, 54, 'A'),
(11, '', 'Gil', 45, 54, 'A'),
(12, '', 'Gil', 45, 54, 'A'),
(13, '', 'Gil', 45, 54, 'A'),
(14, '', 'Gil', 45, 54, 'A'),
(15, '', 'Gil', 45, 54, 'A'),
(16, '', 'Gil', 45, 54, 'A'),
(17, '', 'Gil', 45, 54, 'A'),
(18, '', 'Gil', 45, 54, 'A'),
(19, '', 'Gil', 45, 54, 'A'),
(20, '', 'Gil', 45, 54, 'A'),
(21, '', 'Armando', 100, 20, 'A');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `registro`
--

CREATE TABLE IF NOT EXISTS `registro` (
  `IDCuenta` int(11) NOT NULL,
  `IDPregunta` int(100) NOT NULL,
  `IDRespuesta` varchar(1) NOT NULL,
  `TiempoConsumido` float NOT NULL,
  `IDRun` int(11) NOT NULL,
  PRIMARY KEY (`IDCuenta`,`IDPregunta`,`IDRun`),
  KEY `IDPregunta` (`IDPregunta`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `run`
--

CREATE TABLE IF NOT EXISTS `run` (
  `IDCuenta` int(11) NOT NULL,
  `IDRun` int(11) NOT NULL,
  `Tiempo` float NOT NULL,
  `Puntuacion` int(100) NOT NULL,
  `completado` varchar(1) NOT NULL,
  PRIMARY KEY (`IDCuenta`,`IDRun`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `run`
--

INSERT INTO `run` (`IDCuenta`, `IDRun`, `Tiempo`, `Puntuacion`, `completado`) VALUES
(1, 1, 5.3, 9, 'S'),
(4, 1, 92, 58, 'S'),
(5, 1, 140, 68, 'S'),
(5, 2, 0, 0, 'N'),
(5, 3, 0, 0, 'N'),
(6, 1, 200, 95, 'S'),
(6, 2, 0, 0, 'N'),
(6, 3, 0, 0, 'N'),
(6, 4, 0, 0, 'N'),
(6, 5, 0, 0, 'S'),
(6, 6, 0, 0, 'S'),
(6, 7, 0, 0, 'N'),
(8, 1, 136.4, 75, 'S'),
(8, 2, 0, 0, 'S'),
(8, 3, 0, 0, 'S'),
(8, 4, 0, 0, 'N');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
