using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _completeGamePanel;
    [SerializeField] private GameObject _failGamePanel;
    
    public Button startGameButton;

    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIController>();
            }

            return _instance;
        }

        private set => _instance = value;
    }

    public Action OnStartGameButtonPressed = delegate {  };

    private void Awake()
    {
        _instance = this;
        
        startGameButton.onClick.AddListener(() => OnStartGameButtonPressed());
    }

    private void OnEnable()
    {
        GameController.Instance.OnGameCompleted += OnGameCompleted;
        GameController.Instance.OnGameFailed += OnGameFailed;
    }

    private void OnDisable()
    {
        GameController.Instance.OnGameCompleted -= OnGameCompleted;
        GameController.Instance.OnGameFailed -= OnGameFailed;
    }

    private void OnGameCompleted()
    {
        _completeGamePanel.SetActive(true);
    }

    private void OnGameFailed()
    {
        _failGamePanel.SetActive(true);
    }
}
