using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainController : MonoBehaviour {

	private static MainController instance;

	private string currentSceneName;
	private string nextSceneName;
	private AsyncOperation resourceUnloadTask;
	private AsyncOperation sceneLoadTask;
	private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };
	private SceneState sceneState;
	private delegate void UpdateDelegate();
	private UpdateDelegate[] updateDelegates;

	public static void SwitchScene (string nextSceneName) {
		if (instance != null) {
			if (instance.currentSceneName != nextSceneName) {
				instance.nextSceneName = nextSceneName;
			}
		}
	}

	private void Awake () {
		Object.DontDestroyOnLoad(gameObject);

		if (instance == null) {
			instance = this;
		} else {
			Debug.LogError("There is already a MainController in the scene, there can only be one", instance);
			Destroy(this);
		}

		updateDelegates = new UpdateDelegate[(int)SceneState.Count];

		updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
		updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
		updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
		updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
		updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
		updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
		updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

		nextSceneName = UtilityManager.SceneNames.menuScene;
		sceneState = SceneState.Reset;
		
	}

	private void OnDestroy () {
		if (updateDelegates != null) {
			for (int i = 0; i < (int)SceneState.Count; i++) {
				updateDelegates[i] = null;
			}
			updateDelegates = null;
		}
		if (instance != null) {
			instance = null;
		}
	}

	private void Update () {
		if (updateDelegates[(int)sceneState] != null) {
			updateDelegates[(int)sceneState]();
		}
	}

	private void UpdateSceneReset () {
		System.GC.Collect();
		sceneState = SceneState.Preload;
	}

	// Handle anything that needs to happen before loading
	private void UpdateScenePreload () {
		sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);
		sceneState = SceneState.Load;
	}

	// Show the loading screen until it is loaded
	private void UpdateSceneLoad () {
		if (sceneLoadTask.isDone == true) {
			sceneState = SceneState.Unload;
		} else {
			// update scene loading progress
		}
	}


	private void UpdateSceneUnload () {
		if (resourceUnloadTask == null) {
			resourceUnloadTask = Resources.UnloadUnusedAssets();
		} else {
			if (resourceUnloadTask.isDone == true) {
				resourceUnloadTask = null;
				sceneState = SceneState.Postload;
			}
		}
	}

	// Handle anything that needs to happen immediately after loading
	private void UpdateScenePostload () {
		currentSceneName = nextSceneName;
		sceneState = SceneState.Ready;
	}

	// Handle anything that needs to happen immediately before running
	private void UpdateSceneReady () {
		//System.GC.Collect();
		sceneState = SceneState.Run;
	}

	// Wait for scene change
	private void UpdateSceneRun () {
		if (currentSceneName != nextSceneName) {
			sceneState = SceneState.Reset;
		}
	}
}
