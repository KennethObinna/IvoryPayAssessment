using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Helpers
{
    public static class Utilities
    {
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        public static string GenerateLetter(int minLength,
                                    int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][]
        {
            PASSWORD_CHARS_UCASE.ToCharArray(),
            //  PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }
        public static string GenerateOTP()
        {
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");
            return r;
        }
        public static string ReadHtmlFile(string htmlFileNameWithPath)
        {
            string mail = string.Empty;
            using (StreamReader sr = new StreamReader(htmlFileNameWithPath))
            {
                mail = sr.ReadToEnd();
            }
            return mail;
        }
        public static StringBuilder ReadHtmlFileV2(string htmlFileNameWithPath)
        {
            StringBuilder htmlContent = new System.Text.StringBuilder();
            string line;

            using (System.IO.StreamReader htmlReader = new System.IO.StreamReader(htmlFileNameWithPath))
            {

                while ((line = htmlReader.ReadLine()) != null)
                {
                    htmlContent.Append(line);
                }
            }


            return htmlContent;
        }
        public static string GenerateActivationLink(string userId, IConfiguration config)
        {
            var baseUri = config.GetValue<string>("SystemSettings:ActivationLink");

            var activationLink = $"{baseUri}/{userId}";

            return activationLink;
        }

        public static bool ValidateBinaryFile(string filePathA, string filePathB)
        {

            using FileStream fileStreamA = new FileStream(filePathA, FileMode.Open, FileAccess.Read);
            using FileStream fileStreamB = new FileStream(filePathA, FileMode.Open, FileAccess.Read);

            // Define the expected magic number (a specific sequence of bytes)


            // Read the first few bytes from the file
            byte[] fileHeaderA = new byte[fileStreamA.Length];
            fileStreamA.Read(fileHeaderA, 0, Convert.ToInt32(fileStreamA.Length));

            byte[] fileHeaderB = new byte[fileStreamA.Length];
            fileStreamA.Read(fileHeaderB, 0, Convert.ToInt32(fileStreamA.Length));
            // Compare the read bytes with the expected magic number
            for (int i = 0; i < fileHeaderB.Length; i++)
            {
                if (fileHeaderA[i] != fileHeaderB[i])
                {
                    // The binary file is not valid
                    return false;
                }
            }

            // If all bytes match, the binary file is considered valid
            return true;


        }

        public static bool ValidateBinaryFile(byte[] fileA, byte[] fileB)
        {


            // Compare the read bytes with the expected magic number
            for (int i = 0; i < fileA.Length; i++)
            {
                if (fileB[i] != fileA[i])
                {
                    // The binary file is not valid
                    return false;
                }
            }

            // If all bytes match, the binary file is considered valid
            return true;



        }

        public static string ConvertEnumToWords(this Enum enumType, string delimiter = " ")
        {

            var words = enumType.ToString();
            var result = words.Any(char.IsUpper)
                ? string.Join(delimiter, Regex.Split(words, @"(?<!^)(?=[A-Z])"))
                : words;
            return result;


        }
        public static string ConvertEnumToWords(this string words, string delimiter = " ")
        {
 
            var result = words.Any(char.IsUpper)
                ? string.Join(delimiter, Regex.Split(words, @"(?<!^)(?=[A-Z])"))
                : words;
            return result;


        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // This is a simple distance formula, you might want to use a more accurate formula in production
            // For simplicity, let's assume the earth is a perfect sphere
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = 6371 * c; // Radius of the earth in km
            return distance;
        }
    }
}
