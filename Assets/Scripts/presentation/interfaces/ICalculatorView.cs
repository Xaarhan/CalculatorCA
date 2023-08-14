using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.presentation.interfaces {
	public interface ICalculatorView {

		void AddOnClickAction(Action onClick);
		void AddOnExitAction(Action onExit);

		void SetLog(string log);

		string GetLog();

		void SetInputText(string inputText);
		string GetInputText();

	}
}
