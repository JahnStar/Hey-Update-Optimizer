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

        internal bool NeedApply() => _updatePoolingRatio != updatePoolingRatio || _updatePerFrame != updatePerFrame || _ignoreAllInterfaces != ignoreInterfaces;
        //
        public bool reload;
        [Header("Update Frequency")]
        public bool ignoreInterfaces = false;
        private bool _ignoreAllInterfaces;
        public int updatePerFrame = 2;
        private int _updatePerFrame;
        [Header("Pooling Ratio")]
        [Range(0, 1)]
        public float updatePoolingRatio = 0.5f;
        private float _updatePoolingRatio;
        internal int processPerFrame = 0;
        //
        internal IHeyUpdate[] updatables;
        internal int _updatablesLength = 0;
        [SerializeField] private int[] _updatePerFrameArray;
        [SerializeField] private float[] _checkTimeArray;
        [SerializeField] private int _processIndex = 0, _processCounter = 0, _frameCounter = 0, _counterLimit = 99999;
        private float _checkTime, _delayedTime;
        private bool _update;
        public bool _editorInfo = true;
        public void SetProcessPerFrame(int processPerFrame) => _updatePoolingRatio = 1f - ((float)_updatablesLength / Mathf.Clamp(this.processPerFrame = processPerFrame, 1, _updatablesLength));
        private void OnEnable()
        {
            reload = true;
            _delayedTime = 0;
            _checkTimeArray = (new float[_updatablesLength]).Select(u => _checkTime = Time.time).ToArray();
        }
        private void Awake()
        {
            reload = true;
            Load();
        }
        internal bool Load()
        {
            if (!reload) return false;
            _ignoreAllInterfaces = ignoreInterfaces;
            _updatePerFrame = updatePerFrame;
            _updatePoolingRatio = _ignoreAllInterfaces ? updatePoolingRatio : updatePoolingRatio = 0;
            if (_updatePerFrame < 2) _updatePerFrame = 1;
            //
            updatables = FindObjectsOfType<MonoBehaviour>().OfType<IHeyUpdate>().ToArray();
            _processIndex = _updatablesLength = updatables.Length;
            //
            processPerFrame = _updatePoolingRatio < 0.01f ? _updatablesLength : _updatePoolingRatio > 0.99f ? 1 : (int)(_updatablesLength * (1f - _updatePoolingRatio));
            _updatePerFrameArray = updatables.Select(u => u.UpdatePerFrame < 2 ? 1 : u.UpdatePerFrame).ToArray();
            _counterLimit = _ignoreAllInterfaces ? _updatePerFrame : FindLCM((int[])_updatePerFrameArray.Clone());
            _frameCounter = 0;
            reload = false;
            _checkTimeArray = (new float[_updatablesLength]).Select(u => _checkTime = Time.time).ToArray();
            return true;
        }
        public static void ReLoad() => Instance.reload = true;
        private void Update()
        {
            if (_processIndex >= _updatablesLength)
            {
                _frameCounter++;
                _processIndex = _processCounter = 0;
            }
            if (_ignoreAllInterfaces) _update = Updatable(_updatePerFrame);
            if (_frameCounter >= _counterLimit) _frameCounter = 0;
            while (_processIndex < _updatablesLength)
            {
                try
                {
                    if (!_ignoreAllInterfaces)
                    {
                        _update = Updatable(_updatePerFrameArray[_processIndex]);
                        if (_update) if (_processIndex == 0 && Load()) return;
                        else UpdateTime(ref _checkTimeArray[_processIndex]);
                    }
                    else if (_processIndex == 0 && _update)
                    {
                        if (Load()) return;
                        else UpdateTime(ref _checkTime);
                    }

                    if (_update) updatables[_processIndex].HeyUpdate(_delayedTime);
                }
                catch (Exception e) { reload = true; Debug.LogWarning(e.Message); return; }
                _processIndex++;
                _processCounter++;
                if (_processCounter >= processPerFrame)
                {
                    _processCounter = 0;
                    break;
                }
            }
        }
        public void UpdateTime(ref float checkTime)
        {
            float time = Time.time;
            _delayedTime = time - checkTime;
            checkTime = time;
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