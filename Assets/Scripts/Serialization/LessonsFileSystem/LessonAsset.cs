using System;
using Lesson;
using Newtonsoft.Json;
using UnityEngine;

namespace Serialization.LessonsFileSystem
{
    [CreateAssetMenu]
    public class LessonAsset : FileSystemAsset
    {
        [SerializeField] private TextAsset m_LessonFile;
        
        private static JsonSerializerSettings s_SerializerSettings
            = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};

        private LessonData m_CashedLessonData;

        public override void ValidateNullReferences(ref bool valid)
        {
            if (m_LessonFile == null)
            {
                valid = false;
                Debug.LogError($"Have null at lesson \"{name}\"");
            }
        }

        public LessonData GetLessonDataCashed()
        {
            if (Application.isEditor)
            {
                return GetLessonData();
            }
            return m_CashedLessonData ?? (m_CashedLessonData = GetLessonData());
        }
        
        private LessonData GetLessonData()
        {
            try
            {
                string json = m_LessonFile.text;
                return JsonConvert.DeserializeObject<LessonData>(json, s_SerializerSettings);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while deserializing: " + e);
                return null;
            }
        }
    }
}