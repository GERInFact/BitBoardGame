using System;
using System.Collections.Generic;
using System.Linq;
using Assets._scripts.ExtensionMethods;
using Assets._scripts.Manager;
using Assets._scripts.Model.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._scripts.Model
{
    public class MapCreator : MonoBehaviour, IMapCreator
    {
        #region  Member

        [SerializeField] private List<GameObject> _environmentPrefabs = new List<GameObject>();

        [SerializeField] private int _height;

        [SerializeField] private float _plantingSpeed;

        [SerializeField] private List<GameObject> _housePrefabs = new List<GameObject>();

        [SerializeField] private List<GameObject> _tilePrefabs = new List<GameObject>();

        [SerializeField] private int _width;

        #endregion

        #region  Implemented Interfaces

        public void Create()
        {
            this.BuildBoard();
        }

        public void Delete()
        {
        }

        public void Update()
        {
            this.SpawnPlayerHouse();
        }

        #endregion

        private void BuildBoard()
        {
            for (var i = 0; i < this._width * this._height; i++)
            {
                var tile = this.InstantiateTiles(i);
                this.SetTileName(tile, i);
                BitBoardManager.SetBitInBoard((BoardType)Enum.Parse(typeof(BoardType), tile.tag), i);
            }

            this.InvokeRepeating("PlantEnvironment", 1f, this._plantingSpeed);
        }

        private static GameObject GetRandomPrefab(IReadOnlyList<GameObject> prefabsList)
        {
            return !prefabsList.Any() ? new GameObject("Default") : prefabsList[Random.Range(0, prefabsList.Count)];
        }

        private void InstantiateHouse(RaycastHit hit)
        {
            var house = Instantiate(this._housePrefabs[Random.Range(0, this._housePrefabs.Count)]);
            house.transform.parent = hit.collider.transform;
            house.transform.localPosition = Vector3.zero;
        }

        private GameObject InstantiateTiles(int index)
        {
            var position = index.GetAsVector2(new GridInfo { CellSize = Vector2.one, Width = this._width });
            return Instantiate(GetRandomPrefab(this._tilePrefabs), new Vector3(position.x, 0, position.y),
                Quaternion.identity);
        }

        private bool IsCellEmptyLot(RaycastHit hit)
        {
            var index = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.z)
                .GetAsIndex(new GridInfo { CellSize = Vector2.one, Width = this._width });
            return (BitBoardManager.GetFreeDirtCellMask() >> index & 1L) == 1L;
        }

        private void PlantEnvironment()
        {
            var index = Random.Range(0, this._width * this._height);

            if (!this._environmentPrefabs.Any()
            || (BitBoardManager.GetFreeDirtCellMask() & 1L << index) == 0) return;

            var position = index.GetAsVector2(new GridInfo { CellSize = Vector2.one, Width = this._width });
            var environment = Instantiate(this._environmentPrefabs[Random.Range(0, this._environmentPrefabs.Count)],
                new Vector3(position.x, 0, position.y), Quaternion.identity);

            BitBoardManager.SetBitInBoard(BoardType.Woods, index);
        }

        private void SetTileName(GameObject tile, int index)
        {
            tile.name =
                $"{tile.tag}_{index.GetAsVector2(new GridInfo { CellSize = Vector2.one, Width = this._width }).ToString()}";
        }

        private void SpawnPlayerHouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (this._housePrefabs?.Any() != true || !Input.GetMouseButtonDown(0) ||
                !Physics.Raycast(ray, out var hit) || !this.IsCellEmptyLot(hit)) return;

            this.InstantiateHouse(hit);

            BitBoardManager.SetBitInBoard(BoardType.PlayerBoard,
                new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.z)
                    .GetAsIndex(new GridInfo { CellSize = Vector2.one, Width = this._width }));
        }
    }
}