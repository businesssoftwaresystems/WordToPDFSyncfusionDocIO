﻿using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using Syncfusion.Pdf;
using Syncfusion.DocIORenderer;
using Syncfusion.DocIO.DLS;
using Syncfusion.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
namespace WordToPDFSyncfusionDocIO.Controllers
{
    public class DocIOController : Controller
    {
        public IActionResult DocIOFeatures(string button, string renderingMode1, string renderingMode2, string renderingMode3)
        {
            if (button == null)
                return View();
            if (Request.Form.Files != null)
            {
                // Gets the extension from file.
                string extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
                // Compares extension with supported extensions.
                if (extension == ".doc" || extension == ".docx" || extension == ".dot" || extension == ".dotx" || extension == ".dotm" || extension == ".docm"
                   || extension == ".xml" || extension == ".rtf")
                {
                    MemoryStream stream = new MemoryStream();
                    Request.Form.Files[0].CopyTo(stream);
                    try
                    {
                        // Loads document from stream.
                        WordDocument document = new WordDocument(stream, FormatType.Automatic);
                        stream.Dispose();
                        stream = null;
                        // Creates a new instance of DocIORenderer class.
                        DocIORenderer render = new DocIORenderer();
                        if (renderingMode1 == "PreserveStructureTags")
                            render.Settings.AutoTag = true;
                        if (renderingMode2 == "PreserveFormFields")
                            render.Settings.PreserveFormFields = true;
                        render.Settings.ExportBookmarks = renderingMode3 == "PreserveWordHeadingsToPDFBookmarks"
                                                               ? Syncfusion.DocIO.ExportBookmarkType.Headings
                                                             : Syncfusion.DocIO.ExportBookmarkType.Bookmarks;
                        // Converts Word document into PDF document.
                        PdfDocument pdf = render.ConvertToPDF(document);
                        MemoryStream memoryStream = new MemoryStream();
                        // Save the PDF document.
                        pdf.Save(memoryStream);
                        render.Dispose();
                        pdf.Close();
                        document.Close();
                        memoryStream.Position = 0;
                        return File(memoryStream, "application/pdf", "WordToPDF.pdf");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = string.Format("The input document could not be processed completely, Could you please email the document to support@syncfusion.com for troubleshooting.");
                    }                    
                }
                else
                {
                    ViewBag.Message = string.Format("Please choose Word format document to convert to PDF");
                }
            }
            else
            {
                ViewBag.Message = string.Format("Browse a Word document and then click the button to convert as a PDF document");
            }
            return View();
        }
    }
}
