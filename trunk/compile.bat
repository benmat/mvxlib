::
:: MvxLib, an open source C# library used for communication with Intentia Movex.
:: http://mvxlib.sourceforge.net
::
:: Copyright (C) 2005 - 2006  Mattias Bengtsson
::
:: This library is free software; you can redistribute it and/or
:: modify it under the terms of the GNU Lesser General Public
:: License as published by the Free Software Foundation; either
:: version 2.1 of the License, or (at your option) any later version.
::
:: This library is distributed in the hope that it will be useful,
:: but WITHOUT ANY WARRANTY; without even the implied warranty of
:: MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
:: Lesser General Public License for more details.
::
:: You should have received a copy of the GNU Lesser General Public
:: License along with this library; if not, write to the Free Software
:: Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
::

@ECHO OFF

IF "%1" == "11" GOTO set_11		rem Ms .NET 1.1
IF "%1" == "20" GOTO set_20		rem Ms .NET 2.0
IF "%1" == "mono" GOTO set_mono	rem Mono

ECHO  Compiler argument empty or incorrect (remember to use lowercase).
ECHO  Use 'compile arg' where arg can be:
ECHO  	'11'	for Microsoft .NET 1.1
ECHO  	'20'	for Microsoft .NET 2.0
ECHO  	'mono'	for Mono
ECHO.

GOTO set_20

:set_11
ECHO  Using .Net Framework 1.1 compiler.
SET DOTNET_VERSION=v1.1.4322
SET DEFINE=NET_1_1
GOTO endset

:set_20
ECHO  Using .Net Framework 2.0 compiler.
SET DOTNET_VERSION=v2.0.50727
SET DEFINE=NET_2_0
GOTO endset

:set_mono
ECHO  Using Mono compiler.
SET DEFINE=MONO /r:System.dll
SET COMPILER=mcs
GOTO compile

:endset

SET COMPILER=%WINDIR%\Microsoft.NET\Framework\%DOTNET_VERSION%\csc.exe /nologo

:compile

SET DEBUG=
IF NOT "%2" == "debug" GOTO endset_debug
SET DEBUG=/debug+

:endset_debug

ECHO.
CALL %COMPILER% /define:%DEFINE% /target:library /out:Bin\MvxLib.MvxSck.dll /doc:Bin\MvxLib.MvxSck.xml MvxSck\*.cs %DEBUG%
CALL %COMPILER% /define:%DEFINE% /target:library /out:Bin\MvxLib.MvxSckCommander.dll MvxSckCommander\*.cs %DEBUG%

IF ERRORLEVEL 1 GOTO ERR

ECHO  *** SUCCESS ***
GOTO end

:err
ECHO  *** FAILED ***
PAUSE

:end