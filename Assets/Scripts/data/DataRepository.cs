using System;
using System.Text;
using Assets.Scripts.domain;

namespace Assets.Scripts.data {
	public class DataRepository : IDataRepositoryGateway {

		private IFileRepository _repository;
		private const string LogFileName = "log";

		public DataRepository( IFileRepository repository ) {
			_repository = repository;
		}

		public void SaveLog(string logText) {
			_repository.SaveFile("log", logText);

		}

		public string GetLog() {
			return _repository.LoadFile("log");
		}


	}
}
