﻿using System.Collections.Generic;
using System.IO;
using ArtLibrary.Model;
using ArtLibrary.Model.Resolver;
using ArtLibrary.ModelInterface.Datasets;
using ArtLibrary.ModelInterface.Datasets.Types;
using ArtLibrary.ModelInterface.Results;

namespace ArtLibrary.Model.Datasets
{
    internal class SingleMouse : ModelObjectBase, ISingleMouse
    {
        private string m_Name;
        private string m_Id;
        private ITypeBase m_Type = ModelResolver.Resolve<IUndefined>();
        private List<string> m_Videos; 
        private string m_Class;
        private int m_Age;
        private List<ISingleFile> m_VideoFiles;
        private Dictionary<ISingleFile, IMouseDataResult> m_Results; 

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (Equals(m_Name, value))
                {
                    return;
                }

                m_Name = value;

                MarkAsDirty();
            }
        }

        public string Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                if (Equals(m_Id, value))
                {
                    return;
                }

                m_Id = value;

                MarkAsDirty();
            }
        }

        public ITypeBase Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                if (Equals(m_Type, value))
                {
                    return;
                }

                m_Type = value;

                MarkAsDirty();
            }
        }

        public List<string> Videos
        {
            get
            {
                return m_Videos;
            }
            set
            {
                if (Equals(m_Videos, value))
                {
                    return;
                }

                m_Videos = value;

                MarkAsDirty();
            }
        }

        public string Class
        {
            get
            {
                return m_Class;
            }
            set
            {
                if (Equals(m_Class, value))
                {
                    return;
                }

                m_Class = value;

                MarkAsDirty();
            }
        }

        public int Age
        {
            get
            {
                return m_Age;
            }
            set
            {
                if (Equals(m_Age, value))
                {
                    return;
                }

                m_Age = value;

                MarkAsDirty();
            }
        }

        public List<ISingleFile> VideoFiles
        {
            get
            {
                return m_VideoFiles;
            }
            set
            {
                if (ReferenceEquals(m_VideoFiles, value))
                {
                    return;
                }

                m_VideoFiles = value;

                MarkAsDirty();
            }
        }

        public Dictionary<ISingleFile, IMouseDataResult> Results
        {
            get
            {
                return m_Results;
            }
            set
            {
                if (ReferenceEquals(m_Results, value))
                {
                    return;
                }

                m_Results = value;

                MarkAsDirty();
            }
        }
        
        public void GenerateFiles(string fileLocation)
        {
            List<ISingleFile> files = new List<ISingleFile>();
            string directoryName = Path.GetDirectoryName(fileLocation);
            string[] filesInDirectory = Directory.GetFiles(directoryName);

            foreach (string video in Videos)
            {
                string fileName = Class + "-" + video;
                string actualFile = directoryName + @"\" + fileName;

                bool found = false;
                foreach (string file in filesInDirectory)
                {
                    string currentFileName = Path.GetFileName(file);
                    string extension = Path.GetExtension(currentFileName).ToLower();

                    if (!(extension.Contains("avi") || extension.Contains("mpg") || extension.Contains("mpeg") || extension.Contains("mp4") || extension.Contains("mov")))
                    {
                        continue;
                    }
                    
                    found = true;
                    actualFile += extension;
                    break;
                }

                if (!found)
                {
                    continue;
                }

                ISingleFile singleFile = ModelResolver.Resolve<ISingleFile>();
                singleFile.VideoFileName = actualFile;
                files.Add(singleFile);
            }

            VideoFiles = files;
        }
    }
}
