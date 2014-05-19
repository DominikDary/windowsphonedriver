﻿// <copyright file="NativeMethods.cs" company="Salesforce.com">
//
// Copyright (c) 2014 Salesforce.com, Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
// following conditions are met:
//
//    Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//    disclaimer.
//
//    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//    following disclaimer in the documentation and/or other materials provided with the distribution.
//
//    Neither the name of Salesforce.com nor the names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsPhoneDriverServer.Internal
{
    /// <summary>
    /// Wraps native OS methods that must be called via P/Invoke
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Represents values for system metrics.
        /// </summary>
        internal enum SystemMetrics
        {
            /// <summary>
            /// Represents a Server R2
            /// </summary>
            ServerR2 = 89,

            /// <summary>
            /// Media Center Edition
            /// </summary>
            MediaCenter = 87,

            /// <summary>
            /// Starter Edition
            /// </summary>
            Starter = 88,

            /// <summary>
            /// Tablet PC Edition
            /// </summary>
            TabletPC = 86
        }

        /// <summary>
        /// Represents values for processor architecture.
        /// </summary>
        internal enum ProcessorArchitecture
        {
            /// <summary>
            /// x64 (AMD or Intel)
            /// </summary>
            AMD64 = 9,

            /// <summary>
            /// Intel Itanium Processor Family (IPF)
            /// </summary>
            IA64 = 6,

            /// <summary>
            /// Represents a processor of the x86 architecture
            /// </summary>
            X86 = 0,

            /// <summary>
            /// Unknown architecture
            /// </summary>
            Unknown = 0xffff
        }

        /// <summary>
        /// Represents values for the version of NT of this OS.
        /// </summary>
        internal enum VersionNT
        {
            /// <summary>
            /// The system is a domain controller and the operating system is Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
            /// </summary>
            DomainController = 0x0000002,
 
            /// <summary>
            /// The operating system is Windows Server 2008, Windows Server 2003, or Windows 2000 Server.
            /// </summary>
            /// <remarks>
            /// Note that a server that is also a domain controller is reported as DomainController, not Server.
            /// </remarks>
            Server = 0x0000003,

            /// <summary>
            /// The operating system is Windows Vista, Windows XP Professional, Windows XP Home Edition, or Windows 2000 Professional.
            /// </summary>
            Workstation = 0x0000001
        }

        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern ErrorCode HttpQueryServiceConfiguration(
             IntPtr ServiceIntPtr,
             HttpServiceConfigId ConfigId,
             IntPtr pInputConfigInfo,
             int InputConfigInfoLength,
             IntPtr pOutputConfigInfo,
             int OutputConfigInfoLength,
             [Optional()]
             out int pReturnLength,
             IntPtr pOverlapped);

        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern ErrorCode HttpSetServiceConfiguration(
             IntPtr ServiceIntPtr,
             HttpServiceConfigId ConfigId,
             IntPtr pConfigInformation,
             int ConfigInformationLength,
             IntPtr pOverlapped);

        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern ErrorCode HttpDeleteServiceConfiguration(
             IntPtr ServiceIntPtr,
             HttpServiceConfigId ConfigId,
             IntPtr pConfigInformation,
             int ConfigInformationLength,
             IntPtr pOverlapped);

        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern ErrorCode HttpInitialize(
             HttpApiVersion Version,
             uint Flags,
             IntPtr pReserved);

        [DllImport("httpapi.dll", SetLastError = true)]
        internal static extern ErrorCode HttpTerminate(
             uint Flags,
             IntPtr pReserved);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api", Justification = "Managed equivalent does not provide full granularity")]
        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetVersionEx(ref OSVersionInfoEx lpVersionInfo);

        [DllImport("kernel32")]
        internal static extern void GetSystemInfo(out SystemInfo lpSystemInfo);

        [DllImport("user32")]
        internal static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// Represents the structure used by the GetOSVersionInfoEx API
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVersionInfoEx
        {
            /// <summary>
            /// The size of this data structure, in bytes.
            /// </summary>
            public int dwOSVersionInfoSize;

            /// <summary>
            /// The major version number of the operating system.
            /// </summary>
            public int dwMajorVersion;

            /// <summary>
            /// The major version number of the operating system.
            /// </summary>
            public int dwMinorVersion;

            /// <summary>
            /// The build number of the operating system.
            /// </summary>
            public int dwBuildNumber;

            /// <summary>
            /// The build number of the operating system.
            /// </summary>
            public int dwPlatformId;

            /// <summary>
            /// A null-terminated string, such as "Service Pack 3", that indicates the latest Service Pack
            /// installed on the system. If no Service Pack has been installed, the string is empty.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;

            /// <summary>
            /// The major version number of the latest Service Pack installed on the system. For example, 
            /// for Service Pack 3, the major version number is 3. If no Service Pack has been installed, 
            /// the value is zero.
            /// </summary>
            public short wServicePackMajor;

            /// <summary>
            /// The minor version number of the latest Service Pack installed on the system. For example,
            /// for Service Pack 3, the minor version number is 0.
            /// </summary>
            public short wServicePackMinor;

            /// <summary>
            /// A bit mask that identifies the product suites available on the system.
            /// </summary>
            public short wSuiteMask;

            /// <summary>
            /// Any additional information about the system.
            /// </summary>
            public byte wProductType;

            /// <summary>
            /// Reserved for future use.
            /// </summary>
            public byte wReserved;
        }

        /// <summary>
        /// Represents the structure used by the GetSystemInfo API
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct SystemInfo
        {
            /// <summary>
            /// The processor architecture of the installed operating system.
            /// </summary>
            public ushort processorArchitecture;

            /// <summary>
            /// his member is reserved for future use.
            /// </summary>
            public ushort reserved;

            /// <summary>
            /// The page size and the granularity of page protection and commitment.
            /// </summary>
            public uint pageSize;

            /// <summary>
            /// A pointer to the lowest memory address accessible to applications and dynamic-link libraries (DLLs).
            /// </summary>
            public IntPtr minimumApplicationAddress;

            /// <summary>
            /// A pointer to the highest memory address accessible to applications and DLLs.
            /// </summary>
            public IntPtr maximumApplicationAddress;

            /// <summary>
            /// A mask representing the set of processors configured into the system.
            /// </summary>
            public IntPtr activeProcessorMask;

            /// <summary>
            /// The number of logical processors in the current group.
            /// </summary>
            public uint numberOfProcessors;

            /// <summary>
            /// An obsolete member that is retained for compatibility.
            /// </summary>
            public uint processorType;

            /// <summary>
            /// The granularity for the starting address at which virtual memory can be allocated.
            /// </summary>
            public uint allocationGranularity;

            /// <summary>
            /// The architecture-dependent processor level. It should be used only for display purposes.
            /// </summary>
            public ushort processorLevel;

            /// <summary>
            /// The architecture-dependent processor revision.
            /// </summary>
            public ushort processorRevision;
        }
    }
}
