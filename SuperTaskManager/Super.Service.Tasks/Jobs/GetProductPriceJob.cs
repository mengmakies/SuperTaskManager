using System.Drawing;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AVOSCloud;
using HtmlAgilityPack;

namespace Super.Service.Tasks.Jobs
{
    // xpath语法：http://www.w3school.com.cn/xpath/xpath_syntax.asp
    public class GetProductPriceJob : BaseHtmlJob
    {
        public override void ExcuteTask()
        {
            // 后台云初始化
            AVClient.Initialize("Sfwa6nyMhQ6TWK1vnIteSVnf-gzGzoHsz", "dIUA56iTwM3NgfbbiLKPQdBT");

            // 1.找出9大分类
            var cateUrls = GetCategoryUrls("http://www.huamao.com.au/index.php?dispatch=categories.all");

            // 2.获取该分类下的商品列表总页数,然后分页获取数据
            // http://www.huamao.com.au/index.php?dispatch=categories.view&category_id=548&page=2
            Parallel.ForEach(cateUrls, SaveProductsByCateUrl);
        }

        /// <summary>
        /// 获取商城的9大分类url
        /// </summary>
        /// <param name="homeCategroyUrl"></param>
        /// <returns></returns>
        private List<string> GetCategoryUrls(string homeCategroyUrl)
        {
            var list = new List<string>();

            var doc = GetHtmlDoc(homeCategroyUrl, Encoding.GetEncoding("UTF-8"));
            if (doc == null) return list;

            var cateNodes = doc.DocumentNode.SelectNodes("//div[@class='all-categories-box']//h2/a");
            if (cateNodes == null) return list;

            foreach (var moreValNode in cateNodes)
            {
                var url = moreValNode.GetAttributeValue("href", "");
                if (string.IsNullOrEmpty(url)) continue;

                list.Add($"http://www.huamao.com.au/{url}");
            }

            return list;
        }

        private void SaveProductsByCateUrl(string homeUrl)
        {
            var doc = GetHtmlDoc(homeUrl, Encoding.GetEncoding("UTF-8"));
            if (doc == null) return;

            // 获取总页数，【末页】的【rel】属性记录着总页数
            int pageCount = 1;
            var nodePageCount = doc.DocumentNode.SelectSingleNode("//div[@class='pagination-bottom']//a[@class='cm-history next '][last()]");
            if (nodePageCount != null)
            {
                pageCount = nodePageCount.GetAttributeValue("rel", 0);
            }

            Parallel.For(1, pageCount + 1, async i =>
            {
                var webUrl = $"{homeUrl}&page={i}";

                await SaveProducts(webUrl);
            });
        }

        private async Task SaveProducts(string productListUrl)
        {
            try
            {
                var doc = GetHtmlDoc(productListUrl, Encoding.GetEncoding("UTF-8"));
                if (doc == null) return;

                var proNodes = doc.DocumentNode.SelectNodes(".//div[@class='product-list-box']/div[@class='product-box']");
                Parallel.ForEach(proNodes, async proNode =>
                {
                    var titleNode = proNode.SelectSingleNode(".//a[@class='product-title']");
                    if (titleNode == null) return;

                    var title = titleNode.InnerText;
                    var relativeUrl = titleNode.GetAttributeValue("href", "");
                    var url = $"http://www.huamao.com.au/{relativeUrl}";
                    var proId = Convert.ToInt32(Regex.Match(relativeUrl, @"\d+").Value);


                    var priceNodes = proNode.SelectNodes(".//span[@class='price-num']");
                    if (priceNodes == null) return;

                    var aoPrice = Convert.ToDouble(priceNodes[1].InnerText.Replace(",", ""));
                    var rmbPrice = Convert.ToInt32(priceNodes[4].InnerText.Replace(",", ""));
                    var saveDate = DateTime.Now.ToString("yyyy/MM/dd");

                    // 上传到后端云
                    AVQuery<AVObject> query = new AVQuery<AVObject>("Product");
                    query = query.WhereEqualTo("proId", proId);
                    query = query.WhereEqualTo("saveDate", saveDate);

                    await query.CountAsync().ContinueWith(async t =>
                    {
                        if (t.Result > 0) return;

                        AVObject product = new AVObject("Product");
                        product["proId"] = proId;
                        product["title"] = title;
                        product["url"] = url;
                        product["auPrice"] = aoPrice;
                        product["rmbPrice"] = rmbPrice;
                        product["saveDate"] = saveDate;
                        await product.SaveAsync();
                    });
                });
            }
            catch (Exception ex)
            {
                var detail = ex.Message;
            }
        }
    }
}
