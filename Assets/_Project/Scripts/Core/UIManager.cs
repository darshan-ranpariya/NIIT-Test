using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;
    public static event Action<int> OnNavigationStackChanged;
    [SerializeField] private List<BaseScreen> screenPrefabs;
    [SerializeField] private Transform screenParent;

    private List<BaseScreen> _instantiatedScreens;
    private readonly Stack<BaseScreen> _navigationStack = new Stack<BaseScreen>();

    private void Awake()
    {
        inst = this;
        _instantiatedScreens = new List<BaseScreen>();
        foreach (var screenPrefab in screenPrefabs)
        {
            var screenInstance = Instantiate(screenPrefab, screenParent);
            screenInstance.gameObject.name = screenPrefab.name;
            screenInstance.Hide();
            _instantiatedScreens.Add(screenInstance);
        }
    }

    private void Start()
    {
        var welcomeScreen = GetScreen<WelcomeScreen>();
        if (welcomeScreen != null)
        {
            welcomeScreen.Show();
            _navigationStack.Push(welcomeScreen);
            OnNavigationStackChanged?.Invoke(_navigationStack.Count);
        }
    }

    public T GetScreen<T>() where T : BaseScreen
    {
        return _instantiatedScreens.FirstOrDefault(s => s is T) as T;
    }

    public void ShowScreen<T>() where T : BaseScreen
    {
        var screenToShow = GetScreen<T>();
        if (screenToShow == null) return;

        if (_navigationStack.Count > 0)
        {
            _navigationStack.Peek().Hide();
        }

        screenToShow.Show();
        _navigationStack.Push(screenToShow);
        OnNavigationStackChanged?.Invoke(_navigationStack.Count);
    }

    public void GoBack()
    {
        if (_navigationStack.Count > 1)
        {
            var currentScreen = _navigationStack.Pop();
            currentScreen.Hide();

            var previousScreen = _navigationStack.Peek();
            previousScreen.Show();
            OnNavigationStackChanged?.Invoke(_navigationStack.Count);
        }
    }
}
