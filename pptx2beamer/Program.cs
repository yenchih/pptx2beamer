﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using System.Xml;

namespace pptx2beamer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<1 || !System.IO.File.Exists(args[0]) || !args[0].EndsWith(".pptx"))
            {
                Console.WriteLine("Usage: pptx2beamer [pptx_file]");
                return;
            }

            // Extract information from pptx file
            Console.Write("Opening pptx file...");
            PPTXFile pptx = new PPTXFile(args[0]);
            pptx.ExtractMedia(pptx.FileName.Replace(".pptx", "")+"\\img");
            Console.WriteLine("done.");

            // Generate LaTeX code via XSLT transformations
            Console.Write("Transforming OpenXML to LaTeX...");
            StringBuilder tex = new StringBuilder();
            foreach (PPTXSlide slide in pptx.Slides)
                tex.AppendLine(slide.LaTeX);
            tex.AppendLine("\\end{document}");
            Console.WriteLine("done.");

            // Write LaTeX to source directory
            Console.Write("Writing source file...");
            System.IO.Directory.CreateDirectory(pptx.FileName.Replace(".pptx", ""));
            System.IO.File.WriteAllText(pptx.FileName.Replace(".pptx", "") + "\\" + pptx.FileName.Replace(".pptx", ".tex"), tex.ToString(), UTF8Encoding.ASCII);
            Console.WriteLine("done.");
        }

    }
}
