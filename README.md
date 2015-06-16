# MvxLib

## Introduction

MvxLib is a free library that enables you to communicate with Intentia Movex (now Lawson M3) so called MI-programs via an FPW-service. It is a .NET implementation/clone of MvxSockX that only uses managed code written in C# and can be compiled with Microsoft .Net Framework 1.1, 2.0 as well as Mono. The ability to run it in Mono means that it is (theoretically) usable in Unix environments (Linux, Mac et cetera). Development is sponsored by Insytec.

## About

MvxLib's main purpose was to replace the proprietary (closed source) COM+ component (MVXSOCKX_SVRLib.dll or MvxSockX). It's main advantages is therefore that you don't need to interop COM+ components in pure .NET-environments, which is known to behave unexpectedly (crashing without throwing exceptions for example).

It has been proven to run cleanly with thousands of requests per minute, the communication-codebase has not been changed in several months, proving it's stability.

MvxLib is licensed under the LGPL (GNU Lesser General Public License). If you are unfamiliar with the term, please read more about LGPL or open source in general.

## Contribute

If you have any type of suggestions or feedback I will happily receive it! See my SourceForge user page for contact information. I also provide development and programming services.

## Documentation

The library contains the following classes (all in the namespace MvxLib):

* **MvxSck** and **TraceLog** - 
provides login, logout, command-sending and tracing
* **MvxSckCommand** - 
methods in this class defines common MI-program interfaces, use theese to easily build command-strings to send through the socket
* **MvxSckCommandBuilder** - 
this class provides functions to build command-strings in a controlled manor, it is mainly used by MvxSckCommand
* **MvxSckResponse** - 
methods in this class are used to parse common MI-program response-strings
* **MvxSckResponseBuilder** - 
this class provides functions to parse response-strings and is mainly used by MvxSckResponse

## Example code

Example code in C# using MvxLib. This shows how you could use MvxLib to connect, send a command and retreive the response-message.

```
string host = "MVXHST";
int ip = 6012;
string user = "USRNME";
string pass = "THEPWD";
string libr = "MVXTST";
string prgm = "OIS320MI";

MvxSck conn = null;

try
{
    // Open connection to Movex
    conn = new MvxSck(host, ip);
    conn.Connect();
    conn.Login(
        user,
        pass,
        libr,
        prgm);

    // Prepare command to send, notice the use of datatypes (not just strings)
    string cmd = MvxSckCommand.GetPriceLine(
        1,                // Company
        "AS1",            // Facility
        "654",            // Customer
        "AS-12",          // Item
        "SP1",            // Warehouse
        DateTime.Now,     // Order date
        1,                // Order qty
        "PKG",            // Unit
        "SEK",            // Currency
        string.Empty,     // Order type
        string.Empty);    // Pricelist

    // Run command; get response
    string ret = conn.Execute(cmd);

    MvxSckResponse.PriceLineItem retObj = MvxSckResponse.GetPriceLineItem(ret);
    // retObj contains all the returned information parsed and packaged
}
catch (MvxSckException ex)
{
    // Handle exceptions thrown by MvxSck
}
catch (Exception ex)
{
    // Handle exceptions
}
finally
{
    if (conn != null)
        conn.Close();
    conn = null;
}
```

Example code in Visual Basic doing the same thing using the original MVXSOCKX_SVRLib.dll.

```
Dim s_host As String
Dim s_port As String
Dim s_appl As String
Dim s_crypt As Long
Dim s_crpykey As String

s_host = "MVXHST"
s_port = 6012
s_appl = ""
s_crypt = 0
s_crpykey  = ""

Dim s_comp As String
Dim s_usrnam As String
Dim s_passwd As String
Dim s_library As String
Dim s_program As String

s_comp = "LOCALA"
s_usrnam = "USRNME"
s_passwd = "THEPWD"
s_library = "MVXTST"
s_program = "OIS320MI"

Dim socket As MvxSockX
Dim retval As Long
Dim cmd As String
Dim res_val As String

Set socket = New MvxSockX
retval = socket.MvxSockSetup(s_host, s_port, s_appl, s_crypt, s_crpykey)

If Not retval = 0 Then
    ' Raise error! (errorcode = retval)
Else
    retval = socket.MvxSockInit(s_comp, s_usrnam, s_passwd, s_library & "/" & s_program)

    If Not retval = 0 Then
        ' Raise error! (errorcode = retval)
    Else
        cmd = "GetPriceLine   "
        cmd = cmd & "  1"               ' Company
        cmd = cmd & "AS1"               ' Facility
        cmd = cmd & "       654"        ' Customer
        cmd = cmd & "          AS-12"   ' Item
        cmd = cmd & "SP1"               ' Warehouse
        cmd = cmd & "  20060405"        ' Order date
        cmd = cmd & "         2"        ' Order qty
        cmd = cmd & "PKG"               ' Unit
        cmd = cmd & "SEK"               ' Currency
        cmd = cmd & "INP"               ' Order type
        cmd = cmd & "  "                ' Pricelist

        retval = socket.MvxSockTrans(cmd, res_val)
        ' res_val contains the returned string
    End If

    socket.MvxSockClose
End If
```
