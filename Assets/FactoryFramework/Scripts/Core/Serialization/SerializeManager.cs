using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.IO;
using System.Text;

namespace FactoryFramework
{
    public class SerializeManager : MonoBehaviour
    {

        public FactorySaveData data;

        // Event is triggered when loading completes
        // Boolean value is returned depending on success
        [SerializeField]
        private UnityEvent<bool> _onLoadComplete;
        public UnityEvent<bool> OnLoadComplete
        {
            get { return _onLoadComplete; }
        }

        // Event is triggered when saving completes
        // Boolean value is returned depending on success
        [SerializeField]
        private UnityEvent<bool> _onSaveComplete;
        public UnityEvent<bool> OnSaveComplete
        {
            get { return _onSaveComplete; }
        }

        private string saveFilePath;
        private void Awake()
        {
            saveFilePath = Application.persistentDataPath + "/";
        }

        public void Load() => Load("save.json");
        public void Load(string path)
        {
            string filePath = Path.Combine(saveFilePath, path);

            if (!File.Exists(filePath))
            {
                OnLoadComplete.Invoke(false);
                return;
            }
            try
            {
                string saveString = File.ReadAllText(filePath);

                // Deserialize data into a FactorySaveData object
                data = JsonUtility.FromJson<FactorySaveData>(saveString);

                // remove all existing!
                foreach (Conveyor c in GameObject.FindObjectsOfType<Conveyor>()) Destroy(c.transform.root.gameObject);
                foreach (Building b in GameObject.FindObjectsOfType<Building>()) Destroy(b.transform.root.gameObject);


                // build a lookup of Guid -> LogisticComponent
                // this is used to re-link the conveyor belts to buildings
                Dictionary<Guid, LogisticComponent> lookup = new Dictionary<Guid, LogisticComponent>();

                foreach (ProducerSaveData prod in data.producers)
                {
                    GameObject instantiated = InstantiateBuildingData(prod);
                    Producer p = instantiated.GetComponent<Producer>();
                    p.resource = prod.resource;
                    p.GUID = new Guid(prod.guid);
                    lookup.Add(p.GUID, p);
                }
                foreach (ProcessorSaveData proc in data.processors)
                {
                    GameObject instantiated = InstantiateBuildingData(proc);
                    Processor p = instantiated.GetComponent<Processor>();
                    p.AssignRecipe(proc.recipe);
                    p.GUID = new Guid(proc.guid);
                    lookup.Add(p.GUID, p);
                }
                foreach (StorageSaveData stor in data.storage)
                {
                    GameObject instantiated = InstantiateBuildingData(stor);
                    Storage s = instantiated.GetComponent<Storage>();
                    s.data.storage = stor.storage;
                    s.GUID = new Guid(stor.guid);
                    lookup.Add(s.GUID, s);
                }
                Dictionary<ConveyorSaveData, Conveyor> covneyorMap = new Dictionary<ConveyorSaveData, Conveyor>();
                foreach (ConveyorSaveData convData in data.conveyors)
                {
                    GameObject prefab = Resources.Load<GameObject>(convData.assetPath);
                    GameObject instantiated = Instantiate(prefab);
                    Conveyor conv = instantiated.GetComponent<Conveyor>();
                    conv.data.start = convData.start;
                    conv.data.end = convData.end;
                    conv.data.startDir = convData.startDir;
                    conv.data.endDir = convData.endDir;
                    conv.GUID = new Guid(convData.guid);
                    conv.SetItemsOnBelt(convData.items, convData.capacity);

                    conv.UpdateMesh(true);
                    conv.AddCollider();

                    covneyorMap.Add(convData, conv);
                    lookup.Add(conv.GUID, conv);
                }

                // connect belts after everything is spawned
                // this is separate from belt spawning because some belts connect to other belts
                foreach (KeyValuePair<ConveyorSaveData, Conveyor> belt in covneyorMap)
                {

                    if (!belt.Key.inputSocketBuilding.Equals(""))
                    {
                        Guid input = new Guid(belt.Key.inputSocketBuilding);
                        var connection = lookup[input];
                        if (connection is Building)
                        {
                            Building building = connection as Building;
                            building.GetOutputSocketByIndex(belt.Key.inputSocketIndex).Connect(belt.Value);
                        }
                        else if (connection is Conveyor)
                        {
                            Conveyor conveyor = connection as Conveyor;

                        }
                    }

                    if (!belt.Key.outputSocketBuilding.Equals(""))
                    {
                        Guid output = new Guid(belt.Key.outputSocketBuilding);
                        var connection = lookup[output];
                        if (connection is Building)
                        {
                            Building building = connection as Building;
                            building.GetInputSocketByIndex(belt.Key.outputSocketIndex).Connect(belt.Value);
                        }
                        else if (connection is Conveyor)
                        {
                            Conveyor conveyor = connection as Conveyor;
                            belt.Value.GetBridge().Connect(conveyor);
                        }
                    }
                    else
                    {
                        // nothing connected to the end of the belt, enable the bridge
                        belt.Value.EnableBridge();
                    }
                }

                OnLoadComplete.Invoke(true);
            } 
            catch (Exception e)
            {
                Debug.LogError("FactoryFramework load failed! - " + e.ToString());
                OnLoadComplete.Invoke(false);
            }
        }
        public void Save() => Save("save.json");
        public void Save(string path)
        {
            string filePath = Path.Combine(saveFilePath, path);

            // collect and sort buildings
            var buildings = GameObject.FindObjectsOfType<Building>();
            List<ProducerSaveData> producersData = new List<ProducerSaveData>();
            List<ProcessorSaveData> processorsData = new List<ProcessorSaveData>();
            List<StorageSaveData> storageData = new List<StorageSaveData>();
            List<BuildingSaveData> unspecializedData = new List<BuildingSaveData>();
            foreach (var b in buildings)
            {
                if (b is Producer)
                {
                    producersData.Add(new ProducerSaveData() {
                        position = b.transform.position,
                        rotation = b.transform.rotation,
                        scale = b.transform.lossyScale,
                        guid = b.GUID.ToString(),
                        assetPath = b._prefabPath,
                        resource=(b as Producer).resource
                    });

                } else if (b is Processor)
                {
                    processorsData.Add(new ProcessorSaveData()
                    {
                        position = b.transform.position,
                        rotation = b.transform.rotation,
                        scale = b.transform.lossyScale,
                        guid = b.GUID.ToString(),
                        assetPath = b._prefabPath,
                        recipe = (b as Processor).data.recipe
                    });
                }
                else if (b is Storage)
                {
                    storageData.Add(new StorageSaveData()
                    {
                        position = b.transform.position,
                        rotation = b.transform.rotation,
                        scale = b.transform.lossyScale,
                        guid = b.GUID.ToString(),
                        assetPath = b._prefabPath,
                        storage = (b as Storage).data.storage
                    });
                } else
                {
                    unspecializedData.Add(new BuildingSaveData()
                    {
                        position = b.transform.position,
                        rotation = b.transform.rotation,
                        scale = b.transform.lossyScale,
                        guid = b.GUID.ToString(),
                        assetPath = b._prefabPath
                    });
                }
            }

            List<ConveyorSaveData> conveyorData = new List<ConveyorSaveData>();
            var conveyors = GameObject.FindObjectsOfType<Conveyor>();
            foreach(var c in conveyors)
            {
                conveyorData.Add(new ConveyorSaveData()
                {
                    start = c.data.start,
                    end = c.data.end,
                    startDir = c.data.startDir,
                    endDir = c.data.endDir,
                    capacity = c.Capacity + c.items.Count,
                    inputSocketBuilding= (c.data.inputSocket as LogisticComponent)?.GUID.ToString(),
                    inputSocketIndex = c.data.inputSocketIndex,
                    outputSocketBuilding = (c.data.outputSocket as LogisticComponent)?.GUID.ToString(),
                    outputSocketIndex= c.data.outputSocketIndex,
                    guid = c.GUID.ToString(),
                    assetPath = c._prefabPath,
                    items=c.items.ToArray()
                });
            }

            data = new FactorySaveData()
            {
                producers = producersData.ToArray(),
                processors = processorsData.ToArray(),
                storage = storageData.ToArray(),
                unspecialized = unspecializedData.ToArray(),
                conveyors = conveyorData.ToArray()
            };

            var jsonString = JsonUtility.ToJson(data,true);

            File.WriteAllText(filePath, jsonString);
            print($"saving data to {filePath}");

            OnSaveComplete.Invoke(true);
        }

