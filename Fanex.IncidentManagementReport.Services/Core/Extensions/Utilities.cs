namespace Fanex.IncidentManagementReport.Services.Core.Extensions;

using FluentDateTime;
using System;
using System.Security.Cryptography;
using System.Text;

public static class Utilities
{
    private const string PassPhrase = "Fanex.IncidentMangementReport.Service";

    public static string Encrypt(string toEncrypt)
    {
        var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

        var hashmd5 = MD5.Create();

        var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(PassPhrase));
        hashmd5.Clear();

        var tdes = TripleDES.Create();

        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        var transform = tdes.CreateEncryptor();

        var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        tdes.Clear();

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string cipherString)
    {
        var toEncryptArray = Convert.FromBase64String(cipherString);
        var hashmd5 = MD5.Create();

        var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(PassPhrase));
        hashmd5.Clear();

        var tdes = TripleDES.Create();

        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        var transform = tdes.CreateDecryptor();
        var resultArray = transform.TransformFinalBlock(
            toEncryptArray, 0, toEncryptArray.Length);

        tdes.Clear();

        return Encoding.UTF8.GetString(resultArray);
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

    public static string GetDisplayShortDate(DateTime date)
        => date.ToString("dd MMM yyyy");

    public static string GetDisplayLongDate(DateTime date)
        => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, "Singapore Standard Time").ToString("dd MMM yyyy hh:mm:ss tt");
}