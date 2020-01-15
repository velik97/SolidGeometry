using System.Collections.Generic;
using UnityEngine;

namespace Shapes.Data.Uniqueness
{
    public class UniquenessValidator<TValidatable> where TValidatable : class, IUniquenessValidatable<TValidatable>
    {
        private readonly int m_Capacity;
        private readonly List<ValidatableNode>[] m_HashTable;
        
        public UniquenessValidator(int capacity)
        {
            m_Capacity = capacity;
            m_HashTable = new List<ValidatableNode>[capacity];
        }
        
        public void AddValidatable(TValidatable validatable)
        {
            ValidatableNode node = new ValidatableNode(validatable, this);
            InsertInTable(node);
            validatable.UniqueDeterminingPropertyUpdated += node.UpdateNodeInTable;
        }

        public void RemoveValidatable(TValidatable validatable)
        {
            ValidatableNode node = FindNode(validatable);
            node.UpdateHashCode();
            RemoveFromTable(node);
            validatable.UniqueDeterminingPropertyUpdated -= node.UpdateNodeInTable;
        }

        private void InsertInTable(ValidatableNode validatableNode)
        {
            int hash = validatableNode.ActualHashCode;

            if (m_HashTable[hash] == null)
            {
                m_HashTable[hash] = new List<ValidatableNode>();
            }
            
            m_HashTable[hash].Add(validatableNode);
            CheckListForUniqueness(m_HashTable[hash]);
        }

        private void RemoveFromTable(ValidatableNode validatableNode)
        {
            int hash = validatableNode.OldHashCode;

            if (m_HashTable[hash] == null || m_HashTable[hash].Count == 0)
            {
                return;
            }

            if (m_HashTable[hash].Remove(validatableNode))
            {
                CheckListForUniqueness(m_HashTable[hash]);
            }

            if (m_HashTable[hash].Count == 0)
            {
                m_HashTable[hash] = null;
            }
        }

        private void CheckListForUniqueness(List<ValidatableNode> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                bool unique = true;
                for (int j = 0; j < list.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if (list[i].Validatable.UniqueEquals(list[j].Validatable))
                    {
                        unique = false;
                    }
                }
                list[i].Validatable.SetIsUnique(unique);
            }
        }

        private ValidatableNode FindNode(TValidatable validatable)
        {
            int hash = validatable.GetUniqueHashCode() % m_Capacity;
            if (m_HashTable[hash] == null)
            {
                Debug.LogError("Can't find node");
                return null;
            }
            
            foreach (ValidatableNode node in m_HashTable[hash])
            {
                if (node.Validatable == validatable)
                {
                    return node;
                }
            }
            
            Debug.LogError("Can't find node");
            return null;
        }
        
        private class ValidatableNode
        {
            public readonly TValidatable Validatable;
            private readonly UniquenessValidator<TValidatable> m_Validator;
            private int m_ActualHashCode;
            private int m_OldHashCode;

            public int OldHashCode => m_OldHashCode;
            public int ActualHashCode => m_ActualHashCode;

            public ValidatableNode(TValidatable validatable, UniquenessValidator<TValidatable> validator)
            {
                Validatable = validatable;
                m_Validator = validator;
                m_ActualHashCode = m_OldHashCode = validatable.GetUniqueHashCode() % m_Validator.m_Capacity;
            }

            public void UpdateNodeInTable()
            {
                UpdateHashCode();
                m_Validator.RemoveFromTable(this);
                m_Validator.InsertInTable(this);
            }

            public void UpdateHashCode()
            {
                int hash = Validatable.GetUniqueHashCode() % m_Validator.m_Capacity;
                m_OldHashCode = m_ActualHashCode;
                m_ActualHashCode = hash;
            }
        }
    }
}