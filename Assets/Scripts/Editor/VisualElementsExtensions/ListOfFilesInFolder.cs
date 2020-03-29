using System;
using Serialization;
using UnityEngine.UIElements;

namespace Editor.VisualElementsExtensions
{
    public class ListOfFilesInFolder<T> : VisualElement
    {
        private readonly FolderJsonsListSerializer<T> m_Serializer;
        
        private readonly Action<string, T> m_OnChosenAction;

        public ListOfFilesInFolder(FolderJsonsListSerializer<T> serializer, Action<string, T> onChosenAction)
        {
            m_Serializer = serializer;
            m_Serializer.ListUpdated += UpdateList;
            
            m_OnChosenAction = onChosenAction;
        }

        private VisualElement GetElementForFile(string fileName)
        {
            VisualElement visualElement = new VisualElement
                {style = {flexDirection = FlexDirection.Row}};
            visualElement.Add(new Label(fileName));
            if (m_OnChosenAction != null)
            {
                visualElement.Add(new Button(() => ChoseFile(fileName)) {text = "Choose"});
            }
            visualElement.Add(new Button(() => DeleteFile(fileName)) {text = "Delete"});

            return visualElement;
        }

        private void DeleteFile(string fileName)
        {
            m_Serializer.DeleteObject(fileName);
            m_Serializer.UpdateList();
        }

        private void ChoseFile(string fileName)
        {
            T obj = m_Serializer.GetObject(fileName);
            if (m_OnChosenAction != null && obj != null)
            {
                m_OnChosenAction(fileName, obj);
            }
        }

        private void UpdateList()
        {
            Clear();
            foreach (string fileName in m_Serializer.Names())
            {
                Add(GetElementForFile(fileName));
            }
        }
    }
}