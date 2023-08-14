using Assets.Scripts.presentation.interfaces;

namespace Assets.Scripts.presentation {
	public class CalculatorPresenter : ICalculatorPresenter {

		private ICalculatorView _view;
		private ICalculatorModel _model;
	
		public CalculatorPresenter( ICalculatorModel model, ICalculatorView view) {
			_view = view;
			_model = model;
			_view.AddOnClickAction(Calculate);
			_view.AddOnExitAction(SaveLog);
		}


		public void Calculate() {
			int calcResult = 0;
			string ecuation = _view.GetInputText();
			string log = _view.GetLog();
			if ( _model.CalculateStringEcuation(ecuation, out calcResult )) {
				 ecuation += "=" + calcResult.ToString();
			} else {
				 ecuation += "=Error";
			}

			log = ecuation + "\r\n" + log;
			_view.SetInputText("");
			_view.SetLog(log);
		}


		public void SaveLog() {
			string logForSave = _view.GetInputText()+";" + _view.GetLog();
			logForSave = logForSave.Replace("\r\n", "");
			_model.SaveLog(logForSave);
		}


		public void Init() {
			string log = _model.LoadAll();
			string[] loglines = log.Split(';'); // из за того что для разбиения используется ; то
												// можно сломать запись в логе если вводить ; в калькуляторе 

			if ( loglines.Length == 0 ) return;
			_view.SetInputText(loglines[0]);

			string s = "";
			for ( int i = 1; i < loglines.Length; i++ ) {
				  s += loglines[i] + "\r\n";
			}
			_view.SetLog(s);
			
		}

	}
}
