using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

namespace Lucene.Net.LukeNet.ClassFinder
{
	/// <summary>
	/// This class finds classes that implement one or more specified interfaces.
	/// </summary>
	public class ClassFinder
	{
		private ClassFinder()
		{}

		/// <summary>
		/// Convenience method for <code>findClassesThatExtend(Class[],
		/// boolean)</code> with the option to include inner classes in the search
		/// set to false.
		/// </summary>
		/// <returns>ArrayList containing discovered classes.</returns>
		public static Type[] FindClassesThatExtend(String[] paths, Type[] superTypes)
		{
			return FindClassesThatExtend(paths, superTypes, false);
		}
    
		/// <summary>
		/// Convenience method that finds classes in the application directory
		/// </summary>
		public static Type[] FindClassesThatExtend(Type[] superClasses)
		{
			return FindClassesThatExtend(new string[]{ApplicationDirectory}, superClasses);
		}

		/// <summary>
		/// Convenience method that finds classes in the application directory
		/// </summary>
		public static Type[] FindClassesThatExtend(Type superClass)
		{
			return FindClassesThatExtend(new string[]{ApplicationDirectory}, new Type[]{superClass});
		}
    
		private static string ApplicationDirectory
		{
			get
			{
				FileInfo file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
				string dir = file.DirectoryName;
				if (!(dir.EndsWith("\\") || dir.EndsWith("/")))
					dir += "\\";
				return dir;
			}
		}

		/// <summary>
		/// Convenience method to get a list of types that can be instantiated
		/// </summary>
		/// <param name="superclass">superclass an interface or base class</param>
		public static Type[] GetInstantiableSubtypes(Type superType)
		{
			ArrayList instantiableSubtypes = new ArrayList();
			Type[] types = FindClassesThatExtend(superType);

			foreach (Type t in types)
			{
				if (!(t.IsAbstract && t.IsInterface))
				{
					instantiableSubtypes.Add(t);
				}
			}
			return (Type[]) instantiableSubtypes.ToArray(typeof(Type));
		}

		/// <summary>
		/// Searches the specified path and returns file names
		/// which could be considered as assemblies.
		/// </summary>
		private static String[] GetAssembliesList(String[] paths)
		{
			// remove equal paths
			Hashtable hashPaths = new Hashtable();
			foreach (string path in paths)
			{
				hashPaths.Add(path, null);
			}

			ArrayList fullList = new ArrayList();

			foreach (string path in hashPaths.Keys)
			{
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path, "*.exe");
					fullList.AddRange(files);
					files = Directory.GetFiles(path, "*.dll");
					fullList.AddRange(files);
				}
			}
			return (string[])fullList.ToArray(typeof(string));
		}

		public static Type[] FindClassesThatExtend(string[] strPaths, Type[] superTypes, bool innerTypes)
		{
			// first get all the classes
			Type[] allTypes = FindTypesInPaths(strPaths);

			return FindAllSubtypes(superTypes, allTypes, innerTypes);
		}

		/// <summary>
		/// Finds all classes that extend the classes in the listSuperClasses
		/// ArrayList, searching in the listAllClasses ArrayList.
		/// </summary>
		/// <param name="superTypes">the base types to find subclasses for</param>
		/// <param name="allTypes">the collection of types to search in</param>
		/// <param name="innerClasses">indicate whether to include inner classes in the search</param>
		/// <returns>ArrayList of the subclasses</returns>
		private static Type[] FindAllSubtypes(Type[] superTypes, Type[] allTypes, bool innerClasses)
		{
			ArrayList subClasses = new ArrayList();
		
			if (null == superTypes) return new Type[0];

			foreach (Type t in superTypes)
				subClasses.AddRange(FindAllSubTypesOneType(t, allTypes, innerClasses));
		
			return (Type[])subClasses.ToArray(typeof(Type));
		}
    
		/// <summary>
		/// Finds all classes that extend the class, searching in the listAllClasses
		/// ArrayList.
		/// </summary>
		/// <param name="theType">the parent type</param>
		/// <param name="allTypes">the collection of types to search in</param>
		/// <param name="innerClasses">
		/// indicates whether inners classes should be
		/// included in the search
		/// </param>
		/// <returns>the collection of discovered subtypes</returns>
		private static ArrayList FindAllSubTypesOneType(Type theType, Type[] allTypes, bool innerClasses)
		{
			ArrayList subClasses = new ArrayList();
			if (null == allTypes) return subClasses;

			foreach (Type t in allTypes)
			{
				if (innerClasses)
				{
					subClasses.AddRange(FindAllSubTypesOneType(theType, t.GetNestedTypes(), true));
				}
				if (t.IsSubclassOf(theType))
					subClasses.Add(t);
			}
			return subClasses;
		}
    
		private static Type[] FindTypesInPaths(string[] paths)
		{
			string[] assemblies = GetAssembliesList(paths);
			ArrayList foundTypes = new ArrayList(assemblies.Length*10);

			foreach(string assemblyName in assemblies)
			{
				if (assemblyName != null && assemblyName != "")
				{
					Assembly a = null;
					try
					{
						a = Assembly.LoadFrom(assemblyName);
					}
					catch(FileNotFoundException){}
					catch(BadImageFormatException){}

					if (a != null)
					{
						foundTypes.AddRange(a.GetTypes());
					}
				}
			}
			return (Type[])foundTypes.ToArray(typeof(Type));
		}
	}
}