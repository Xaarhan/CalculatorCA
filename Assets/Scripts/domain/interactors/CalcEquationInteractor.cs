using System;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.domain.interactors;

namespace Assets.Scripts.domain.interactors {
	public class CalcEquationInteractor : ICalcEquationBoundary {

		private ICalculator _calculator;

		public CalcEquationInteractor( ICalculator calc) {
			_calculator = calc;
		}


		public bool CalcStringEquation(string equation, out int result) {
			result = 0;

			// Проверки что стринг правильный можно было бы вынести в отдельный модуль
			if ( IsWrongEnter(equation) ) {
				return false;
			}

			string[] strValues = equation.Split('+');
			if ( strValues.Length != 2 ) {
				return false;
			}

			int val1 = 0;
			if ( !int.TryParse(strValues[0], out val1) ) {
				return false;
			}

			int val2 = 0;
			if ( !int.TryParse(strValues[1], out val2) ) {
				return false;
			}

			result = _calculator.Calculate(val1, val2);
			return true;
		}

		public bool IsWrongEnter(string s) {
			// Тут можно было бы обращаться к отельному классу который бы проверял правильность ввода
			bool haveWrongChars = !Regex.IsMatch(s, @"^[0-9+]+$");
			int addCount = s.Count(f => (f == '+'));
			int index = s.IndexOf("+");
			return haveWrongChars || addCount != 1 || index < 1 || index > s.Length - 2;
		}




	}
}
