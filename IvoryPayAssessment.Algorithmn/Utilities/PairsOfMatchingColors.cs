using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Algorithmn.Utilities
{
    public class PairsOfMatchingColors
    {
      public  static int sockMerchant(int[] socks)
        {
            Dictionary<int, int> sockCounts = new Dictionary<int, int>();

            // Count occurrences of each sock color
            foreach (int sock in socks)
            {
                if (sockCounts.ContainsKey(sock))
                {
                    sockCounts[sock]++;
                }
                else
                {
                    sockCounts[sock] = 1;
                }
            }

            // Count pairs for each color
            int totalPairs = 0;
            foreach (int count in sockCounts.Values)
            {
                totalPairs += count / 2;
            }

            return totalPairs;
        }

     
    }
}
