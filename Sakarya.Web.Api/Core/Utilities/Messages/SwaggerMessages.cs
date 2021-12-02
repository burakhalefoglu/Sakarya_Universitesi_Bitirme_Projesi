using System;

namespace Core.Utilities.Messages
{
    public static class SwaggerMessages
    {
        public static string Version => "v1";
        public static string Title => "Sakarya Universitesi Bitirme Projesi";
        public static string ContactName => "Burak Halefoğlu";
        public static string LicenceName => "MIT Lisansı";
        public static string ContactEMail => "burakhalefoglu@gmail.com";
        public static Uri ContactUrl => new Uri("https://www.linkedin.com/in/burakhalefoglu/");

        public static string Description => "Bu proje Burak halefoğlu tarafından, Sakarya " +
                                            "Üniversitesi bitirme projesi olarak tasarlanmıştır." +
                                            " İçerideki örnek mimari geliştirilerek veri bilimi " +
                                            "projelerinde kullanılabilir.";
    }
}