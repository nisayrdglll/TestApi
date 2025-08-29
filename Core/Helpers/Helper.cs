using DataAccess.Context;

namespace Core.Helpers
{
    public class Helper
    {
        private readonly DbContext _context;
        public Helper(DbContext context)
        {
            _context = context;
        }
        public static string DetermineFieldNameFromModel(string errorMessage, object model)
        {
            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (errorMessage.Contains(property.Name))
                {
                    return property.Name;
                }
            }
            return errorMessage;
        }
        public static string ProcessHref(string href)
        {
            if (string.IsNullOrWhiteSpace(href))
                return href;

            if (href.StartsWith("/"))
                href = href.Substring(1);

            href = href.Replace("/", "-");

            href = href.ToLower();

            return href;
        }

        public static string GetFileIconClass(string fileType)
        {
            if (string.IsNullOrEmpty(fileType))
                return "bi bi-file-earmark fs-2";

            string iconClass = "bi bi-file-earmark";

            if (fileType.StartsWith("image/"))
            {
                iconClass = "bi bi-file-earmark-image";
            }
            else if (fileType.StartsWith("video/"))
            {
                iconClass = "bi bi-file-earmark-play";
            }
            else if (fileType.Contains("pdf"))
            {
                iconClass = "bi bi-file-earmark-pdf";
            }
            else if (fileType.Contains("excel") || fileType.Contains("spreadsheet"))
            {
                iconClass = "bi bi-file-earmark-excel";
            }
            else if (fileType.Contains("word") || fileType.Contains("document"))
            {
                iconClass = "bi bi-file-earmark-word";
            }

            return iconClass;
        }


        public static bool IsTeknikOrganizasyonBirimi(int organizasyonBirimiId)
        {
            // Teknik organizasyon birimlerinin ID'leri
            var teknikBirimler = new HashSet<int>
            {
                1, // Sondaj Dairesi Başkanlığı
                2, // Maden Etüt ve Arama Dairesi Başkanlığı
                3, // Jeoloji Etütleri Dairesi Başkanlığı
                4, // Jeofizik Etütleri Dairesi Başkanlığı
                5, // Bilimsel Dokümantasyon ve Tanıtma Dairesi Başkanlığı
                8, //Maden Analizleri ve Teknolojileri Dairesi Başkanlığı
                11,  // Makine İkmal Dairesi Başkanlığı
                16, //idari ve Mali İşler Dairesi Başkanlığı
            };

            return teknikBirimler.Contains(organizasyonBirimiId);
        }

        public static class TextHelper
        {
            // Küçük harfle yazılacak bağdaşlar listesi
            public static readonly List<string> Bagdaslar = new List<string>
            {
                "ve", "veya", "ile", "de", "da", "ki", "ya", "ya da", "yahut", "fakat", "ama", "lakin",
                "ancak", "oysa", "oysaki", "yani", "üzere", "için", "dolayı", "ise", "ötürü", "gibi",
                "karşın", "rağmen", "değil", "göre", "kadar", "beraber", "birlikte"
            };

            // Kelimelerin baş harfini büyük harf yapıp, bağdaşları küçülten fonksiyon
            public static string IlkHarfleriBuyut(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                // Kelimelere ayır
                string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Her kelimeyi işle
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length == 0)
                        continue;

                    // Kelime bağdaş mı kontrol et
                    bool isBagdas = (i > 0) && Bagdaslar.Contains(words[i].ToLower());

                    if (isBagdas)
                    {
                        // Bağdaşı tamamen küçük harfe çevir
                        words[i] = words[i].ToLower();
                    }
                    else
                    {
                        // Bağdaş değilse, baş harfi büyük yap, diğerlerini küçük
                        if (words[i].Length == 1)
                        {
                            words[i] = words[i].ToUpper();
                        }
                        else
                        {
                            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                        }
                    }
                }

                // Kelimeleri tekrar birleştir
                return string.Join(" ", words);
            }
        }

    }
}