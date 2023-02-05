// Github: @JahnStar
using JahnStar.Optimization;
using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour
{
    public HeyUpdateManager heyUpdateOptimizer;
    public RuntimeSpawner runtimeSpawner;
    public Text timeText, fpsText, countText;
    private float _deltaTime = 0.0f;
    private Transform _camera;
    private Vector3 startPos;
    private float _elapsedTime = 1;
    public float cameraSpeed = 0.01f;
    private void Start()
    {
        EnableOptimizer();
        // Camera
        float _middle = runtimeSpawner.groupCount * runtimeSpawner.distance / 2f;
        startPos = new Vector3(_middle - runtimeSpawner.distance / 2f, 0, _middle);
        _elapsedTime = 1;
        _camera = Camera.main.transform;
        countText.text = runtimeSpawner.clones.Count + 1 + " Object";
    }
    public void Update()
    {
        timeText.text = $"Absolute Time: {Time.time:0.0}";
        //
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        // Camera
        if (_camera) _camera.position = new Vector3(startPos.x, startPos.y, startPos.z * (_elapsedTime -= Time.deltaTime * cameraSpeed));
        RenderSettings.skybox.SetFloat("_Rotation", _elapsedTime * 360);
        if (_elapsedTime <= 0) _elapsedTime = 1;
    }
    public void EnableOptimizer()
    {
        heyUpdateOptimizer.enabled = true;
        runtimeSpawner.clock.GetComponent<HeyClock>().unityUpdateMode = false;
        foreach (GameObject clone in runtimeSpawner.clones) clone.GetComponent<HeyClock>().unityUpdateMode = false;
    }
    public void DisableOptimizer()
    {
        heyUpdateOptimizer.enabled = false;
        runtimeSpawner.clock.GetComponent<HeyClock>().unityUpdateMode = true;
        foreach (GameObject clone in runtimeSpawner.clones) clone.GetComponent<HeyClock>().unityUpdateMode = true;
    }
}
