using System;
using Assets.Scripts.presentation;
using Assets.Scripts.presentation.interfaces;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CalculatorView : MonoBehaviour, ICalculatorView {


    [SerializeField]
    private InputField _inputText;

    [SerializeField]
    private TextMeshProUGUI _logText;

    [SerializeField]
    private Button _calcBtn;


    // Вызываем у презентера 
    private Action _onExitAction;
    private Action _onClickAction;


    public void AddOnClickAction(Action onClickAction) {
        _onClickAction = onClickAction;

    }

    public void OnClick() {
        _onClickAction.Invoke();
    }


    public void AddOnExitAction(Action onExit) {
        _onExitAction = onExit;
    }

    public void SetLog(string log) {
        _logText.text = log;
	}

    public void SetInputText(string txt) {
        _inputText.text = txt;
    }


    public string GetInputText() {
        return _inputText.text;
	}

    public string GetLog() {
        return _logText.text;
    }

    void OnApplicationQuit() {
        _onExitAction();

    }





}
