using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            return string.IsNullOrEmpty(value.Trim());
        }

        public static string RemoveLastCharacter(this String instr)
        {
            return instr.Substring(0, instr.Length - 1);
        }

        public static string RemoveLast(this String instr, int number)
        {
            return instr.Substring(0, instr.Length - number);
        }

        public static string RemoveFirstCharacter(this String instr)
        {
            return instr.Substring(1);
        }

        public static string RemoveFirst(this String instr, int number)
        {
            return instr.Substring(number);
        }

        public static Stream ToStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        public static void CopyTo(this Stream fromStream, Stream toStream)
        {
            if (fromStream == null)
            {
                throw new ArgumentNullException("fromStream");
            }

            if (toStream == null)
            {
                throw new ArgumentNullException("toStream");
            }

            var bytes = new byte[8092];
            int dataRead;

            while ((dataRead = fromStream.Read(bytes, 0, bytes.Length)) > 0)
            {
                toStream.Write(bytes, 0, dataRead);
            }
        }
    }
}
