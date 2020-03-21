using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Serialization
{
    public class FolderJsonsListSerializer<T>
    {
        public event Action ListUpdated;
        
        private readonly string m_FolderPath;

        private readonly List<string> m_FileNames;
        
        private readonly JsonSerializerSettings m_SerializerSettings;

        private string NoSuchDirectoryMessage => $"No such directory : '{m_FolderPath}'";
        private string NoSuchFileMessage(string filePath) => $"No such file : '{filePath}'";

        public FolderJsonsListSerializer(string folderPath)
        {
            m_FolderPath = folderPath;
            m_FileNames = new List<string>();

            m_SerializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
        }

        public IReadOnlyList<string> Names()
        {
            return m_FileNames;
        }
        
        public T GetObject(string name)
        {
            if (!IsValidFolderPath())
            {
                Debug.LogError(NoSuchDirectoryMessage);
                return default;
            }

            string path = Path.Combine(m_FolderPath, name + ".json");
            if (!File.Exists(path))
            {
                Debug.LogError(NoSuchFileMessage(path));
                return default;
            }
            
            try
            {
                string json = File.ReadAllText(path);
                T deserializedObject = JsonConvert.DeserializeObject<T>(json, m_SerializerSettings);
                return deserializedObject;
            }
            catch (Exception e)
            {
                Debug.LogError("Error while deserializing: " + e);
                return default;
            }
        }

        public void SaveObject(T objectToSave, string name)
        {
            if (!IsValidFolderPath())
            {
                Debug.LogError(NoSuchDirectoryMessage);
                return;
            }

            try
            {
                string serializedObject =
                    JsonConvert.SerializeObject(objectToSave, Formatting.Indented, m_SerializerSettings);
            
                string path = Path.Combine(m_FolderPath, name + ".json");
                File.WriteAllText(path, serializedObject);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while serializing: " + e);
                return;
            }
            
            UpdateList();
        }

        public void DeleteObject(string name)
        {
            if (!IsValidFolderPath())
            {
                Debug.LogError(NoSuchDirectoryMessage);
                return;
            }
            
            string path = Path.Combine(m_FolderPath, name + ".json");

            if (!File.Exists(path))
            {
                Debug.LogError(NoSuchFileMessage(path));
                return;
            }
            
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while deleting: " + e);
                return;
            }
            
            UpdateList();
        }

        public bool IsValidFolderPath()
        {
            return Directory.Exists(m_FolderPath);
        }

        private bool CreateDirectory()
        {
            Directory.CreateDirectory(m_FolderPath);
            return IsValidFolderPath();
        }

        public void UpdateList()
        {
            m_FileNames.Clear();
            if (!CreateDirectory())
            {
                Debug.LogError(NoSuchDirectoryMessage);
                return;
            }

            m_FileNames.AddRange(Directory
                .EnumerateFiles(m_FolderPath, "*.json")
                .Select(Path.GetFileName)
                .Select(name =>  name.Split('.')[0]));
            
            ListUpdated?.Invoke();
        }
    }
}