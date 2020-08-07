using Entity.Dtos;

namespace Business.Constants
{
    public class Messages
    {
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Şifre yanlış.";
        public static string LoginSuccessful = "Giriş işlemi başarılı.";
        public static string UserAlreadyExists = "Böyle bir kullanıcı mevcut.";
        public static string UserAvailable = "Kullanıcı uygun.";
        public static string RegisterSucessfull = "Kayıt işlemi başarıyla tamamlandı.";
        public static string RegisterFailed = "Kayıt işlemi başarısız.";
        public static string AccessTokenCreated = "Access Token oluşturuldu.";
        public static string AccessTokenFailed = "Acces Token oluşturulamadı.";
        public static string UserBanned = "Bu kullanıcı askıya alınmıştır.";
        public static string InvalidToken = "Geçersiz token.";
        public static string AuthorizationDenied = "Yetkisiz eylem.";
        public static string UserLogsNotFound = "Kullanıcı kayıtları bulunamadı.";
        public static string UserForgetPasswordSuccesfull = "Kullanıcı şifre güncelleme işlemi başarıyla tamamlandı.";
        public static string UserForgetPasswordFailed = "Kullanıcı şifre güncelleme işlemi başarısız oldu.";
        public static string MissingConfigurations = "Geçersiz konfigurasyon bilgileri.";
        public static string InvalidEndDate = "Lütfen bir bitiş tarihi giriniz.";
    }
}
