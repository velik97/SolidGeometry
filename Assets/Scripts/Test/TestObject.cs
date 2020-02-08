using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Test
{
    public class TestObject : MonoBehaviour
    {
        private void Awake()
        {
            RootClass rootClass = new RootClass();
            rootClass.Values = new List<A>();
            rootClass.Values.Add(new B {Value = 2});
            rootClass.Values.Add(new C {Value = 1});

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            string serializedObject = JsonConvert.SerializeObject(rootClass, Formatting.Indented, settings);
            Debug.Log(serializedObject);
            RootClass newRootClass = JsonConvert.DeserializeObject<RootClass>(serializedObject, settings);
            Debug.Log(newRootClass.Values[0].GetType());
            Debug.Log(newRootClass.Values[1].GetType());
        }
    }
    
    public class RootClass
    {
        public List<A> Values;
    }

    public abstract class A
    { }

    public class B : A
    {
        public int Value;
    }
    
    public class C : A
    {
        public int Value;
    }
}