using System.Collections.Generic;
using UnityEngine;

namespace Shapes.Validators.Uniqueness
{
    public class UniquenessValidatorsSolver<TValidator> where TValidator : class, IUniquenessValidator<TValidator>
    {
        private readonly int m_Capacity;
        private readonly List<ValidatorNode>[] m_HashTable;
        
        public UniquenessValidatorsSolver(int capacity)
        {
            m_Capacity = capacity;
            m_HashTable = new List<ValidatorNode>[capacity];
        }
        
        public void AddValidatable(TValidator validator)
        {
            ValidatorNode node = new ValidatorNode(validator, this);
            InsertInTable(node);
            validator.UniqueDeterminingPropertyUpdated += node.UpdateNodeInTable;
        }

        public void RemoveValidatable(TValidator validator)
        {
            ValidatorNode node = FindNode(validator);
            node.UpdateHashCode();
            RemoveFromTable(node);
            validator.UniqueDeterminingPropertyUpdated -= node.UpdateNodeInTable;
        }

        private void InsertInTable(ValidatorNode validatorNode)
        {
            int hash = validatorNode.HashCode;

            if (m_HashTable[hash] == null)
            {
                m_HashTable[hash] = new List<ValidatorNode>();
            }
            
            m_HashTable[hash].Add(validatorNode);
            CheckListForUniqueness(m_HashTable[hash]);
        }

        private void RemoveFromTable(ValidatorNode validatorNode)
        {
            int hash = validatorNode.HashCode;

            if (m_HashTable[hash] == null || m_HashTable[hash].Count == 0)
            {
                return;
            }

            if (m_HashTable[hash].Remove(validatorNode))
            {
                CheckListForUniqueness(m_HashTable[hash]);
            }

            if (m_HashTable[hash].Count == 0)
            {
                m_HashTable[hash] = null;
            }
        }

        private void CheckListForUniqueness(List<ValidatorNode> list)
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
                    if (list[i].Validator.UniqueEquals(list[j].Validator))
                    {
                        unique = false;
                    }
                }
                list[i].Validator.SetIsUnique(unique);
            }
        }

        private ValidatorNode FindNode(TValidator validator)
        {
            int hash = (validator.GetUniqueHashCode() & 0xfffffff) % m_Capacity;
            if (m_HashTable[hash] == null)
            {
                Debug.LogError("Can't find node");
                return null;
            }
            
            foreach (ValidatorNode node in m_HashTable[hash])
            {
                if (node.Validator == validator)
                {
                    return node;
                }
            }
            
            Debug.LogError("Can't find node");
            return null;
        }
        
        private class ValidatorNode
        {
            public readonly TValidator Validator;
            private readonly UniquenessValidatorsSolver<TValidator> m_ValidatorsSolver;
            private int m_HashCode;

            public int HashCode => m_HashCode;
            
            public ValidatorNode(TValidator validator, UniquenessValidatorsSolver<TValidator> validatorsSolver)
            {
                Validator = validator;
                m_ValidatorsSolver = validatorsSolver;
                UpdateHashCode();
            }

            public void UpdateNodeInTable()
            {
                m_ValidatorsSolver.RemoveFromTable(this);

                UpdateHashCode();
                m_ValidatorsSolver.InsertInTable(this);
            }

            public void UpdateHashCode()
            {
                m_HashCode = (Validator.GetUniqueHashCode() & 0xfffffff) % m_ValidatorsSolver.m_Capacity;
            }
        }
    }
}