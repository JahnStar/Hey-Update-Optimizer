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
        public bool applyChanges;
        [Header("Update Frequency")]
        [Tooltip("Allows modification of 'updatePerFrame' runtime ignoring 'UpdatePerFrame' in IHeyUpdate.")]
        public bool ignoreAllInterfaces = false;
        private bool _ignoreAllInterfaces;
        [Tooltip("Determines how often the objects will be updated per frame. (Helps to optimize the update frequency)")]
        public int updatePerFrame = 2;
        private int _updatePerFrame;
        [Header("Pooling Ratio")]
        [Tooltip("Allows modification 'processPerFrame'"), Range(0, 1)]
        public float updatePoolingRatio = 0.5f;
        public float _updatePoolingRatio;
        [Tooltip("Determines the maximum number of objects that will be updated per frame. (Helps to optimize the update load, affects the actual frame rate)")]
        private int processPerFrame = 0;
        //
        private IHeyUpdate[] updatables;
        private int[] _updatePerFrameArray;
        private int _updatablesLength, _processIndex = 0, _processCounter = 0, _frameCounter = 0, _counterLimit = 99999;
        private float _checkTime, _delayedTime;
        public void SetProcessPerFrame(int processPerFrame) => _updatePoolingRatio = 1f - ((float)_updatablesLength / Mathf.Clamp(this.processPerFrame = processPerFrame, 1, _updatablesLength));
        private void Awake() => Load();
        internal void Load()
        {
            updatables = FindObjectsOfType<MonoBehaviour>().OfType<IHeyUpdate>().ToArray();
            _processIndex = _updatablesLength = updatables.Length;
            //
            _ignoreAllInterfaces = ignoreAllInterfaces;
            _updatePerFrame = updatePerFrame;
            _updatePoolingRatio = updatePoolingRatio;
            //
            processPerFrame = _updatePoolingRatio < 0.01f ? _updatablesLength : _updatePoolingRatio > 0.99f ? 1 : (int)(_updatablesLength * (1f - _updatePoolingRatio));
            _updatePerFrameArray = updatables.Select(u => u.UpdatePerFrame).ToArray();
            _counterLimit = _ignoreAllInterfaces ? _updatePerFrame : FindLCM(_updatePerFrameArray);
            if (_updatePerFrame < 2) _updatePerFrame = 1;
            applyChanges = false;
        }
        public static void ReLoad() => Instance.applyChanges = true;
        private void Update()
        {
            if (_processIndex >= _updatablesLength)
            {
                _frameCounter++;
                //
                float time = Time.time;
                _delayedTime = time - _checkTime;
                _checkTime = time;
                //
                if (applyChanges) Load();
                _processIndex = _processCounter = 0;
            }
            if (_frameCounter >= _counterLimit) _frameCounter = 0;
            while (_processIndex < _updatablesLength)
            {
                try
                {
                    if (!_ignoreAllInterfaces)
                    {
                        _updatePerFrame = _updatePerFrameArray[_processIndex];
                        if (_updatePerFrame < 2) _updatePerFrame = 1;
                    }
                    if (Updatable(_updatePerFrame)) updatables[_processIndex].HeyUpdate(_updatePerFrame * _delayedTime);
                }
                catch { applyChanges = true; return; }
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