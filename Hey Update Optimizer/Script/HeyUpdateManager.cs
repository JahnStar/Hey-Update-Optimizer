// </> Developed by Halil Emre Yildiz (All right reserved. 2023)
// Github: @JahnStar
// https://jahnstar.github.io
// Contributors: None yet
using System;
using System.Linq;
using UnityEngine;
namespace JahnStar.Optimization
{
    [DisallowMultipleComponent, AddComponentMenu("Jahn Star/Hey Update Optimizer"), HelpURL("https://github.com/JahnStar/Hey-Update-Optimizer")]
    public class HeyUpdateManager : MonoBehaviour
    {
        private static HeyUpdateManager _instance;
        public static HeyUpdateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<HeyUpdateManager>();
                    if (_instance == null) _instance = new GameObject("Hey Update Optimizer").AddComponent<HeyUpdateManager>();
                }
                return _instance;
            }
        }
        //
        [Header("Update Frequency")]
        [Tooltip("Allows modification of 'updatePerFrame' runtime ignoring 'UpdatePerFrame' in IHeyUpdate.")]
        public bool ignoreAllInterfaces = false;
        [Tooltip("Determines how often the objects will be updated per frame. (Helps to optimize the update frequency)")]
        public int updatePerFrame = 2;
        [Header("Pooling Ratio")]
        [Tooltip("Allows modification 'processPerFrame'"), Range(0, 1)]
        public float updatePoolingRatio = 0.5f;
        [Tooltip("Determines the maximum number of objects that will be updated per frame. (Helps to optimize the update load, affects the actual frame rate)")]
        private int processPerFrame = 0;
        //
        private IHeyUpdate[] updatables;
        private int[] updatePerFrameArray;
        private int _updatablesLength, _processIndex = 0, _processCounter = 0, _frameCounter = 0, _counterLimit = 99999;
        private float _updatePoolingRatio, _checkTime, _delayedTime;
        public void SetProcessPerFrame(int processPerFrame) => updatePoolingRatio = 1f - ((float)_updatablesLength / Mathf.Clamp(processPerFrame, 1, _updatablesLength));
        private void Awake() => Load();
        public void Load()
        {
            _frameCounter = _processCounter = 0;
            updatables = FindObjectsOfType<MonoBehaviour>().OfType<IHeyUpdate>().ToArray();
            _processIndex = _updatablesLength = updatables.Length;
            //
            updatePerFrameArray = new int[_updatablesLength];
            for (int i = 0; i < _updatablesLength; i++) updatePerFrameArray[i] = updatables[i].UpdatePerFrame;
            _counterLimit = ignoreAllInterfaces ? updatePerFrame : FindLCM(updatePerFrameArray);
            //
            _updatePoolingRatio = -1;
        }
        public static void ReLoad() => Instance.Load();
        private void Update()
        {
            if (_processIndex >= _updatablesLength)
            {
                _processIndex = _processCounter = 0;
                _frameCounter++;
                //
                float time = Time.time;
                _delayedTime = time - _checkTime;
                _checkTime = time;
                //
                if (_updatePoolingRatio != updatePoolingRatio) // Unnecessary cost: If you modify 'processPerFrame' this line can be removed.
                {
                    processPerFrame = updatePoolingRatio < 0.01f ? _updatablesLength : updatePoolingRatio > 0.99f ? 1 : (int)(_updatablesLength * (1f - updatePoolingRatio));
                    _updatePoolingRatio = updatePoolingRatio;
                }
            }
            if (_frameCounter >= (ignoreAllInterfaces ? updatePerFrame : _counterLimit)) _frameCounter = 0;
            while (_processIndex < _updatablesLength)
            {
                IHeyUpdate updatable;
                try { updatable = updatables[_processIndex]; }
                catch { Load(); return; }
                if (!ignoreAllInterfaces)
                {
                    updatePerFrame = updatable.UpdatePerFrame;
                    if (updatePerFrameArray[_processIndex] != updatePerFrame)
                    {
                        _counterLimit = ignoreAllInterfaces ? updatePerFrame : FindLCM(updatables.Select(u => u.UpdatePerFrame).ToArray());
                        updatePerFrameArray[_processIndex] = updatePerFrame;
                    }
                } // Unnecessary cost: If you use the 'updatePerFrame' you don't need it anymore.
                if (updatePerFrame < 2) updatePerFrame = 1; // Unnecessary cost: If you don't need this line, you can remove it.
                if (Updatable(updatePerFrame)) updatable.HeyUpdate(updatePerFrame * _delayedTime);
                _processIndex++;
                _processCounter++;
                if (_processCounter >= processPerFrame)
                {
                    _processCounter = 0;
                    break;
                }
            }
        }
        public bool Updatable(int updatePerFrame) => _frameCounter % updatePerFrame == 0;
        public static int FindLCM(int[] element_array)
        {
            int lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {
                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {
                    if (element_array[i] == 0) return 0;
                    else if (element_array[i] < 0) element_array[i] *= (-1);
                    if (element_array[i] == 1) counter++;
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] /= divisor;
                    }
                }
                if (divisible) lcm_of_array_elements *= divisor;
                else divisor++;
                if (counter == element_array.Length) return lcm_of_array_elements;
            }
        }
    }
    public interface IHeyUpdate { int UpdatePerFrame { get; } void HeyUpdate(float deltaTime); }
}