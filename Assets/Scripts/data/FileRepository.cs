using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.data {
	public class FileRepository : IFileRepository {

		public string LoadFile(string filename) {
			string result = null;
			string line;

			if ( !Directory.Exists(Application.dataPath) ) return result;

			if ( !File.Exists(Application.dataPath + "/" + filename + ".txt") ) {
				return result;
			} else {
				StreamReader theReader = new StreamReader(Application.dataPath + "/" + filename + ".txt", System.Text.Encoding.Default);
				result = theReader.ReadToEnd();
				theReader.Close();
			}
			return result.Trim();
		}



		public void SaveFile(string filename, string content) {
			StreamWriter writer;
			if ( !Directory.Exists(Application.dataPath) )
				Directory.CreateDirectory(Application.dataPath);

			writer = new StreamWriter(Application.dataPath + "/" + filename + ".txt");
			writer.WriteLine(content);
			writer.Close();
		}

	}

}
