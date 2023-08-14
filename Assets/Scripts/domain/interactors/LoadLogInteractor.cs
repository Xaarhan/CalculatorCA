namespace Assets.Scripts.domain.interactors {
	public class LoadLogInteractor : ILoadLogBoundary {

		private IDataRepositoryGateway _repository;

		public LoadLogInteractor(IDataRepositoryGateway repo) {
			_repository = repo;
		}

		public string LoadLog() {
			return _repository.GetLog();
		}


	}
}
