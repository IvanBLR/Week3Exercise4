using System.Collections.Generic;
using UnityEngine;
public class MapBuilder : MonoBehaviour
{
    [SerializeField]
    private Transform _parentForTiles;

    [SerializeField]
    private PlanePositions _planePositions;

    [SerializeField]
    private Material _red;

    [SerializeField]
    private Material _green;

    private Dictionary<MeshRenderer, Material> _defaultMaterials = new Dictionary<MeshRenderer, Material>();

    private GameObject _currentTile;
    private Vector3 _currentPoint;
    private int[,] _planePoints = new int[10, 10];

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo) && _currentTile != null)
        {
            var point = _planePositions.GetPlanePoint(hitInfo.point);// вычисления корректные
            _currentPoint = point;

            if (_currentTile.transform.localPosition != point)
            {
                LightningCurrentTile(_currentTile, point);
            }
            _currentTile.transform.localPosition = point;
        }
     
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentTile != null && CanPushCoordinates())
            {
                ReturnDefautMaterial(_currentTile);
                _planePoints[(int)_currentPoint.x, (int)Mathf.Abs(_currentPoint.z)] = 1;
                _currentTile = null;
            }
            return;
        }
    }

    private bool CanPushCoordinates()
    {
        var x = (int)_currentPoint.x;
        var y = (int)_currentPoint.z;

        if (x < 0 || x > 9 || y > 0 || y < -9 ||
            _planePoints[x, Mathf.Abs(y)] == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
    /// В качестве параметра передается префаб тайла, изображенный на кнопке.
    /// Вы можете использовать префаб tilePrefab внутри данного метода.
    /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        if (_currentTile != null)
        {
            return;
        }
        var pointForTile = _planePositions.GetPlanePoint(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        _currentTile = Instantiate(tilePrefab, _parentForTiles);

        _currentTile.transform.position = _planePositions.GetPlanePoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        var materials = _currentTile.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < materials.Length; i++)
        {
            if (!_defaultMaterials.ContainsKey(materials[i]))
            {
                _defaultMaterials.Add(materials[i], materials[i].material);
            }
        }
    }

    private void LightningCurrentTile(GameObject tile, Vector3 point)
    {
        var childrenRenderer = tile.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < childrenRenderer.Length; i++)
        {
            if (!CanPushCoordinates())
            {
                childrenRenderer[i].material = _red;
            }
            else
            {
                childrenRenderer[i].material = _green;
            }
        }
    }
    private void ReturnDefautMaterial(GameObject tile)
    {
        var mesh = tile.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < mesh.Length; i++)
        {
            if (_defaultMaterials.ContainsKey(mesh[i]))
            {
                mesh[i].material = _defaultMaterials.GetValueOrDefault(mesh[i]);
            }
        }
    }
}