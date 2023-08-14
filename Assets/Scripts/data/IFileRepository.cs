using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IFileRepository {

	string LoadFile(string filename);
	void SaveFile(string filename, string content);

}