        public GameObject InstantiateBuildingData(BuildingSaveData b)
        {
            GameObject prefab = Resources.Load<GameObject>(b.assetPath);
            GameObject instantiated = Instantiate(prefab, b.position, b.rotation);
            instantiated.transform.localScale = b.scale;
            instantiated.GetComponent<LogisticComponent>().GUID = new Guid(b.guid);

            return instantiated;
        }

        //public void Clear()
        //{
        //    var prefabs = FindObjectsOfType<SerializablePrefab>(true);
        //    foreach (SerializablePrefab p in prefabs)
        //    {
        //        Destroy(p.gameObject);
        //    }
        //}

        [Serializable]
        public class BuildingSaveData
        {
            // Transform data
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;

            // GUID assigned automatically by LogisticComponent or re-assigned on load
            public string guid;
            // Prefab path used to load the prefab from resources folder on load
            public string assetPath;
        }
        [Serializable]
        public class ProducerSaveData: BuildingSaveData
        {
            public LocalStorage resource;
        }
        [Serializable]
        public class ProcessorSaveData : BuildingSaveData
        {
            public Recipe recipe;
        }
        [Serializable]
        public class StorageSaveData : BuildingSaveData
        {
            public ItemStack[] storage;
        }
        [Serializable]
        public class ConveyorSaveData
        {
            public Vector3 start;
            public Vector3 end;
            public Vector3 startDir;
            public Vector3 endDir;

            public int capacity;

            // Building that inputs to the conveyor
            public string inputSocketBuilding;
            public int inputSocketIndex;

            // Building the conveyor outputs to
            public string outputSocketBuilding;
            public int outputSocketIndex;

            // GUID assigned automatically by LogisticComponent or re-assigned on load
            public string guid;
            // Prefab path used to load the prefab from resources folder on load
            public string assetPath;

            public ItemOnBelt[] items;
        }

        [Serializable]
        public class FactorySaveData
        {
            // global stuff
            public ProducerSaveData[] producers;
            public ProcessorSaveData[] processors;
            public StorageSaveData[] storage;
            public BuildingSaveData[] unspecialized;

            public ConveyorSaveData[] conveyors;
        }
    }
}
