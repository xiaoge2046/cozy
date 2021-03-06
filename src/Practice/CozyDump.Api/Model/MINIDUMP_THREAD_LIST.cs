﻿using System.Runtime.InteropServices;

namespace CozyDump.Api.Model
{
    public static partial class DumpApiModel
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MINIDUMP_THREAD_LIST
        {
            public uint NumberOfThreads;
            public MINIDUMP_THREAD[] Threads;
        }
    }
}
