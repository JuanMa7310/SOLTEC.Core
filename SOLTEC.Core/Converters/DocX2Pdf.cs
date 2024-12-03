using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using PugPdf.Core;
using System.Drawing.Imaging;
using System.Xml.Linq;
using static System.Drawing.Imaging.ImageCodecInfo;
using static System.Drawing.Imaging.ImageFormat;

namespace SOLTEC.Core.Converters;

public class DocX2Pdf 
{
    public virtual async Task Execute(string pathDocxFile, string pathPdfFile, PdfPrintOptions pdfPrintOptions)
    {
        var htmlText = GenerateHtmlText(pathDocxFile);
        await CreatePdf(pathPdfFile, pdfPrintOptions, htmlText);
    }

    private async Task CreatePdf(string pathPdfFile, PdfPrintOptions pdfPrintOptions, string htmlText) 
    {
        var renderer = new HtmlToPdf {  PrintOptions = pdfPrintOptions };
        var pdfDocument = await renderer.RenderHtmlAsPdfAsync(htmlText);
        pdfDocument.SaveAs(pathPdfFile);
    }

    private string GenerateHtmlText(string pathDocxFile) 
    {
        var fileInfo = new FileInfo(pathDocxFile);
        var fullFilePath = fileInfo.FullName;
        try {
            return ParseDOCX(fileInfo);
        }
        catch (OpenXmlPackageException e) {
            if (e.ToString().Contains("Invalid Hyperlink")) {
                using (FileStream fs = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                    UriFixer.FixInvalidUri(fs, brokenUri => FixUri(brokenUri));
                }
                return ParseDOCX(fileInfo);
            }
        }
        return string.Empty;
    }

    public static Uri FixUri(string brokenUri) 
    {
        string newURI;
        if (brokenUri.Contains("mailto:")) 
        {
            var mailToCount = "mailto:".Length;
            brokenUri = brokenUri.Remove(0, mailToCount);
            newURI = brokenUri;
        }
        else 
        {
            newURI = " ";
        }
        return new Uri(newURI);
    }

    public static string ParseDOCX(FileInfo fileInfo) 
    {
        try {
            var byteArray = File.ReadAllBytes(fileInfo.FullName);
            using (var memoryStream = new MemoryStream()) {
                memoryStream.Write(byteArray, 0, byteArray.Length);
                using (var wDoc = WordprocessingDocument.Open(memoryStream, true)) 
                {
                    var pageTitle = fileInfo.FullName;
                    var part = wDoc.CoreFilePropertiesPart;
                    if (part != null)
                        pageTitle = (string)part.GetXDocument()
                            .Descendants(DC.title)
                            .FirstOrDefault()! ?? fileInfo.FullName;

                    WmlToHtmlConverterSettings settings = new WmlToHtmlConverterSettings() 
                    {
                        AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,
                        ImageHandler = imageInfo => {
                            var extension = imageInfo.ContentType.Split('/')[1].ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png") 
                                imageFormat = Png;
                            else if (extension == "gif") 
                                imageFormat = Gif;
                            else if (extension == "bmp") 
                                imageFormat = Bmp;
                            else if (extension == "jpeg") 
                                imageFormat = Jpeg;
                            else if (extension == "tiff") 
                            {
                                extension = "gif";
                                imageFormat = Gif;
                            }
                            else if (extension == "x-wmf") 
                            {
                                extension = "wmf";
                                imageFormat = Wmf;
                            }

                            if (imageFormat == null) return null;

                            string? base64 = null;
                            try
                            {
                                using (var ms = new MemoryStream())
                                {
                                    imageInfo.Bitmap.Save(ms, imageFormat);
                                    var ba = ms.ToArray();
                                    base64 = System.Convert.ToBase64String(ba);
                                }
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }

                            var format = imageInfo.Bitmap.RawFormat;
                            var codec = Enumerable.First(GetImageDecoders(), 
                                c => c.FormatID == format.Guid);
                            var mimeType = codec.MimeType;
                            var imageSource = string.Format("data:{0};base64,{1}", mimeType, base64);
                            var img = new XElement(Xhtml.img,
                                    new XAttribute(NoNamespace.src, imageSource),
                                    imageInfo.ImgStyleAttribute,
                                    imageInfo.AltText != null ?
                                        new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    var htmlElement = WmlToHtmlConverter.ConvertToHtml(wDoc, settings);
                    var html = new XDocument(new XDocumentType("html", null, null, null), htmlElement);
                    var htmlString = html.ToString(SaveOptions.DisableFormatting);
                    return htmlString;
                }
            }
        }
        catch 
        {
            return "The file is either open, please close it or contains corrupt data";
        }
    }
}
