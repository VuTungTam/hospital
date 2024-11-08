using Hospital.SharedKernel.Libraries.ExtensionMethods;
using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace Hospital.SharedKernel.Libraries.Helpers
{
    public static class StringHelper
    {
        public static string RemoveExtraWhitespace(string input)
        {
            if (input == null)
            {
                return "";
            }

            input = input.Trim();

            var currentLength = input.Length;
            while (true)
            {
                input = input.Replace("  ", " ");
                if (currentLength == input.Length)
                {
                    return input;
                }
                currentLength = input.Length;
            }
        }

        public static string RemoveSpecialCharacters(string input)
        {
            // This code will remove all of the special characters
            // but if you doesn't want to remove some of the special character for e.g. comma "," and colon ":"
            // then make changes like this: Regex.Replace(Your String, @"[^0-9a-zA-Z:,]+", "")  
            return Regex.Replace(input, @"[^0-9a-zA-Z]+", "");
        }

        public static (string Toc, string Html) GenerateTOC(string html, string slug)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var headingNames = new List<string> { "h1", "h2", "h3", "h4", "h5", "h6" };
            var headings = doc.DocumentNode.Descendants().Where(n => headingNames.Contains(n.Name)).ToList();

            var tocBuilder = new StringBuilder();
            tocBuilder.AppendLine("<ul class='post-toc'>");

            var headingCounters = new int[6];
            foreach (var heading in headings)
            {
                var headingText = heading.InnerText.Trim();
                var tagName = heading.Name;
                var level = int.Parse(tagName.Substring(1)) - 1;

                headingCounters[level]++;
                for (int i = level + 1; i < headingCounters.Length; i++)
                {
                    headingCounters[i] = 0;
                }

                var tocNumber = string.Join(".", headingCounters.Take(level + 1).Where(c => c > 0));
                var tocNumberEle = $"<span class='toc-num {(!tocNumber.Contains(".") ? "toc-num-head" : "")}'>{tocNumber}</span>";

                var anchorId = headingText.ViToEn(true).Replace(" ", "-").ToLower();

                if (heading.Attributes["id"] == null)
                {
                    heading.Attributes.Add("id", anchorId);
                    heading.Attributes.Add("class", "heading " + anchorId);
                }

                tocBuilder.AppendLine($"<li class=\"item-level-{level + 1}\"><a href=\"tin-tuc/{slug}#{anchorId}\">{tocNumberEle}. {headingText}</a></li>");
            }

            html = doc.DocumentNode.OuterHtml;

            return (tocBuilder.AppendLine("</ul>").ToString(), html);
        }

    }
}
