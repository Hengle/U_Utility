﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AsssetHelper
{
    [System.Serializable]
    public class AHProjectFolderInfo
    {
        [UnityEngine.SerializeField]
        private List<AssetObjectInfo> m_assetList = new List<AssetObjectInfo>();

        [UnityEngine.SerializeField]
        private List<int> m_childFolderIndexers = new List<int>();

        [UnityEngine.SerializeField]
        string m_directoryName;

        [UnityEngine.SerializeField]
        private int m_ParentListIndex;

        [UnityEngine.SerializeField]
        int m_assetsInChildren = 0;

        [UnityEngine.SerializeField]
        long m_fileSize;

        [UnityEngine.SerializeField]
        long m_fileSizeAccumulated;

        [UnityEngine.SerializeField]
        string m_fileSizeString;

        [UnityEngine.SerializeField]
        string m_fileSizeAccumulatedString;

        public bool FoldOut = false;

        internal void AddAsset(AssetObjectInfo assetInfo)
        {
            if (AssetList == null)
                AssetList = new List<AssetObjectInfo>();

            assetInfo.SetParent(this);
            AssetList.Add(assetInfo);
        }

        public List<AssetObjectInfo> AssetList
        {
            get { return m_assetList; }
            set { m_assetList = value; }
        }
        public List<int> ChildFolderIndexers
        {
            get { return m_childFolderIndexers; }
            set { m_childFolderIndexers = value; }
        }
        public string DirectoryName
        {
            get { return m_directoryName; }
            set { m_directoryName = value; }
        }
        public int ParentIndex
        {
            get { return m_ParentListIndex; }
            set { m_ParentListIndex = value; }
        }
        public string FileSizeString
        {
            get { return m_fileSizeString; }
            set { m_fileSizeString = value; }
        }
        public string FileSizeAccumulatedString
        {
            get { return m_fileSizeAccumulatedString; }
            set { m_fileSizeAccumulatedString = value; }
        }

        internal bool ShouldBeListed(SortedDictionary<AHSerializableSystemType, bool> validTypeList)
        {
            return m_assetsInChildren >= 1 && hasValidAssetInChildren(this, validTypeList);
        }

        public void RecalcChildAssets(SortedDictionary<AHSerializableSystemType, bool> validTypeList)
        {
            CountChildren(validTypeList);

            if (m_ParentListIndex != -1)
                AHMainWindow.Instance.ReCalcUnusedAssetsFromIndex(m_ParentListIndex);
        }

        private bool hasValidAssetInChildren(AHProjectFolderInfo assetFolderInfo, SortedDictionary<AHSerializableSystemType, bool> validTypeList)
        {
            foreach (AssetObjectInfo aInfo in assetFolderInfo.AssetList)
            {
                if ((validTypeList.ContainsKey(aInfo.m_Type) && validTypeList[aInfo.m_Type] == true))
                    return true;
            }

            bool foundValidAsset = false;
            foreach (int indexer in assetFolderInfo.m_childFolderIndexers)
            {
                foundValidAsset = hasValidAssetInChildren(AHMainWindow.Instance.GetFolderList()[indexer], validTypeList);

                if (foundValidAsset)
                    return true;
            }

            return false;
        }

        private int calcAssetsInChildren(AHProjectFolderInfo assetFolderInfo, SortedDictionary<AHSerializableSystemType, bool> validTypeList, out long folderFileSize)
        {
            assetFolderInfo.m_fileSize = assetFolderInfo.GetUnusedAssetSize();

            long childrenSizeAccumulated = 0;

            int value = 0;
            foreach (int indexer in assetFolderInfo.m_childFolderIndexers)
            {
                long childSize = 0;
                value += AHMainWindow.Instance.GetFolderList()[indexer].m_assetsInChildren = calcAssetsInChildren(AHMainWindow.Instance.GetFolderList()[indexer], validTypeList, out childSize);

                childrenSizeAccumulated += childSize;
            }

            List<AssetObjectInfo> assetInfoList = (assetFolderInfo.AssetList.Where(val => (validTypeList.ContainsKey(val.m_Type) && validTypeList[val.m_Type]) == true)).ToList<AssetObjectInfo>();

            assetFolderInfo.m_fileSizeString = AHHelper.BytesToString(assetFolderInfo.m_fileSize);
            assetFolderInfo.m_fileSizeAccumulated = assetFolderInfo.m_fileSize + childrenSizeAccumulated;

            assetFolderInfo.m_fileSizeAccumulatedString = AHHelper.BytesToString(assetFolderInfo.m_fileSizeAccumulated);

            folderFileSize = assetFolderInfo.m_fileSizeAccumulated;

            return (value + assetInfoList.Count());
        }

        private long GetUnusedAssetSize()
        {
            long size = 0;
            foreach (AssetObjectInfo assetInfo in m_assetList)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(assetInfo.m_Path);
                size += fileInfo.Length;
            }
            return size;
        }

        internal void AddChildFolder(AHProjectFolderInfo afInfo)
        {
            if (m_childFolderIndexers == null)
                m_childFolderIndexers = new List<int>();

            AHMainWindow.Instance.AddProjectFolderInfo(afInfo);

            m_childFolderIndexers.Add(AHMainWindow.Instance.GetFolderList().IndexOf(afInfo));
        }

        internal void CountChildren(SortedDictionary<AHSerializableSystemType, bool> validTypeList)
        {
            long accumulatedSize = 0;
            m_assetsInChildren = calcAssetsInChildren(this, validTypeList, out accumulatedSize);

            //m_fileSize = accumulatedSize;
            //m_fileSizeString = AHHelper.BytesToString(m_fileSize);
        }

        internal int GetAssetCountInChildren()
        {
            return m_assetsInChildren;
        }

        internal void RemoveAsset(AssetObjectInfo assetObjectInfo)
        {
            m_assetList.Remove(assetObjectInfo);
        }

        internal string GetTopFolderName()
        {
            return System.IO.Path.GetFileName(DirectoryName.TrimEnd(System.IO.Path.DirectorySeparatorChar));
        }
    }

    [System.Serializable]
    public class AssetObjectInfo
    {
        public string m_Path;

        [UnityEngine.SerializeField]
        public AHSerializableSystemType m_Type;

        public string m_Name;
        public string m_ParentPath;

        public long m_FileSize;
        public string m_FileSizeString;

        public AssetObjectInfo(string path, AHSerializableSystemType type)
        {
            this.m_Path = path;
            string[] parts = path.Split('/');
            this.m_Name = parts[parts.Length - 1];
            this.m_Type = type;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
            this.m_FileSize = fileInfo.Length;
            this.m_FileSizeString = AHHelper.BytesToString(m_FileSize);
        }

        internal void Delete(SortedDictionary<AHSerializableSystemType, bool> validTypeList)
        {
            AHProjectFolderInfo parentFolder = AHMainWindow.Instance.GetFolderInfo(m_ParentPath);
            parentFolder.RemoveAsset(this);
            UnityEditor.AssetDatabase.DeleteAsset(m_Path);
            //UnityEditor.AssetDatabase.MoveAssetToTrash(m_Path);
        }

        internal void SetParent(AHProjectFolderInfo projectFolderInfo)
        {
            m_ParentPath = projectFolderInfo.DirectoryName;
        }
    }
}
