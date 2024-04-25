using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace IvoryPayAssessment.Application.Common.Helpers
{
    public static class SanitizeInputHelper
    {

        public static bool SanitizeInput3(string input)
        {
            // Regular expression pattern to check for only letters, spaces, and letters
            string pattern = @"^[A-Za-z ]+$";

            // Check if the input matches the pattern
            var match = Regex.IsMatch(input, pattern);
            return match;
        }
        public static string SanitizeInput2(string input)
        {
            string sanitizedInput = HttpUtility.HtmlEncode(input);
            return sanitizedInput;
        }
        public static bool SanitizeUploadContent(IFormFile file)
        {
            string[] expectedSignatures = { "FFD8", "89504E470D0A1A0A", "89504E" };

            // Read the first few bytes of the file
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Convert the file bytes to hexadecimal string
            string fileSignature = BitConverter.ToString(fileBytes.Take(4).ToArray()).Replace("-", "");

            // Check if the file signature matches any of the expected signatures
            foreach (string expectedSignature in expectedSignatures)
            {
                if (fileSignature.StartsWith(expectedSignature, StringComparison.OrdinalIgnoreCase))
                {
                    return true; // File content matches the expected format
                }
            }

            return false; // File content does not resemble the expected formats

        }
        public static bool SanitizeUpload(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension == ".pdf")
            {
                return false;
            }
            var allowed = SanitizeUploadContent(file);
            if (allowed)
            {
                // Check if the file extension is .jpeg, .jpg, or .png
                return IsFileValid(file);
            }
            else
            {
                return allowed;
            }

        }
        private static bool IsFileValid(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            return (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".pdf");
        }

        public static bool SanitizeUpload(List<IFormFile> files)
        {
            foreach (var file in files)
            {

                if (!IsFileValid(file))
                {
                    return false;
                }

            }
            return true;
        }
    }
}
