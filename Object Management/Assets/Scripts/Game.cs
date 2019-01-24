using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Game : PersistableObject {
    #region Data

    [SerializeField]
    ShapeFactory shapeFactory;
    [SerializeField]
    PersistentStorage storage;
    [SerializeField]
    int levelCount;
    [SerializeField]
    bool reseedOnLoad;
    [SerializeField]
    Slider creationSpeedSlider;
    [SerializeField]
    Slider destructSpeedSlider;

    public float CreationSpeed { get; set; }
    public float DestructSpeed { get; set; }

    float creationProgress, destructProgress; //when 1; create shape

    const int saveVersion = 4;


    public KeyCode createKey = KeyCode.C;
    public KeyCode destroyKey = KeyCode.X;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;


    Random.State mainRandomState;
    int loadedLevelBuildIndex;
    List<Shape> shapes;

    #endregion


    void Start() {
        mainRandomState = Random.state;
        shapes = new List<Shape>();
        if(Application.isEditor) {
            for(int i = 0; i < SceneManager.sceneCount; i++) {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if(loadedScene.name.Contains("Level ")) {
                    SceneManager.SetActiveScene(loadedScene);
                    loadedLevelBuildIndex = loadedScene.buildIndex;
                    return;
                }
            }
        }
        BeginNewGame();
        StartCoroutine(LoadLevel(1));
    }

    private void Update() {
        if(Input.GetKeyDown(createKey)) {
            CreateShape();
        }
        else if(Input.GetKeyDown(newGameKey)) {
            BeginNewGame();
            StartCoroutine(LoadLevel(loadedLevelBuildIndex));
        }
        else if(Input.GetKeyDown(saveKey)) {
            storage.Save(this, saveVersion);
        }
        else if(Input.GetKeyDown(loadKey)) {
            BeginNewGame();
            storage.Load(this);
        }
        else if(Input.GetKeyDown(destroyKey)) {
            DestroyShape();
        }
        else {
            for(int i = 1; i <= levelCount; i++) {
                if(Input.GetKeyDown(KeyCode.Alpha0 + i)) {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }
    }

    private void FixedUpdate() { //creation & destroy are in fixed update so it's always same, regardless of framerate
        for (int i = 0; i < shapes.Count; i++) {
            shapes[i].GameUpdate(); 
        }
        creationProgress += Time.deltaTime * CreationSpeed;
        while(creationProgress >= 1f) {
            creationProgress -= 1f;
            CreateShape();
        }

        destructProgress += Time.deltaTime * DestructSpeed;
        while(destructProgress >= 1f) {
            destructProgress -= 1f;
            DestroyShape();
        }
    }

    void BeginNewGame() {
        Random.state = mainRandomState;
        int seed = Random.Range(0, int.MaxValue) ^ (int)Time.unscaledTime;
        mainRandomState = Random.state;
        Random.InitState(seed);

        creationSpeedSlider.value = CreationSpeed = 0;
        destructSpeedSlider.value =DestructSpeed = 0;

        for(int i = 0; i < shapes.Count; i++) {
            shapeFactory.Reclaim(shapes[i]);
        }
        shapes.Clear(); //gets rid of references to destroyed objects
    }

    void CreateShape() {
        Shape instance = shapeFactory.GetRandom();
        GameLevel.Current.ConfigureSpawn(instance);
        shapes.Add(instance);
    }

    void DestroyShape() {
        if(shapes.Count > 0) {
            int index = Random.Range(0, shapes.Count);
            shapeFactory.Reclaim(shapes[index]);
            int lastIn = shapes.Count - 1;
            shapes[index] = shapes[lastIn];
            shapes.RemoveAt(lastIn);
        }

    }

    public override void Save(GameDataWriter writer) {

        writer.Write(shapes.Count);
        writer.Write(Random.state);
        writer.Write(CreationSpeed);
        writer.Write(creationProgress);
        writer.Write(DestructSpeed);
        writer.Write(destructProgress);
        writer.Write(loadedLevelBuildIndex);
        GameLevel.Current.Save(writer);
        for(int i = 0; i < shapes.Count; i++) {
            writer.Write(shapes[i].ShapeId); //writing shapeId to file before shape saves itself
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader) {
        int vers = reader.Version;
        if(vers > saveVersion) {
            Debug.LogError("Unsupported future save version " + vers);
            return;
        }
        StartCoroutine(LoadGame(reader));
    }

    IEnumerator LoadGame(GameDataReader reader) {
        int vers = reader.Version;

        int count = vers <= 0 ? -vers : reader.ReadInt(); //checks if old file or new file

        if(vers >= 3) {
            Random.State state = reader.ReadRandomState();
            if(!reseedOnLoad) {
                Random.state = state;
            }
            CreationSpeed = reader.ReadFloat();
            creationProgress = reader.ReadFloat();
            destructProgress = reader.ReadFloat();
            destructProgress = reader.ReadFloat();
        }

        yield return LoadLevel(vers < 2 ? 1 : reader.ReadInt());
        if(vers >= 3) {
            GameLevel.Current.Load(reader);
        }
        for(int i = 0; i < count; i++) {
            int shapeId = vers > 0 ? reader.ReadInt() : 0; //if old file just read 0/get cubes
            int materialId = vers > 0 ? reader.ReadInt() : 0;
            Shape instance = shapeFactory.Get(shapeId, materialId);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }

    IEnumerator LoadLevel(int lvlBuildIndex) {
        enabled = false; //prevents player from doing stuff before lvl loads

        if(loadedLevelBuildIndex > 0) {
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
        }

        yield return SceneManager.LoadSceneAsync(lvlBuildIndex, LoadSceneMode.Additive); //using async loading that yields a coroutine
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(lvlBuildIndex));
        loadedLevelBuildIndex = lvlBuildIndex;
        enabled = true;
    }
}
/*void Save() //old save; replaced by use of persistable obj's and storage
{
    //using (//etc){} -> ensures proper disposal of w/e writer uses; is shorthand for a common try/catch block
    using (
    var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)) //creates binary stream to write data into
    ) {
        writer.Write(objects.Count);

        //store transformation data of each cube
        for (int i = 0; i < objects.Count; i++)
        {
            Transform t = objects[i];
            writer.Write(t.localPosition.x);
            writer.Write(t.localPosition.y);
            writer.Write(t.localPosition.z);
        }
    }
    Debug.Log("Saving data to... " + savePath);

}

void Load()
{
    BeginNewGame();
    using (
        var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
        )
    {
        int count = reader.ReadInt32(); //being super explicit ab what we are reading in
        //readSingle -> floats
        for (int i = 0; i < count; i++)
        {
            Vector3 p;
            p.x = reader.ReadSingle();
            p.y = reader.ReadSingle();
            p.z = reader.ReadSingle();

            //instantiating the cubes that just read data for
            Transform t = Instantiate(prefab);
            t.localPosition = p;
            objects.Add(t);
        }
    }
}
*/


