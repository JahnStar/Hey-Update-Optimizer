// Github: @JahnStar
using UnityEngine;
using JahnStar.Optimization;
public class HeyClock : MonoBehaviour, IHeyUpdate
{
    [Header("Clock")]
    public Transform pendulum;
    public Transform hour, minutes, secons;
    public float timeSpeed = 1;
    public float pendulumAngle = 5f;
    //[Range(1, 60)]
    //public int updatePerFrame = 60; 
    private float _elapsedTime;
    [Space]
    public int updatePerFrame = 2;
    public bool unityUpdateMode = false;
    public int UpdatePerFrame { get => updatePerFrame; }
    private bool enable;
    public void Update()
    {
        if (unityUpdateMode) HeyUpdate(Time.deltaTime);
    }
    public void HeyUpdate(float deltaTime)
    {
        if (!enable) return;
        _elapsedTime += deltaTime * timeSpeed;
        Vector3 _timeNormal = Vector3.forward * _elapsedTime;
        secons.localRotation = Quaternion.Euler(_timeNormal *= 6f);
        minutes.localRotation = Quaternion.Euler(_timeNormal /= 60f);
        hour.localRotation = Quaternion.Euler(_timeNormal / 12f);
        pendulum.localRotation = Quaternion.Euler(Mathf.Sin(_elapsedTime * Mathf.PI * 2) * pendulumAngle * Vector3.forward);
    }
    private void OnEnable()
    {
        enable = true;
    }
    private void OnDisable()
    {
        enable = false;
    }
}