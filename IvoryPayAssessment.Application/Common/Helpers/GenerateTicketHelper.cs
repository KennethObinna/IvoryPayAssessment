namespace IvoryPayAssessment.Application.Common.Helpers
{
    public static class GenerateTicketHelper
    {
        public static IEnumerable<string> NextStrings(this Random rnd)
        {
            string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
            ISet<string> usedRandomStrings = new HashSet<string>();
            (int min, int max) length=(15,64);
            char[] chars = new char[length.max];
            int setLength = allowedChars.Length;
            int count = 25;
            while (count-- > 0)
            {
                int stringLength = rnd.Next(length.min, length.max + 1);

                for (int i = 0; i < stringLength; ++i)
                {
                    chars[i] = allowedChars[rnd.Next(setLength)];
                }

                string randomString = new string(chars, 0, stringLength);

                if (usedRandomStrings.Add(randomString))
                {
                    yield return randomString;
                }
                else
                {
                    count++;
                }
            }



        }

        public static  string Ticket()
        {
            Random rndn = new Random(0987654321);
         var rd=   rndn.NextDouble();
            return rd.ToString();
        }
    }
}
