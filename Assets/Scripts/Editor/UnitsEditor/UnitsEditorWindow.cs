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
            _name = EditorGUILayout.TextField("Name", _name);

            DrawProperty("HP", () => _hp = EditorGUILayout.Slider(_hp, 1, 100_000));
            DrawProperty("Damage", () => _damage = EditorGUILayout.Slider(_damage, 0.1f, 100_000));
            DrawProperty("Diagonal damage multiplier",
                () => _diagonalDamageMultiplier = EditorGUILayout.Slider(_diagonalDamageMultiplier, 0.1f, 1));
            DrawProperty("Range", () => _range = EditorGUILayout.IntSlider(_range, 1, 10));
            DrawProperty("Prefab",
                () => _prefab = (Unit)EditorGUILayout.ObjectField(_prefab, typeof(Unit), true));

            if (GUILayout.Button("Create Data"))
            {
                TryCreateUnitData();
            }
        }
        private void DrawProperty(in string label, Action action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            action?.Invoke();
            GUILayout.EndHorizontal();
        }
        private void TryCreateUnitData()
        {
            if (string.IsNullOrWhiteSpace(_name)) throw new Exception("Incorrect unit name");
            if (ResourceAlreadyExists()) throw new Exception($"Resource with name {_name} already created");
            if (_prefab is null) throw new Exception("No prefab data");
            
            var staticData = CreateInstance<UnitStaticData>();
            staticData.name = GetUnitDataName();
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
        private bool ResourceAlreadyExists()
        {
            var resource = Resources.Load($"{Constants.UnitDataPath.LocalPath}/{GetUnitDataName()}");
            return resource is not null;
        }
        private string GetUnitDataName() => _name;
    }
}