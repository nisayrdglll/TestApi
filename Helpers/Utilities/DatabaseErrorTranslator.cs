using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public static class DatabaseErrorTranslator
    {
        public static string toTR(string error)
        {
            if (error.Contains("unique constraint"))
            {
                return "Bu kayıt zaten mevcut, benzersiz bir değer kullanın.";
            }
            else if (error.Contains("Cannot insert duplicate key row"))
            {
                return "Bu kayıt anahtarı zaten mevcut, farklı bir değer kullanın.";
            }
            else if (error.Contains("foreign key constraint"))
            {
                return "Geçersiz yabancı anahtar değeri. İlgili kayıt bulunamadı.";
            }
            else if (error.Contains("primary key constraint"))
            {
                return "Birincil anahtar ihlali. Bu anahtar zaten var.";
            }
            else if (error.Contains("check constraint"))
            {
                return "Bir doğrulama kuralı ihlal edildi. Girdi verilerini kontrol edin.";
            }
            else if (error.Contains("data type mismatch"))
            {
                return "Veri türü uyuşmazlığı. Girilen veri, beklenen türde değil.";
            }
            else if (error.Contains("length exceeded"))
            {
                return "Veri uzunluğu, izin verilen maksimum sınırı aştı.";
            }
            else if (error.Contains("deadlock"))
            {
                return "İşlem kilitlendi. Lütfen işlemi tekrar deneyin.";
            }
            else if (error.Contains("syntax error"))
            {
                return "Sorguda sözdizimi hatası var. Lütfen sorguyu kontrol edin.";
            }
            else if (error.Contains("null value"))
            {
                return "Bir zorunlu alan boş bırakıldı. Lütfen tüm zorunlu alanları doldurun.";
            }
            else if (error.Contains("timeout"))
            {
                return "Veritabanı zaman aşımına uğradı. Lütfen daha sonra tekrar deneyin.";
            }
            else if (error.Contains("connection"))
            {
                return "Veritabanı bağlantı hatası. Lütfen bağlantınızı kontrol edin.";
            }
            else if (error.Contains("data may have been modified or deleted"))
            {
                return "Veritabanı bağlantı hatası. Veri silinmiş veya değiştirilmiş olabilir.";
            }
            else
            {
                return "Bilinmeyen bir veritabanı hatası oluştu: " + error;
            }
        }

    }
}
