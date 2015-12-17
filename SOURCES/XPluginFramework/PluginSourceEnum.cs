using System;

namespace XPluginFramework
{
	/// <summary>
	/// Determines whether the PluginManager will dynamically load assemblies, dynamically compile them, or
	/// both.
	/// </summary>
	public enum PluginSourceEnum
	{
		/// <summary>
		/// Dynamic loading of pre-compiled assemblies
		/// </summary>
		DynamicAssemblies,
		/// <summary>
		/// Dynamic compilation of uncompiled source files
		/// </summary>
		DynamicCompilation,
		/// <summary>
		/// Both dynamic loading and dynamic compilation
		/// </summary>
		Both
	}
}
