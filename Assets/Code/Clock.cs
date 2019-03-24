using UnityEngine;

public class Clock : MonoBehaviour {

    [HideInInspector]
    public double DeltaTime { get; private set; }

    [HideInInspector]
    public double Time { get; private set; }

    public static Clock Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogErrorFormat("Too many clocks! ({0})", gameObject.name);
        }
    }

    void Update() {
        DeltaTime = (double) UnityEngine.Time.deltaTime;
        Time += DeltaTime;
    }
}
