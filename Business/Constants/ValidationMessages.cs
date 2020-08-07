using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public class ValidationMessages
    {
        public static string NotNull = "{0} boş geçilemez.";
        public static string NotEmpty = "{0} boş geçilemez.";
        public static string LengthFull = "{0}, {1} ile {2} karakter uzunluğunda olmalıdır.Girilen karakter sayısı {3}.";
        public static string ExactLength = "{0}, {1} karakter uzunluğunda olmalıdır.Girilen karakter sayısı {2}.";
        public static string MinLength = "{0}, en az {1} karakter uzunluğunda olmalıdır.Girilen karakter sayısı {2}.";
        public static string MaxLength = "{0}, en fazla {1} karakter uzunluğunda olmalıdır.Girilen karakter sayısı {2}.";
        public static string InvalidMust = "{0} bilgisi geçersiz.";
        public static string InvalidEmail = "Email adresi geçersiz.";
        public static string MustBeGreaterThan = "Girilen {0}, {1}'den büyük olmalıdır.";
        public static string MustBeLessThan = "Girilen {0}, {1}'den küçük olmalıdır.";
        public static string NotDecimal = "Girilen {0} geçersizdir.";

    }
}
