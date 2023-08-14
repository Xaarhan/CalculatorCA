namespace Assets.Scripts.domain {
	public interface IDataRepositoryGateway {
		void SaveLog(string logText);
		string GetLog();
		
	}
}
