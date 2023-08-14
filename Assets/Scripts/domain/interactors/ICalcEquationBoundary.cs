using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.domain.interactors {
	public interface ICalcEquationBoundary {

		bool CalcStringEquation(string equation, out int Result);

	}
}
