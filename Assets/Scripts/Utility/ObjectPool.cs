using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Assets.Scripts.Utility
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        private class Pool
        {
            [SerializeField]
            public GameObject Prefab;
            [SerializeField]
            public int Max;
            [SerializeField]
            public bool CreateIfExhausted = true;
            [SerializeField]
            public bool PreWarmOnStart = true;

            private bool preWarming = false;

            private Transform parent;
            private List<GameObject> objects = new List<GameObject>();

            public void SetParent(Transform parent)
            {
                this.parent = parent;
            }

            public System.Collections.IEnumerator PreWarm()
            {
                preWarming = true;
                if (objects.Count < Max)
                {
                    int toCreate = Max - objects.Count;
                    for (int i = 0; i < toCreate; i++)
                    {
                        InstantiateObject();
                        yield return null;
                    }
                }
                preWarming = false;
            }

            private GameObject InstantiateObject(bool pool = true)
            {
                GameObject newObj = Instantiate(Prefab, parent);
                newObj.transform.SetParent(parent, false);
                newObj.transform.localPosition = Vector3.zero;
                newObj.SetActive(false);
                Poolable poolable = newObj.GetComponent<Poolable>();
                if (poolable == null)
                {
                    poolable = newObj.AddComponent<Poolable>();
                }
                poolable.PoolName = Prefab.name;

                if (pool)
                {
                    objects.Add(newObj);
                }

                return newObj;
            }

            public void PoolObject(GameObject toPool)
            {
                objects.Add(toPool);
                toPool.SetActive(false);
                toPool.transform.parent = parent;
                toPool.transform.localPosition = Vector3.zero;
            }

            public GameObject GetObject(Transform parent = null)
            {
                GameObject obj = null;
                if (objects.Count > 0)
                {
                    obj = objects[0];
                    objects.RemoveAt(0);
                }
                else if (CreateIfExhausted || preWarming)
                {
                    obj = InstantiateObject(false);
                }
                return obj;
            }
        }

        private static ObjectPool instance;

        private static GameObject parentObject;

        [SerializeField]
        private List<Pool> pools = new List<Pool>();

        private static Dictionary<string, Pool> poolMap = new Dictionary<string, Pool>();

        public void Awake()
        {
            if (instance != null)
            {
                Debug.LogError($"Object pool has been added to more than one parent object, in this case '${gameObject.name}' This is not supported. Please remove the instance from this object.");
                return;
            }
            
            parentObject = gameObject;
            instance = this;

            BuildPoolMap();
            PreWarmOnStart();
        }

        public void AddNewPool(GameObject prefab, int max, bool createIfExhausted)
        {
            if(poolMap.ContainsKey(prefab.name))
            {
                Debug.LogWarning($"Pool already contains an entry for '{prefab.name}'. Ignoring.");
            }
            else
            {
                Pool pool = new Pool();
                pool.Prefab = prefab;
                pool.Max = max;
                pool.CreateIfExhausted = createIfExhausted;
                pool.SetParent(transform);
                poolMap.Add(pool.Prefab.name, pool);
                pools.Add(pool);
            }
        }

        private void BuildPoolMap()
        {
            foreach (Pool pool in pools)
            {
                pool.SetParent(parentObject.transform);
                poolMap.Add(pool.Prefab.name, pool);
            }
        }

        private void PreWarmOnStart()
        {
            foreach (Pool pool in pools)
            {
                if (pool.PreWarmOnStart)
                {
                    StartCoroutine(pool.PreWarm());
                }
            }
        }

        public static GameObject GetObjectForType(string objectType, Transform parent = null, Vector3? position = null, Quaternion? rotation = null)
        {
            GameObject obj = null;
            if (!poolMap.ContainsKey(objectType))
            {
                Debug.LogError($"No pool by name '{objectType}' defined.");
            }
            else
            {
                Pool pool = poolMap[objectType];
                obj = pool.GetObject();
                obj.transform.SetParent(parent, false);

                if (position != null && position.HasValue)
                {
                    obj.transform.position = position.Value;
                }

                if(rotation!=null && rotation.HasValue)
                {
                    obj.transform.rotation = rotation.Value;
                }

                obj.SetActive(true);
            }

            return obj;
        }

        public static void PoolObject(GameObject toPool)
        {
            if (toPool == null) 
            {
                Debug.LogWarning("Attempted to pool an object that has already been destroyed!");
            }
            else
            {
                Poolable poolable = toPool.GetComponent<Poolable>();
                if (poolable == null)
                {
                    Debug.LogError($"Attempted to pool a non-poolable object. ({toPool.name})");
                } 
                else
                {
                    if (!poolMap.ContainsKey(poolable.PoolName))
                    {
                        Debug.LogError($"No pool named '{poolable.PoolName}' found. Cannot pool object.");
                    }
                    else
                    {
                        Pool pool = poolMap[poolable.PoolName];
                        pool.PoolObject(toPool);
                    }
                }
            }
        }

        public static void PoolObject(GameObject obj, float delay)
        {
            if (instance != null)
            {
                instance.StartCoroutine(PoolObjectDelayed(obj, delay));
            }
            else
            {
                // Probably unloading the scene.  Just toss the object.  
                Destroy(obj);
            }
        }

        private static System.Collections.IEnumerator PoolObjectDelayed(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            PoolObject(obj);
        }

    }

    public class Poolable : MonoBehaviour
    {
        public string PoolName;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : UnityEditor.Editor
    {
        // This will contain the <pools> array of the ObjectPool. 
        SerializedProperty pools;

        // The Reorderable List we will be working with 
        ReorderableList list;

        private void OnEnable()
        {
            pools = serializedObject.FindProperty("pools");
            list = new ReorderableList(serializedObject, pools, false, true, true, true);

            list.drawElementCallback = DrawListItems; // Delegate to draw the elements on the list
            list.drawHeaderCallback = DrawHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
        }

        //This is the function that makes the custom editor work
        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Update the array property's representation in the inspector

            list.DoLayoutList(); // Have the ReorderableList do its work

            // We need to call this so that changes on the Inspector are saved by Unity.
            serializedObject.ApplyModifiedProperties();
        }

        void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); // The element in the list

            //Create a property field and label field for each property. 
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), "Prefab");
            EditorGUI.PropertyField(
                new Rect(rect.x+50, rect.y, 100, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Prefab"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x+155, rect.y, 30, EditorGUIUtility.singleLineHeight), "Max");
            EditorGUI.PropertyField(
                new Rect(rect.x + 185, rect.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Max"),
                GUIContent.none
            );
            
            EditorGUI.LabelField(new Rect(rect.x+225, rect.y, 50, EditorGUIUtility.singleLineHeight), "PreWarmOnStart");
            EditorGUI.PropertyField(
                new Rect(rect.x+280, rect.y, 20, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("PreWarmOnStart"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x + 300, rect.y, 110, EditorGUIUtility.singleLineHeight), "CreateIfExhausted");
            EditorGUI.PropertyField(
                new Rect(rect.x + 415, rect.y, 23, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("CreateIfExhausted"),
                GUIContent.none
            );

        }

        void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Pools");
        }
    }
#endif
}

