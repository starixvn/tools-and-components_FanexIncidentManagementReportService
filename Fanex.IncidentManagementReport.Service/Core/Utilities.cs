using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluentDateTime;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Fanex.IncidentManagementReport.Service.Core
{
    public static class Utilities
    {
        private const string PassPhrase = "Fanex.IncidentMangementReport.Service";

        public static void RazorEngineConfig()
        {
            var config = new TemplateServiceConfiguration();
            config.DisableTempFileLocking = true;
            config.CachingProvider = new DefaultCachingProvider(t => { });

            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
        }

        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(PassPhrase));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = tdes.CreateEncryptor();

            byte[] resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(PassPhrase));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = tdes.CreateDecryptor();
            byte[] resultArray = transform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static DateTime DateOfPreviousWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var dateTime = dt.WeekEarlier();
            var firstDayOfWeek = dateTime.FirstDayOfWeek().AddDays(1);
            var lastDayOfWeek = dateTime.LastDayOfWeek().AddDays(1);

            for (var i = firstDayOfWeek; i <= lastDayOfWeek; i = i.AddDays(1))
            {
                if (i.DayOfWeek == startOfWeek)
                {
                    return i;
                }
            }

            return dateTime;
        }
    }
}
