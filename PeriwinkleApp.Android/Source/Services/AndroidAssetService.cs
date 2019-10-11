using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Android.Content;
using Android.Content.Res;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Android.Source.Services
{
    public abstract class AndroidAssetService
    {
        private readonly Context context;

        protected AndroidAssetService (Context context)
        {
            this.context = context;
        }

        protected XmlDocument AssetToXmlDocument (string filename)
        {
            // create and xml document from the "Assets" folder. 
            // Make sure File's Build Action is set to "AndroidAsset "
            AssetManager assets = context.Assets;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load (assets.Open (filename));
            return xmlDoc;
        }

        protected IList <string> XmlNodeListToList (XmlNodeList nodeList)
        {
            // converts xml node list to list of strings
            return nodeList.Cast <XmlNode> ()
                           .Select (node => node.InnerText)
                           .ToList ();
        }

//
//        public void Blabla (string filename)
//        {
//            filename = "Mbes.xml";
//            string content;
//            AssetManager assets = context.Assets;
////            using (StreamReader sr = new StreamReader(assets.Open(filename)))
////            {
////                content = sr.ReadToEnd();
////            }
//
//            XmlDocument xmlDoc = new XmlDocument();
//
//            xmlDoc.Load (assets.Open (filename));
//
//            var mbesform = xmlDoc.GetElementsByTagName ("MbesForm");
//            var instructions = xmlDoc.GetElementsByTagName ("Instructions");
//            var instruction = xmlDoc.GetElementsByTagName("Instruction");
//            var questions = xmlDoc.GetElementsByTagName("Questions");
//            var question = xmlDoc.GetElementsByTagName("Question");
//
//
//            var questionList = question.Cast <XmlNode> ()
//                                       .Select (node => node.InnerText)
//                                       .ToList ();
//
//
//            Logger.Log ("hehe");
//
////            Logger.Log (content);
////            Logger.Log (content);
//        }

    }
}
