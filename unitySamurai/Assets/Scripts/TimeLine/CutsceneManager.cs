using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance {  get; private set; }

    public event EventHandler OnCutScenePlaying;
    public event EventHandler OnCutSceneStop;
    [SerializeField] private PlayableDirector[] cutscenes;
    [SerializeField] private Transform[] sceneObjects;
    private int currentCutsceneIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayCutscene()
    {
        PlayCutscene(currentCutsceneIndex);
    }

    private void PlayCutscene(int index)
    {
        if (index < 0 || index > cutscenes.Length) return;
        sceneObjects[index].gameObject.SetActive(true);
        var director = cutscenes[index];
        director.stopped += Director_stopped;
        
        OnCutScenePlaying?.Invoke(this, EventArgs.Empty);
        director.Play();

    }

    private void Director_stopped(PlayableDirector obj)
    {
        obj.stopped -= Director_stopped;
        sceneObjects[currentCutsceneIndex].gameObject.SetActive(false);
        currentCutsceneIndex++;
        OnCutSceneStop?.Invoke(this, EventArgs.Empty);

    }
}
