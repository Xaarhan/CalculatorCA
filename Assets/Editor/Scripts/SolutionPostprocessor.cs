using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace EngineModule.Editor {
    public class SolutionPostprocessor : AssetPostprocessor {
        private const string c_AssemblyCSharp = "Assembly-CSharp";
        private const string c_AssemblyCSharpFirstpass = "Assembly-CSharp-firstpass";
        private const string c_AssemblyCSharpEditor = "Assembly-CSharp-Editor";
        private const string c_AssemblyCSharpEditorFirstpass = "Assembly-CSharp-Editor-firstpass";
        private static readonly char[] c_Slashes = new[] { '\\', '/' };

        public static readonly bool Enable = true;
        public static readonly bool UseCrossPathForStandartAsseblies = false; //Если true то ищет общий путь для стандартных сборок (которые не asmdef)

        public static void OnGeneratedCSProjectFiles() {
            if ( !Enable )
                return;
            var filenames = Directory.GetFiles(".", "*.csproj", SearchOption.TopDirectoryOnly);
            for ( int i = 0; i < filenames.Length; i++ ) {
                FixCsproj(Path.GetFileNameWithoutExtension(filenames[i]));
            }
        }

        private static void OnAssemblyCompilationFinished(string s, CompilerMessage[] compilerMessages) {
            if ( !Enable )
                return;
            var filename = Path.GetFileNameWithoutExtension(s);
            FixCsproj(filename);
        }

        private static void FixCsproj(string csprojName) {
            if ( string.IsNullOrEmpty(csprojName) )
                return;

            var csproj = csprojName + ".csproj";
            if ( !File.Exists(csproj) )
                return;

            var doc = XDocument.Load(csproj);
            if ( doc.Root == null )
                return;

            var allAsmdefPaths = Directory.GetFiles("Assets", "*.asmdef", SearchOption.AllDirectories).Select(Path.GetDirectoryName).ToArray();

            var asmdef = Directory.GetFiles("Assets", csprojName + ".asmdef", SearchOption.AllDirectories).FirstOrDefault();
            var asmDefRootDir = Path.GetDirectoryName(asmdef);
            bool isPlugins = csprojName == c_AssemblyCSharpEditorFirstpass || csprojName == c_AssemblyCSharpFirstpass;
            bool isEditor = csprojName == c_AssemblyCSharpEditor || csprojName == c_AssemblyCSharpEditorFirstpass;
            if ( asmDefRootDir != null ) {
                isPlugins |= IsPlugins(asmDefRootDir);
                isEditor |= IsEditor(asmDefRootDir);
            }

            var includedPaths = new List<string>();
            var itemGroups = GetElements(doc.Root, "ItemGroup").ToList();
            foreach ( var itemGroup in itemGroups ) {
                if ( UseCrossPathForStandartAsseblies && asmDefRootDir == null ) {
                    var compiles = GetElements(itemGroup, "Compile").ToList();
                    foreach ( var compile in compiles ) {
                        var includeAttribute = compile.Attribute("Include");
                        if ( includeAttribute != null )
                            includedPaths.Add(includeAttribute.Value);
                    }
                }

                var nones = GetElements(itemGroup, "None").ToList();

                foreach ( var none in nones ) {
                    var includeAttribute = none.Attribute("Include");
                    if ( includeAttribute != null ) {
                        if ( CanRemove(includeAttribute.Value, asmDefRootDir, isEditor, isPlugins, allAsmdefPaths) )
                            none.Remove();
                        else if ( UseCrossPathForStandartAsseblies && asmDefRootDir == null )
                            includedPaths.Add(includeAttribute.Value);
                    }
                }
            }

            var baseDirBuilder = new StringBuilder();
            if ( asmDefRootDir != null )
                baseDirBuilder.Append(asmDefRootDir);
            else if ( UseCrossPathForStandartAsseblies && includedPaths.Count > 0 ) {
                var crossString = CrossStarts(includedPaths);
                var crossDirectories = GetDirectories(crossString);
                var crossDirString = string.Join("\\", crossDirectories.ToArray());
                baseDirBuilder.Append(crossDirString);
            } else
                baseDirBuilder.Append("Assets");

            var baseDir = baseDirBuilder.ToString();

            var propGroups = GetElements(doc.Root, "PropertyGroup").ToList();
            foreach ( var propGroup in propGroups ) {
                var baseDirectoryProperty = GetElements(propGroup, "BaseDirectory").FirstOrDefault();

                if ( baseDirectoryProperty != null ) {
                    baseDirectoryProperty.Value = baseDir;
                }
            }


            doc.Save(csprojName + ".csproj");
        }

        private static string CrossStarts(IList<string> strings) {
            if ( strings.Count == 0 )
                return null;

            var s0 = strings[0];
            int maxLength = s0.Length;
            for ( int i = 1; i < strings.Count; i++ ) {
                var length = strings[i].Length;
                if ( length < maxLength )
                    maxLength = length;
            }

            var result = new StringBuilder();
            for ( int i = 0; i < maxLength; i++ ) {
                for ( int j = 1; j < strings.Count; j++ ) {
                    var sOther = strings[j];
                    if ( s0[i] != sOther[i] )
                        return result.ToString();
                }
                result.Append(s0[i]);
            }
            return result.ToString();
        }

        private static List<string> GetDirectories(string path) {
            var directories = path.Split(c_Slashes, StringSplitOptions.None).ToList();
            directories.RemoveAt(directories.Count - 1); //remove filename
            return directories;
        }

        private static IEnumerable<XElement> GetElements(XElement parent, string localName) {
            var rootElements = parent.Elements();
            foreach ( var rootElement in rootElements )
                if ( rootElement.Name.LocalName == localName )
                    yield return rootElement;
        }

        private static bool CanRemove(string path, string asmdefRoot, bool isEditor, bool isPlugins, string[] asmdefPaths) {
            if ( isEditor != IsEditor(path) ) {
                return true;
            }

            if ( isPlugins != IsPlugins(path) ) {
                return true;
            }

            if ( asmdefRoot != null ) {
                if ( !path.StartsWith(asmdefRoot) ) {
                    return true;
                }
            }

            if ( asmdefPaths.Length > 0 ) {
                for ( int i = 0; i < asmdefPaths.Length; i++ ) {
                    var asmdefPath = asmdefPaths[i];
                    if ( (asmdefRoot == null || !asmdefRoot.StartsWith(asmdefPath)) && asmdefPath != asmdefRoot ) {
                        if ( path.StartsWith(asmdefPath) ) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool IsPlugins(string path) {
            return path.Contains("\\Plugins\\") || path.EndsWith("\\Plugins");
        }

        private static bool IsEditor(string path) {
            return path.Contains("\\Editor\\") || path.EndsWith("\\Editor");
        }

        [InitializeOnLoad]
        public static class SolutionInitOnLoad {
            static SolutionInitOnLoad() {
                CompilationPipeline.assemblyCompilationFinished += OnAssemblyCompilationFinished;
            }
        }
    }
}