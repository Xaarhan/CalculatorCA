using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.presentation.interfaces;
using Assets.Scripts.domain.interactors;

namespace Assets.Scripts.presentation {
	public class CalculatorModel : ICalculatorModel {

		private ICalcEquationBoundary _calculator;
		private ILoadLogBoundary _logLoader;
		private ISaveLogBoundary _logSaver;


		public CalculatorModel( ICalcEquationBoundary calculator, ILoadLogBoundary logLoader, ISaveLogBoundary logSaver) {
			_calculator = calculator;
			_logLoader = logLoader;
			_logSaver = logSaver;
		}


		//Тут надо бы модель данных с результатом передавать
		public bool CalculateStringEcuation(string ecuation, out int result) {
			return _calculator.CalcStringEquation(ecuation, out result);
		}

		public string LoadAll() {
			return _logLoader.LoadLog();
		}

		public void SaveLog(string log) {
			_logSaver.SaveLog(log);
		}

	}
}
