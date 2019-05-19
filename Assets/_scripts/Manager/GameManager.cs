using System;
using Assets._scripts.Model;
using Assets._scripts.Model.Interfaces;
using TMPro;
using UnityEngine;

namespace Assets._scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region  Member

        private IMapCreator _mapCreator;

        [SerializeField] private TextMeshProUGUI _text;

        #endregion

        private void Awake()
        {
            this._mapCreator = FindObjectOfType<MapCreator>() ?? this.gameObject.AddComponent<MapCreator>();
            this._text = FindObjectOfType<TextMeshProUGUI>() ?? throw new ArgumentNullException(nameof(this._text));
        }

        private void Start()
        {
            this._mapCreator.Create();
        }

        private void Update()
        {
            this._mapCreator.Update();
            this._text.text = $"Score: {BitBoardManager.GetCellCount(BitBoardManager.PlayerBoard64)}";
        }
    }
}