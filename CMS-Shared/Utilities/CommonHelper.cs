using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Web;
namespace CMS_Shared.Utilities
{
    public class CommonHelper
    {
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob00900X";
        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] {  "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                            "đ",
                                            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                            "í","ì","ỉ","ĩ","ị",
                                            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] {  "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                            "d",
                                            "e","e","e","e","e","e","e","e","e","e","e",
                                            "i","i","i","i","i",
                                            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                            "u","u","u","u","u","u","u","u","u","u","u",
                                            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static string GetDurationFromNow_Full(DateTime? dateUpdate)
        {
            var ret = "";
            try
            {
                var span = (TimeSpan)(DateTime.Now - dateUpdate);
                /* get total day */
                int totalDay = span.Days;

                /* get total year */
                int years = totalDay / 365;
                totalDay -= (years * 365);

                /* get total months */
                int months = totalDay / 30;
                totalDay -= (months * 30);

                /* string format */
                string formatted = string.Format("{0}{1}{2}{3}{4}",
                                    years > 0 ? string.Format("{0:0} year{1} ", years, years == 1 ? String.Empty : "s") : string.Empty,
                                    months > 0 ? string.Format("{0:0} month{1} ", months, months == 1 ? String.Empty : "s") : string.Empty,
                                    totalDay > 0 ? string.Format("{0:0} day{1} ", totalDay, totalDay == 1 ? String.Empty : "s") : string.Empty,
                                    years > 0 ? "" : span.Hours > 0 ? string.Format("{0:0} hour{1} ", span.Hours, span.Hours == 1 ? String.Empty : "s") : string.Empty,
                                    months > 0 ? "" : span.Minutes > 0 ? string.Format("{0:0} minute{1} ", span.Minutes, span.Minutes == 1 ? String.Empty : "s") : string.Empty
                                    );

                ret += formatted;
                //ret += " ago.";
            }
            catch (Exception ex) { };
            return ret;
        }

        public static string GetDurationFromNow(DateTime? dateUpdate)
        {
            var ret = "";
            try
            {
                var span = (TimeSpan)(DateTime.Now - dateUpdate);
                /* get total day */
                int totalDay = span.Days;

                /* get total year */
                int years = totalDay / 365;
                totalDay -= (years * 365);

                /* get total months */
                int months = totalDay / 30;
                totalDay -= (months * 30);

                /* string format */
                string formatted = string.Format("{0}{1}{2}{3}{4}",
                                    years > 0 ? string.Format("{0:0}y ", years) : string.Empty,
                                    months > 0 ? string.Format("{0:0}m ", months) : string.Empty,
                                    years > 0 ? "" : totalDay > 0 ? string.Format("{0:0}d ", totalDay) : string.Empty,
                                    months > 0 ? "" : span.Hours > 0 ? string.Format("{0:0}h ", span.Hours) : string.Empty,
                                    totalDay > 0 ? "" : span.Minutes > 0 ? string.Format("{0:0}min ", span.Minutes) : string.Empty
                                    );

                ret += formatted;
                //ret += " ago.";
            }
            catch (Exception ex) { };
            return ret;
        }

        public static List<string> ParseStringToList(string input = "", char[] separator = null)
        {
            var lstString = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(input) && separator != null)
                {
                    var data = input.Split(separator);
                    foreach (var item in data)
                    {
                        lstString.Add(item);
                    }
                }
            }
            catch (Exception) { }
            return lstString;
        }

        public static void WriteLog(string _input)
        {
            try
            {
                string Path = AppDomain.CurrentDomain.BaseDirectory + "\\fileLog.txt";
                var str = new
                {
                    DateTime = DateTime.Now,
                    Msg = _input,
                };
                File.AppendAllText(Path, str.ToString() + Environment.NewLine);
            }
            catch (Exception) { }
        }

        public static void WriteLogs(string _input)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/logs/fileLog.txt");
                StreamWriter SW = new StreamWriter(path);
                SW.Flush();
                SW.WriteLine(_input);
                SW.Close();
            }
            catch(Exception ex) { }
        }
    }
}
