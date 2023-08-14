namespace Assets.Scripts.presentation.interfaces {
	public interface ICalculatorModel {

		bool CalculateStringEcuation(string ecuation, out int result);

		string LoadAll();

		void SaveLog( string log );

	}
}
