using System;
using Common;
using Common.StaticData;
using Common.UnityLogic.Units;
using UnityEditor;
using UnityEngine;

namespace Editor.UnitsEditor
{
    public sealed class UnitsEditorWindow : EditorWindow
    {
        private string _name;
        private float _hp;
        private float _damage;
        private float _diagonalDamageMultiplier;
        private int _range;
        private Unit _prefab;
        
        [MenuItem("Window/Units Editor")]
        public static void ShowWindow() => GetWindow<UnitsEditorWindow>(nameof(UnitsEditorWindow));

        private void OnGUI()
        {
            GUILayout.Label("Unit Data");
            _name = EditorGUILayout.TextField("Name", _name?.ToUpper());

            DrawProperty("HP", () => _hp = EditorGUILayout.Slider(_hp, 1, 100_000));
            DrawProperty("Damage", () => _damage = EditorGUILayout.Slider(_damage, 0.1f, 100_000));
            DrawProperty("Diagonal damage multiplier",
                () => _diagonalDamageMultiplier = EditorGUILayout.Slider(_diagonalDamageMultiplier, 0.1f, 1));
            DrawProperty("Range", () => _range = EditorGUILayout.IntSlider(_range, 1, 10));
            DrawProperty("Prefab",
                () => _prefab = (Unit)EditorGUILayout.ObjectField(_prefab, typeof(Unit), true));

            GUILayout.Space(50);
            
            if (GUILayout.Button("Create/Update Data")) CreateUnitData();

            if (GUILayout.Button("Delete Data")) TryDeleteData();
        }

        private void DrawProperty(in string label, Action action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            action?.Invoke();
            GUILayout.EndHorizontal();
        }
        
        private void CreateUnitData()
        {
            CheckNameData();
            if (_prefab is null) throw new Exception("No prefab data");
            
            CheckAlreadyCreatingAsset();
            var staticData = CreateInstance<UnitStaticData>();
            staticData.name = _name;
            staticData.UnitName = _name;
            staticData.HP = _hp;
            staticData.Damage = _damage;
            staticData.DiagonalAttackMultiplier = _diagonalDamageMultiplier;
            staticData.Range = _range;
            staticData.Unit = _prefab;
            
            AssetDatabase.CreateAsset(staticData, $"{Constants.UnitDataPath.GlobalPath}/{Constants.UnitDataPath.LocalPath}/{staticData.name}.asset");
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = staticData;
        }
        
        private void TryDeleteData()
        {
            CheckNameData();

            var unitStaticData = GetResourcesStaticData();
            if (unitStaticData is not null)
            {
                DeleteData();
            }
            else
            {
                Debug.LogWarning($"No data with name: {_name}");
            }
        }
        
        private void DeleteData()
        {
            AssetDatabase.DeleteAsset(
                $"{Constants.UnitDataPath.GlobalPath}/{Constants.UnitDataPath.LocalPath}/{_name}.asset");
        }
        
        private void CheckAlreadyCreatingAsset()
        {
            var unitStaticData = GetResourcesStaticData();
            if (unitStaticData is not null) DeleteData();
        }

        private UnitStaticData GetResourcesStaticData()
        {
            var unitStaticData = Resources.Load<UnitStaticData>($"{Constants.UnitDataPath.LocalPath}/{_name}");
            return unitStaticData;
        }
        
        private void CheckNameData()
        {
            if (string.IsNullOrWhiteSpace(_name)) throw new Exception("Incorrect unit name");
        }
    }
}