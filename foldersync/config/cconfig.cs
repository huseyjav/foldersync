using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.config
{

    public abstract class CConfig
    {
        protected string m_sourcePath;
        protected string m_replicaPath;
        protected int m_syncInterval;
        protected string m_logFile;

        public string sourcePath
        {
            get { return m_sourcePath; }
        }
        public string replicaPath
        {
            get { return m_replicaPath; }
        }

        public int syncInterval
        {
            get { return m_syncInterval; }
        }

        public string logFile
        {
            get { return m_logFile; }
        }
    }
}
