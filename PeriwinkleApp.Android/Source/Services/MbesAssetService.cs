using System.Collections.Generic;
using System.Xml;
using Android.Content;

namespace PeriwinkleApp.Android.Source.Services
{
    public interface IMbesAssetService
    {
        IList <string> GetInstructions ();
        IList <string> GetQuestions ();
    }

    public class MbesAssetService : AndroidAssetService, IMbesAssetService
    {
        private const string Filename = "Mbes.xml";
        private const string TagInstruction = "Instruction";
        private const string TagQuestion = "Question";

        public MbesAssetService (Context context) : base (context) { }

        public IList <string> GetInstructions ()
        {
            XmlDocument xmlDoc = AssetToXmlDocument (Filename);

            XmlNodeList instructions = xmlDoc.GetElementsByTagName (TagInstruction);

            return XmlNodeListToList (instructions);
        }
        
        public IList <string> GetQuestions ()
        {
            XmlDocument xmlDoc = AssetToXmlDocument (Filename);

            XmlNodeList questions = xmlDoc.GetElementsByTagName (TagQuestion);

            return XmlNodeListToList (questions);
        }
    }
}
