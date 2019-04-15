using UnityEngine;

public class Clock : MonoBehaviour {

    public double DeltaTime { get; private set; }

    public double Time { get; set; }

    public static Clock Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogErrorFormat("Too many clocks! ({0})", gameObject.name);
        }
    }

    void Update() {
        DeltaTime = UnityEngine.Time.deltaTime;
        Time += DeltaTime;
    }
}
