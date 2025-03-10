﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

internal partial class Interop
{
    internal partial class Kernel32
    {
        [LibraryImport(Libraries.Kernel32, SetLastError = true)]
        public static unsafe partial int WideCharToMultiByte(
            CP CodePage,
            uint dwFlags,
            char* lpWideCharStr,
            int cchWideChar,
            byte* lpMultiByteStr,
            int cbMultiByte,
            IntPtr lpDefaultChar,
            BOOL* lpUsedDefaultChar);
    }
}
