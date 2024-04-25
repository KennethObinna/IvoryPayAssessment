using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Algorithmn.Utilities
{
    public class WithSearching
    {
      public  static int CountSockPairs(int[] socks)
        {
            HashSet<int> seenColors = new HashSet<int>();
            int pairs = 0;

            foreach (int sock in socks)
            {
                // If the color is already in the set, it means we found a pair
                if (seenColors.Contains(sock))
                {
                    pairs++;
                    seenColors.Remove(sock); // Remove the color from the set since it's paired now
                }
                else
                {
                    seenColors.Add(sock); // Otherwise, add it to the set
                }
            }

            return pairs;
        }
    }
}
