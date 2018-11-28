using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject {

    public PersistableObject prefab;
    public PersistentStorage storage;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    
    List<PersistableObject> objects;

    private void Awake()
    {
        objects = new List<PersistableObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            
            storage.Save(this);
            Debug.Log("saving");
        }
        else if (Input.GetKeyDown(loadKey))
        {
            
            BeginNewGame();
            storage.Load(this);
            Debug.Log("loading");
        }
    }

    void BeginNewGame()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }
        objects.Clear(); //gets rid of references to destroyed objects
    }

    void CreateObject()
    {
        PersistableObject o = Instantiate(prefab);
        Transform t = o.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale =  Vector3.one * Random.Range(0.5f, 1.5f);
        objects.Add(o);
    }

    public override void Save (GameDataWriter writer)
    {
        writer.Write(objects.Count);
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].Save(writer);
        }
    }

    public override void Load (GameDataReader reader)
    {
        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            PersistableObject o = Instantiate(prefab);
            o.Load(reader);
            objects.Add(o);
        }
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


