/*
 *
 * MvxLib, an open source C# library used for communication with Intentia Movex.
 * http://mvxlib.sourceforge.net
 *
 * Copyright (C) 2005 - 2006  Mattias Bengtsson
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("MvxSck")]
[assembly: AssemblyDescription("A free C# implementation of MvxSockX. Provides an API to communicate with Intentia Movex MI-programs via the FPW-service.")]
[assembly: AssemblyCompany("")]

#if MONO
[assembly: AssemblyProduct("MvxLib (compiled with Mono)")]
[assembly: AssemblyConfiguration("Compiled with Mono")]
#elif NET_2_0
[assembly: AssemblyProduct("MvxLib (compiled with Microsoft .NET Framework 2.0)")]
[assembly: AssemblyConfiguration("Compiled with Microsoft .NET Framework 2.0")]
#else
[assembly: AssemblyProduct("MvxLib (compiled with Microsoft .NET Framework 1.1)")]
[assembly: AssemblyConfiguration("Compiled with Microsoft .NET Framework 1.1")]
#endif

[assembly: AssemblyCopyright("Copyright (C) 2005 - 2006  Mattias Bengtsson")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("0.2.*")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
