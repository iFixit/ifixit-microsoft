
using iFixit.WP8.UI.Code;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

public static class RichTextBlockExtensions
{
    public static void SetLinkedText(this BetterRichTextbox richTextBox, string htmlFragment)
    {
        var regEx = new Regex(
            @"\<a\s(href\=""|[^\>]+?\shref\="")(?<link>[^""]+)"".*?\>(?<text>.*?)(\<\/a\>|$)",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        richTextBox.Blocks.Clear();

        int nextOffset = 0;

        foreach (Match match in regEx.Matches(htmlFragment))
        {
            if (match.Index > nextOffset)
            {
                string url = match.Groups["link"].Value.TrimStart('/');
                if (!url.Contains("http://"))
                {
                    url = "http://" + url;
                }
                richTextBox.AppendText(htmlFragment.Substring(nextOffset, match.Index - nextOffset));
                nextOffset = match.Index + match.Length;
                richTextBox.AppendLink(match.Groups["text"].Value, new Uri(url));
            }

            Debug.WriteLine(match.Groups["text"] + ":" + match.Groups["link"]);
        }

        if (nextOffset < htmlFragment.Length)
        {
            richTextBox.AppendText(htmlFragment.Substring(nextOffset));
        }
    }

    public static void AppendText(this BetterRichTextbox richTextBox, string text)
    {
        Paragraph paragraph;

        if (richTextBox.Blocks.Count == 0 ||
            (paragraph = richTextBox.Blocks[richTextBox.Blocks.Count - 1] as Paragraph) == null)
        {
            paragraph = new Paragraph();
            richTextBox.Blocks.Add(paragraph);
        }

        paragraph.Inlines.Add(new Run { Text = text });
    }

    public static void AppendLink(this BetterRichTextbox richTextBox, string text, Uri uri)
    {
        Paragraph paragraph;

        if (richTextBox.Blocks.Count == 0 ||
            (paragraph = richTextBox.Blocks[richTextBox.Blocks.Count - 1] as Paragraph) == null)
        {
            paragraph = new Paragraph();
            richTextBox.Blocks.Add(paragraph);
        }

        var run = new Run { Text = text };
        var link = new Hyperlink { NavigateUri = uri, TargetName="_blank", Foreground = Application.Current.Resources["OrangeBrush"] as Brush };

        link.Inlines.Add(run);
        paragraph.Inlines.Add(link);
    }
}