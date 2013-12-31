// Guids.cs
// MUST match guids.h
using System;

namespace Company.NimrodVS
{
    static class GuidList
    {
        public const string guidNimrodVSPkgString = "2d33766d-5a12-49b5-9e70-80530a3eba98";
        public const string guidNimrodVSCmdSetString = "d61fb739-928f-4d76-8920-b2d42230ff93";

        public static readonly Guid guidNimrodVSCmdSet = new Guid(guidNimrodVSCmdSetString);
    };
}