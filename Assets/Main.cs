using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.domain.interactors;
using Assets.Scripts.data;
using Assets.Scripts.presentation;

public class Main : MonoBehaviour
{
    [SerializeField]
    private CalculatorView _calcView;
    void Start() {
        AddCalculator calculator = new AddCalculator();
        FileRepository fileRepo = new FileRepository();

        DataRepository dataRepo = new DataRepository(fileRepo);

        CalcEquationInteractor calcIteractor = new CalcEquationInteractor(calculator);
        LoadLogInteractor loadLog = new LoadLogInteractor(dataRepo); // вместо loadlog и savelog мне лучше бы придумать другое название
        SaveLogInteractor saveLog = new SaveLogInteractor(dataRepo);

        CalculatorModel model = new CalculatorModel(calcIteractor, loadLog, saveLog);
        CalculatorPresenter presenter = new CalculatorPresenter(model, _calcView);
        presenter.Init();
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
