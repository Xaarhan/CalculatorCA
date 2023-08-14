namespace Assets.Scripts.domain.interactors {
	public class SaveLogInteractor : ISaveLogBoundary {

		private IDataRepositoryGateway _repository;

		public SaveLogInteractor(IDataRepositoryGateway repo) {
			_repository = repo;
		}

		public void SaveLog(string saveData) {
			_repository.SaveLog(saveData);
		}


	}
}
