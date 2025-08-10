using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.config
{
    public class CConsoleConfig : CConfig
    {
        public CConsoleConfig()
        {
            Console.WriteLine("Source path:");
            m_sourcePath = Console.ReadLine();
            Console.WriteLine("Replica path:");
            m_replicaPath = Console.ReadLine();
            Console.WriteLine("Sync Interval (in ms):");
            m_syncInterval = int.Parse(Console.ReadLine());
        }
    }
}
