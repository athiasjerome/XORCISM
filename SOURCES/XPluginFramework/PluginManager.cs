using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Data;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace XPluginFramework
{
	/// <summary>
    /// Copyright (C) 2012-2015 Jerome Athias
    /// The PluginManager tracks changes to the plugin directory, handles reloading of the plugins (with dynamic compilation), and monitoring of the plugins' directory.
    /// Yep, hot updates baby ;p
    /// All trademarks and registered trademarks are the property of their respective owners.
    /// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
    /// 
    /// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    /// 
    /// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
    /// </summary>
    
    public class PluginManager : MarshalByRefObject
	{
		// private bool started = false;
		// private bool autoReload = true;
		// private IList compilerErrors = null;
		// private bool ignoreErrors = true;
		private PluginSourceEnum pluginSources = PluginSourceEnum.Both;
		protected string pluginDirectory = null;
		// protected FileSystemWatcher fileSystemWatcher = null;
		// protected DateTime changeTime = new DateTime(0);
		// protected Thread pluginReloadThread = null;
		protected string lockObject = "{PLUGINMANAGERLOCK}";
		// protected bool beginShutdown = false;
		// protected bool active = true;
		protected AppDomain pluginAppDomain = null;
		protected AppDomainSetup pluginAppDomainSetup = null;
		// protected RemoteLoader remoteLoader = null;
		protected LocalLoader localLoader = null;
		protected IList references = new ArrayList();

		/// <summary>
		/// Constructs a plugin manager
		/// </summary>
        /*
		public PluginManager() : this("plugins", true)
		{
		}
        */

		/// <summary>
		/// Constructs a plugin manager
		/// </summary>
		/// <param name="pluginRelativePath">The relative path to the plugins directory</param>
        /*
		public PluginManager(string pluginRelativePath) : this(pluginRelativePath, true)
		{
		}
        */

		/// <summary>
		/// Constructs a plugin manager
		/// </summary>
		/// <param name="autoReload">Should auto reload on file changes</param>
        /*
		public PluginManager(bool autoReload) : this("plugins", autoReload)
		{
		}
        */

		/// <summary>
		/// Constructs a plugin manager
		/// </summary>
		/// <param name="pluginRelativePath">The relative path to the plugins directory</param>
		public PluginManager(string pluginRelativePath)
		{
			// this.autoReload = autoReload;

			string assemblyLoc = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = assemblyLoc.Substring(0, assemblyLoc.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			pluginDirectory = Path.Combine(currentDirectory, pluginRelativePath);
			if (!pluginDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				pluginDirectory = pluginDirectory + Path.DirectorySeparatorChar;
			}
			
			localLoader = new LocalLoader(pluginDirectory);

			// Add the most common references since plugin authors can't control which references they
			// use in scripts.  Adding a reference later that already exists does nothing.
			AddReference("Accessibility.dll");
			AddReference("Microsoft.Vsa.dll");
			AddReference("System.Configuration.Install.dll");
			AddReference("System.Data.dll");
			AddReference("System.Design.dll");
			AddReference("System.DirectoryServices.dll");
			AddReference("System.Drawing.Design.dll");
			AddReference("System.Drawing.dll");
			AddReference("System.EnterpriseServices.dll");
			AddReference("System.Management.dll");
			AddReference("System.Runtime.Remoting.dll");
			AddReference("System.Runtime.Serialization.Formatters.Soap.dll");
			AddReference("System.Security.dll");
			AddReference("System.ServiceProcess.dll");
			AddReference("System.Web.dll");
			AddReference("System.Web.RegularExpressions.dll");
			AddReference("System.Web.Services.dll");
			AddReference("System.Windows.Forms.Dll");
			AddReference("System.XML.dll");
            //TODO: Review this
		}

        public override object InitializeLifetimeService()
        {
            return null; //Allow infinite lifetime
        } 

		/// <summary>
		/// The destructor for the plugin manager
		/// </summary>
		~PluginManager()
		{
            Stop();
            localLoader = null;
		}

        

		/// <summary>
		/// Fires when the plugins have been reloaded and references to the old objects need
		/// to be updated.
		/// </summary>
		public event EventHandler PluginsReloaded;

		/// <summary>
		/// Handles changes to the file system in the plugin directory
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        /*
		private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			changeTime = DateTime.Now + new TimeSpan(0, 0, 10);
		}
        */

		/// <summary>
		/// The main updater thread loop.
		/// </summary>
        /*
		protected void ReloadThreadLoop()
		{
			if (!started)
			{
				throw new InvalidOperationException("PluginManager has not been started.");
			}
			DateTime invalidTime = new DateTime(0);
			while (!beginShutdown)
			{
				if (changeTime != invalidTime && DateTime.Now > changeTime)
				{
					ReloadPlugins();
				}
				Thread.Sleep(5000);
			}
			active = false;
		}
        */

		/// <summary>
		/// Initializes the plugin manager
		/// </summary>
        /*
		public void Start()
		{
			started = true;
			if (autoReload)
			{
				fileSystemWatcher = new FileSystemWatcher(pluginDirectory);
				fileSystemWatcher.EnableRaisingEvents = true;
				fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
				fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemWatcher_Changed);
				fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_Changed);

				pluginReloadThread = new Thread(new ThreadStart(this.ReloadThreadLoop));
				pluginReloadThread.Start();
			}
			ReloadPlugins();
		}
        */

		/// <summary>
		/// Reloads all plugins in the plugins directory
		/// </summary>
        /*
		public void ReloadPlugins()
		{
			if (!started)
			{
				throw new InvalidOperationException("PluginManager has not been started.");
			}
			lock (lockObject)
			{
				localLoader.Unload();
				localLoader = new LocalLoader(pluginDirectory);
				LoadUserAssemblies();

				changeTime = new DateTime(0);
				if (PluginsReloaded != null)
				{
					PluginsReloaded(this, new EventArgs());
				}
			}
		}
        */

		/// <summary>
		/// Loads all user created plugin assemblies
		/// </summary>
		public void LoadUserAssembly(string filename)
		{
            //TODO: Input validation
            /*
			if (!started)
			{
				throw new InvalidOperationException("PluginManager has not been started.");
			}
			compilerErrors = new ArrayList();
            */

            localLoader.LoadAssembly(filename);

            /*
			DirectoryInfo directory = new DirectoryInfo(pluginDirectory);
			// if (pluginSources == PluginSourceEnum.DynamicAssemblies || pluginSources == PluginSourceEnum.Both)
            if(1 == 1)
			{
				foreach (FileInfo file in directory.GetFiles("*.dll"))
				{	
					try
					{
						localLoader.LoadAssembly(file.FullName);
					}
					catch (PolicyException e)
					{
						throw new PolicyException(
							String.Format("Cannot load {0} - code requires privilege to execute", file.Name),
							e);
					}
				}
			}
            */

            /*
			if (pluginSources == PluginSourceEnum.DynamicCompilation ||
				pluginSources == PluginSourceEnum.Both)
			{
				// Load all C# scripts
				{
					ArrayList scriptList;
					scriptList = new ArrayList();
					foreach (FileInfo file in directory.GetFiles("*.cs"))
					{
						scriptList.Add(file.FullName);
					}
					LoadScriptBatch((string[])scriptList.ToArray(typeof(string)));
				}
				// Load all VB.net scripts
				{
					ArrayList scriptList;
					scriptList = new ArrayList();
					foreach (FileInfo file in directory.GetFiles("*.vb"))
					{
						scriptList.Add(file.FullName);
					}
					LoadScriptBatch((string[])scriptList.ToArray(typeof(string)));
				}
				// Load all JScript scripts
				{
					ArrayList scriptList;
					scriptList = new ArrayList();
					foreach (FileInfo file in directory.GetFiles("*.js"))
					{
						scriptList.Add(file.FullName);
					}
					LoadScriptBatch((string[])scriptList.ToArray(typeof(string)));
				}
			}
            */
		}

		/// <summary>
		/// Batch loads a set of scripts of the same language
		/// </summary>
		/// <param name="filenames">The list of script filenames to load</param>
        /*
		private void LoadScriptBatch(string[] filenames)
		{
			if (filenames.Length > 0)
			{
				IList errors = localLoader.LoadScripts(filenames, references);
				if (errors.Count > 0)
				{
					// If there are compiler errors record them and the file they occurred in
					foreach (string error in errors)
					{
						compilerErrors.Add(error);
					}
					if (!ignoreErrors)
					{
						StringBuilder aggregateErrorText = new StringBuilder();
						foreach (string error in errors)
						{
							aggregateErrorText.Append(error + "\r\n");
						}
						throw new InvalidOperationException(
							"\r\nCompiler error(s) have occurred:\r\n\r\n " +
							aggregateErrorText.ToString() + "\r\n");
					}
				}
			}
		}
        */

		/// <summary>
		/// Adds a reference to the plugin manager to be used when compiling scripts
		/// </summary>
		/// <param name="referenceToDll">The reference to the dll to add</param>
		public void AddReference(string referenceToDll)
		{
			if (!references.Contains(referenceToDll))
			{
				references.Add(referenceToDll);
			}
		}

		/// <summary>
		/// Shuts down the plugin manager
		/// </summary>
		public void Stop()
		{
			try
			{
				// started = false;

				localLoader.Unload();
                AppDomain.Unload(pluginAppDomain);
                pluginAppDomain = null;
                /*
				beginShutdown = true;
				while (active)
				{
					Thread.Sleep(100);  //Hardcoded
				}
                */
			}
			catch
			{
				// We don't want to get any exceptions thrown if unloading fails for some reason.
			}
		}

		/// <summary>
		/// Should auto reload on file changes
		/// </summary>
        /*
		public bool AutoReload
		{
			get
			{
				return autoReload;
			}
			set
			{
				if (autoReload != value)
				{
					autoReload = value;
					if (!autoReload)
					{
						fileSystemWatcher.EnableRaisingEvents = false;
						Stop();
						pluginReloadThread = null;
						fileSystemWatcher = null;
					}
					else
					{
						fileSystemWatcher = new FileSystemWatcher(pluginDirectory);
						fileSystemWatcher.EnableRaisingEvents = true;
						fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
						fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemWatcher_Changed);
						fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_Changed);

						pluginReloadThread = new Thread(new ThreadStart(this.ReloadThreadLoop));
						pluginReloadThread.Start();
					}
				}
			}
		}
        */

		/// <summary>
		/// Determines whether an exception will be thrown if a compiler error occurs in a script file
		/// </summary>
        /*
		public bool IgnoreErrors
		{
			get
			{
				return ignoreErrors;
			}
			set
			{
				ignoreErrors = value;
			}
		}
        */

		/// <summary>
		/// The type of plugin sources that will be managed by the plugin manager
		/// </summary>
		public PluginSourceEnum PluginSources
		{
			get
			{
				return pluginSources;
			}
			set
			{
				pluginSources = value;
			}
		}

		/// <summary>
		/// The list of all compiler errors for all scripts.
		/// Null if no compilation has ever occurred, empty list if compilation succeeded
		/// </summary>
        /*
		public IList CompilerErrors
		{
			get
			{
				if (!started)
				{
					throw new InvalidOperationException("PluginManager has not been started.");
				}
				return compilerErrors;
			}
		}
        */

		/// <summary>
		/// The list of loaded plugin assemblies
		/// </summary>
		public string[] Assemblies
		{
			get
			{
                //if (!started)
                //{
                //    throw new InvalidOperationException("PluginManager has not been started.");
                //}
				return localLoader.Assemblies;
			}
		}

		/// <summary>
		/// The list of loaded plugin types
		/// </summary>
		public string[] Types
		{
			get
			{
                //if (!started)
                //{
                //    throw new InvalidOperationException("PluginManager has not been started.");
                //}
				return localLoader.Types;
			}
		}

		/// <summary>
		/// Retrieves the type objects for all subclasses of the given type within the loaded plugins.
		/// </summary>
		/// <param name="baseClass">The base class</param>
		/// <returns>All subclases</returns>
		public string[] GetSubclasses(string baseClass)
		{
            //if (!started)
            //{
            //    throw new InvalidOperationException("PluginManager has not been started.");
            //}
			return localLoader.GetSubclasses(baseClass);
		}

		/// <summary>
		/// Determines if this loader manages the specified type
		/// </summary>
		/// <param name="typeName">The type to check if this PluginManager handles</param>
		/// <returns>True if this PluginManager handles the type</returns>
		public bool ManagesType(string typeName)
		{
            //if (!started)
            //{
            //    throw new InvalidOperationException("PluginManager has not been started.");
            //}
			return localLoader.ManagesType(typeName);
		}

		/// <summary>
		/// Returns the value of a static property
		/// </summary>
		/// <param name="typeName">The type to retrieve the static property value from</param>
		/// <param name="propertyName">The name of the property to retrieve</param>
		/// <returns>The value of the static property</returns>
		public object GetStaticPropertyValue(string typeName, string propertyName)
		{
            //if (!started)
            //{
            //    throw new InvalidOperationException("PluginManager has not been started.");
            //}
			return localLoader.GetStaticPropertyValue(typeName, propertyName);
		}

		/// <summary>
		/// Returns the result of a static method call
		/// </summary>
		/// <param name="typeName">The type to call the static method on</param>
		/// <param name="propertyName">The name of the method to call</param>
		/// <param name="methodParams">The parameters to pass to the method</param>
		/// <returns>The return value of the method</returns>
		public object CallStaticMethod(string typeName, string methodName, object[] methodParams)
		{
            //if (!started)
            //{
            //    throw new InvalidOperationException("PluginManager has not been started.");
            //}
			return localLoader.CallStaticMethod(typeName, methodName, methodParams);
		}

		/// <summary>
		/// Returns a proxy to an instance of the specified plugin type
		/// </summary>
		/// <param name="typeName">The name of the type to create an instance of</param>
		/// <param name="bindingFlags">The binding flags for the constructor</param>
		/// <param name="constructorParams">The parameters to pass to the constructor</param>
		/// <returns>The constructed object</returns>
		public MarshalByRefObject CreateInstance(string typeName, BindingFlags bindingFlags, object[] constructorParams)
		{
            //if (!started)
            //{
            //    throw new InvalidOperationException("PluginManager has not been started.");
            //}
			return localLoader.CreateInstance(typeName, bindingFlags, constructorParams);
		}
	}
}
