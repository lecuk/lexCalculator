using System;
using System.Runtime.InteropServices;

namespace lexCalculator.Static
{
	// Not implemented yet.
	static class lexCUDAHelper
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
		internal static extern IntPtr LoadLibrary(string lpszLib);
	}
}
