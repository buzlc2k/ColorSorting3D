using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ColorSorting3D.Config;
using ServiceButcator;
using UnityEditor;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class LevelEditorManager : MonoBehaviour
    {
        // External Depedencies
        private Camera mainCamera;
        private GridManager gridManager;
        private RingsContainerManager containerManager;
        private RingManager ringManager;
        private LevelConfig levelConfig;
        private UIServices uiServices;

        private void Start()
           => LoadDepedencies();

        private void LoadDepedencies()
        {
            mainCamera = ServiceLocators.SceneOf(this).Get<Camera>();
            gridManager = ServiceLocators.SceneOf(this).Get<GridManager>();
            containerManager = ServiceLocators.SceneOf(this).Get<RingsContainerManager>();
            ringManager = ServiceLocators.SceneOf(this).Get<RingManager>();
            levelConfig = ServiceLocators.SceneOf(this).Get<LevelConfig>();
            uiServices = ServiceLocators.SceneOf(this).Get<UIServices>();
        }

        private bool TryGetLevelFromInput(out int currentLevel)
        {
            currentLevel = uiServices.GetLevelInput();
            if (currentLevel == -1)
            {
                Debug.Log("LevelEditor: Invalid Level Format");
                return false;
            }

            return true;
        }

        public void Load()
        {
            if (!TryGetLevelFromInput(out var currentLevel))
                return;

            if (!TryGetLevel(out var levelConfigRecord))
                return;

            Clear();

            SetCameraPos();
            foreach (var savedRingsContainer in levelConfigRecord.Containers)
                LoadSavedRingsContainer(savedRingsContainer);

            Debug.Log($"LevelEditor: Level {currentLevel} loaded successfully!");

            bool TryGetLevel(out LevelConfigRecord levelConfigRecord)
            {
                levelConfigRecord = levelConfig.Get(currentLevel);
                if (levelConfigRecord != null)
                    return true;

                Debug.LogWarning($"LevelEditor: Level {currentLevel} does not exist!");
                return false;
            }
            
            void LoadSavedRingsContainer(SavedRingsContainer savedRingsContainer)
            {
                var cell = gridManager.GetCell(savedRingsContainer.Position);
                containerManager.Spawn(cell, savedRingsContainer.Size, savedRingsContainer.Available);
                ringManager.Spawn(cell, savedRingsContainer.Rings);
            }

            void SetCameraPos()
            {
                var cameraPos = levelConfigRecord.CameraPos;
                mainCamera.transform.position = cameraPos;
                uiServices.SetPosSliderValue(cameraPos.y); // y is the absolute of camera pos
            }
        }   

        public void Save()
        {
            if (!TryGetLevelFromInput(out var currentLevel))
                return;

            RemoveLegacyRecord();

            List<SavedRingsContainer> savedRingsContainers = GetContainers(out var targetContainerFinished);

            var newRecord = new LevelConfigRecord(currentLevel, savedRingsContainers, targetContainerFinished, mainCamera.transform.position);
            levelConfig.Add(newRecord);

            EditorUtility.SetDirty(levelConfig);
            AssetDatabase.SaveAssets();

            Debug.Log($"Level {currentLevel} saved successfully!");

            void RemoveLegacyRecord()
            {
                var existingRecord = levelConfig.Get(currentLevel);
                if (existingRecord == null)
                    return;

                Debug.Log($"Overriding existing level {currentLevel}");
                levelConfig.Remove(existingRecord);
            }

            List<SavedRingsContainer> GetContainers(out int targetContainerFinished)
            {
                HashSet<RingID> ringIDs = new();
                List<SavedRingsContainer> savedRingsContainers = new();

                foreach (var cell in gridManager.GetAllCells())
                {
                    if (cell.Available)
                        continue;

                    List<RingID> containerRingIDs = cell.Rings.Select(ring => ring.GetID()).ToList();

                    savedRingsContainers.Add(new SavedRingsContainer(
                        cell.WorldPos, 
                        cell.Container.GetSize(), 
                        cell.Container.GetAvailable(),
                        containerRingIDs
                    ));

                    ringIDs.UnionWith(containerRingIDs);
                }

                targetContainerFinished = ringIDs.Count;
                return savedRingsContainers;
            }
        }

        public void Clear()
        {
            foreach (var cell in gridManager.GetAllCells())
            {
                if (cell.Available)
                    continue;

                cell.RemoveContainer();
            }
        }
    }
}