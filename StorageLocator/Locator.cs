using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StorageLocator
{
    public static class Locator
    {
        private const int clusterSize = 4;
        public static List<int> LocateStorages(string fileId, SortedDictionary<int, int> storageHashes)
        {

            byte[] encoded = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(fileId));

            var hashedFile = BitConverter.ToInt32(encoded, 0);

            int index = 0;

            //Finding the first server that has the register
            foreach (var entry in storageHashes)
            {
                if (entry.Key > hashedFile)
                {
                    break;
                }
                index++;
            }

            //if there is no match, wrap around
            if (index == storageHashes.Count)
                index = 0;

            List<int> cluster = new List<int>();

            // Get the cluster with the records
            for (int i = index; i < index + clusterSize && i < index + storageHashes.Count; i++)
            {
                cluster.Add(storageHashes.Values.ToList()[i % storageHashes.Count]);
            }

            return cluster;
        }
    }

    
}
