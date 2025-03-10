﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using static Interop.Kernel32;

internal static partial class Interop
{
    internal static partial class Ole32
    {
        [DllImport(Libraries.Ole32, ExactSpelling = true)]
        public static extern IntPtr OleDuplicateData(IntPtr hSrc, short cfFormat, GMEM uiFlags);
    }
}
