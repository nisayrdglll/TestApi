using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace Service.Belgenet
{
    //public class BelgenetService
    //{
    //    private string _endpointUrl = "http://10.0.200.220:8080/edys-webservices/services/evrakServis";

    //    public string TaslakEvrakOlustur(
    //        string username,
    //        string password,
    //        string konuKodu,
    //        string ustYaziHtml,
    //        byte[] ustYaziPdf,
    //        string klasorKodu,
    //        string evrakTuru = "RESMIYAZI",
    //        string gizlilik = "TASNIF_DISI",
    //        string ivedilik = "NORMAL",
    //        string evrakTipi = "GIDEN_EVRAK")
    //    {
    //        try
    //        {
    //            var binding = new BasicHttpBinding();
    //            binding.MaxReceivedMessageSize = int.MaxValue;
    //            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
    //            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

    //            var endpoint = new EndpointAddress(_endpointUrl);
    //            var client = new EvrakServisPortTypeClient(binding, endpoint);

    //            client.ClientCredentials.UserName.UserName = username;
    //            client.ClientCredentials.UserName.Password = password;

    //            var request = new taslakEvrakOlustur
    //            {
    //                evrakTuru = evrakTuru,
    //                gizlilikDerecesi = gizlilik,
    //                ivedilik = ivedilik,
    //                evrakTipi = evrakTipi,
    //                konuKodu = Convert.ToBase64String(Encoding.UTF8.GetBytes(konuKodu)),
    //                ustYaziHtml = Encoding.UTF8.GetBytes(ustYaziHtml),
    //                ustYaziPdf = ustYaziPdf,
    //                kaldirilacakKlasorler = new[] {
    //                    new klasorKodu { klasorKodu = Convert.ToBase64String(Encoding.UTF8.GetBytes(klasorKodu)) }
    //                },
    //                dogrulamaKoduBasılsın = true
    //            };

    //            var result = client.taslakEvrakOlustur(request);
    //            return result?.evrakId ?? "Hata: EvrakId alınamadı";
    //        }
    //        catch (Exception ex)
    //        {
    //            return $"Hata oluştu: {ex.Message}";
    //        }
    //    }

    //    public string EvrakOku(string evrakId, string username, string password)
    //    {
    //        try
    //        {
    //            var requestXml = $@"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:web=\"http://webservices.edys.turksat.com.tr/\">
    //               < soapenv:Header />
    //               < soapenv:Body >
    //                  < web:evrakOku >
    //                     < evrakOkuParametre >
    //                        < evrakId >{ evrakId}</ evrakId >
    //                        < sorgulayanKullaniciBirimId > 9253 </ sorgulayanKullaniciBirimId >
    //                     </ evrakOkuParametre >
    //                  </ web:evrakOku >
    //               </ soapenv:Body >
    //            </ soapenv:Envelope > ";

    //            using (var client = new System.Net.Http.HttpClient())
    //            {
    //                client.DefaultRequestHeaders.Clear();
    //                client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));
    //                var content = new System.Net.Http.StringContent(requestXml, Encoding.UTF8, "text/xml");
    //                var response = client.PostAsync(_endpointUrl, content).Result;

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    return response.Content.ReadAsStringAsync().Result;
    //                }
    //                else
    //                {
    //                    return "Hata: " + response.StatusCode;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return "Hata oluştu: " + ex.Message;
    //        }
    //    }

    //    public string TeslimAlHavaleEt(string username, string password, string evrakId, string teslimAlacakKullaniciBirimId)
    //    {
    //        try
    //        {
    //            var envelope = $@"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:web=\"http://webservices.edys.turksat.com.tr/\">
    //               < soapenv:Header />
    //               < soapenv:Body >
    //                  < web:teslimAlHavaleEt >
    //                     < teslimAlHavaleEt >
    //                        < teslimAlinacakEvrakId >{ evrakId}</ teslimAlinacakEvrakId >
    //                        < teslimAlacakKullaniciBirimId >{ teslimAlacakKullaniciBirimId}</ teslimAlacakKullaniciBirimId >
    //                     </ teslimAlHavaleEt >
    //                  </ web:teslimAlHavaleEt >
    //               </ soapenv:Body >
    //            </ soapenv:Envelope > ";

    //            using (var client = new System.Net.Http.HttpClient())
    //            {
    //                client.DefaultRequestHeaders.Clear();
    //                client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));
    //                var content = new System.Net.Http.StringContent(envelope, Encoding.UTF8, "text/xml");
    //                var response = client.PostAsync(_endpointUrl, content).Result;

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    var resultXml = response.Content.ReadAsStringAsync().Result;
    //                    var xmlDoc = new XmlDocument();
    //                    xmlDoc.LoadXml(resultXml);
    //                    var success = xmlDoc.GetElementsByTagName("basarili")[0]?.InnerText;
    //                    if (success == "true")
    //                        return "Havale işlemi başarılı";
    //                    else
    //                        return xmlDoc.GetElementsByTagName("aciklama")[0]?.InnerText ?? "Havale işlemi başarısız";
    //                }
    //                else
    //                {
    //                    return "HTTP Hatası: " + response.StatusCode;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return "Hata oluştu: " + ex.Message;
    //        }
    //    }
    //}
}
