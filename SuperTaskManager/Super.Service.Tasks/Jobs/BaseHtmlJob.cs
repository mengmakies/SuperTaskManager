using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Logging;
using HtmlAgilityPack;
using Quartz;

namespace Super.Service.Tasks.Jobs
{
    public abstract class BaseHtmlJob : BaseJob
    {
        protected HtmlDocument GetHtmlDoc(string url, Encoding encode)
        {
            string html = GetHtmlByUrl(url, encode);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }

        /// <summary>
        /// 传入URL返回网页的html代码
        /// </summary>
        /// <param name="url">网址 如http://www.taobao.com</param>
        /// <returns>返回页面的源代码</returns>
        protected string GetHtmlByUrl(string url, Encoding encode)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                //伪造浏览器数据，避免被防采集程序过滤
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215; C3DN.net;dongxi.douban.com)";

                //注意，为了更全面，可以加上如下一行，避开ASP常用的POST检查
                request.Referer = "http://dongxi.douban.com/";//您可以将这里替换成您要采集页面的主页

                var response = request.GetResponse() as HttpWebResponse;

                // 获取输入流
                var respStream = response.GetResponseStream();

                var reader = new StreamReader(respStream, encode);
                string content = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                return content;
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected int GetNumberInt(string str)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^/d./d]", "");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?/d*[.]?/d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }
    }
}