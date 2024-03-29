﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AsssetHelper
{
    [System.Serializable]
    public class AHSerializableSystemType : IComparer<AHSerializableSystemType>
    {
        [SerializeField]
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
        }

        [SerializeField]
        private string m_AssemblyQualifiedName;

        public string AssemblyQualifiedName
        {
            get { return m_AssemblyQualifiedName; }
        }

        [SerializeField]
        private string m_AssemblyName;

        public string AssemblyName
        {
            get { return m_AssemblyName; }
        }

        private System.Type m_SystemType;
        public System.Type SystemType
        {
            get
            {
                if (m_SystemType == null)
                {
                    GetSystemType();
                }
                return m_SystemType;
            }
        }

        private void GetSystemType()
        {
            m_SystemType = System.Type.GetType(m_AssemblyQualifiedName);
        }

        public AHSerializableSystemType(System.Type _SystemType)
        {
            m_SystemType = _SystemType;
            m_Name = _SystemType.Name;
            m_AssemblyQualifiedName = _SystemType.AssemblyQualifiedName;
            m_AssemblyName = _SystemType.Assembly.FullName;
        }

        public override bool Equals(System.Object obj)
        {
            AHSerializableSystemType temp = obj as AHSerializableSystemType;
            if ((object)temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }

        public override int GetHashCode()
        {
            return SystemType.GetHashCode();
        }

        public bool Equals(AHSerializableSystemType _Object)
        {
            return _Object.SystemType.Equals(SystemType);
        }

        public static bool operator ==(AHSerializableSystemType a, AHSerializableSystemType b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(AHSerializableSystemType a, AHSerializableSystemType b)
        {
            return !(a == b);
        }

        public int Compare(AHSerializableSystemType a, AHSerializableSystemType b)
        {
            if (a.SystemType.Name.CompareTo(b.SystemType.Name) != 0)
            {
                return a.SystemType.Name.CompareTo(b.SystemType.Name);
            }
            else
            {
                return 0;
            }
        }
    }

    public class SerializableSystemTypeComparer : IComparer<AHSerializableSystemType>
    {
        public int Compare(AHSerializableSystemType a, AHSerializableSystemType b)
        {
            if (a.SystemType.Name.CompareTo(b.SystemType.Name) != 0)
            {
                return a.SystemType.Name.CompareTo(b.SystemType.Name);
            }
            else
            {
                return 0;
            }
        }
    }
}