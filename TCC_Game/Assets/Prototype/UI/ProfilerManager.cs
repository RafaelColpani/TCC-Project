using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

/*
 * FPS Counter logic based on Code Monkey's video: https://youtu.be/I2r97r9h074
 * 
 *
 */

public class ProfilerManager : MonoBehaviour
{
    string statsText;
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder gpuMemoryRecorder;


    public TMPro.TextMeshProUGUI fpsText;

    int lastFrameIndex;
    float[] frameDeltaTimeArray;


    static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;

            var samples = new List<ProfilerRecorderSample>(samplesCount);
            recorder.CopyTo(samples);
            for (var i = 0; i < samples.Count; ++i)
                r += samples[i].Value;
            r /= samplesCount;

        return r;
    }

    void OnEnable()
    {
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
        gpuMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "gpu");
    }

    void OnDisable()
    {
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
    }
    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }

    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        fpsText.text = Mathf.RoundToInt(FpsCounter()).ToString();

        var sb = new StringBuilder(500);
        sb.AppendLine($"Frame Time: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");
        sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
        statsText = sb.ToString();
    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(10, 30, 250, 50), statsText);
    }

    float FpsCounter()
    {
        float total = 0f;
        foreach(float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length/total;
    }
}