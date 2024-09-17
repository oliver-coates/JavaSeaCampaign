using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace KahuInteractive.UIHelpers
{


public class BasicSelectionManager : MonoBehaviour
{

    private static BasicSelectionManager _Instance;

    [Header("UI References:")]
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _Instance = this;
        Hide();
    }

    private void Show()
    {       
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    public static void RequestSelection<T>(SelectionCategory<T>[] categories, Action<T> onFulfilled)
    {    
        foreach (SelectionCategory<T> category in categories)
        {
            if (category.options.Length == 0)
            {
                continue;
            }

            // Create Category divider

            foreach (SelectionOption<T> option in category.options)
            {
                // Create option button
                Debug.Log($"Creating option {option.displayText} under category {category.categoryName}");
            }
        }

        _Instance.Show();
    }


    public struct SelectionCategory<T>
    {
        public string categoryName;
        public SelectionOption<T>[] options;

        public SelectionCategory(string name, SelectionOption<T>[] options)
        {
            categoryName = name;
            this.options = options;
        }
    }

    public struct SelectionOption<T>
    {
        public T value;
        public string displayText;

        public SelectionOption(string displayText, T value)
        {
            this.displayText = displayText;
            this.value = value;
        }
    }


}

}