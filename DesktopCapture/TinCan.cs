using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RusticiSoftware.TinCanAPILibrary;
using RusticiSoftware.TinCanAPILibrary.Model;
using System.Windows.Forms;

namespace DesktopCapture
{
    class TinCan
    {
        private static TCAPI tincan;
        private static bool connectedTinCan;
        private static string username;

        private static Queue<Statement> _offlineQueuedStatements = new Queue<Statement>();

        public static void ConnectToTinCan(string usrName, string passwd)
        {
            try
            {
                tincan = new TCAPI(new Uri("http://35.9.22.105:8000/xapi"), new BasicHTTPAuth(usrName, passwd),
                            RusticiSoftware.TinCanAPILibrary.Helper.TCAPIVersion.TinCan1p0p0);
                connectedTinCan = true;
            }
            catch (Exception e)
            {
                const string msg = "Invalid login information.  Please re-try.";
                connectedTinCan = false;
            }
            if (connectedTinCan)
                username = usrName;

        }

        public static void SendStatement(string activity)
        {
            string email = "mailto:";
            email += username;

            Statement[] statements = new Statement[1];
            Activity newAct = new Activity("http://35.9.22.105/xapi");

            LanguageMap inter = new LanguageMap();
            inter.Add("en-US", "Interacted with");
            System.Uri verbURI = new System.Uri("http://verbs/interaction");
            StatementVerb interact = new StatementVerb(verbURI, inter);

            newAct.Definition = new ActivityDefinition();
            newAct.Definition.Name = new LanguageMap();
            newAct.Definition.Name.Add("en-US", activity);
            Random rand = new Random();
            int random = rand.Next();
            newAct.Id = "http://desktopapp/" + random.ToString();

            statements[0] = new Statement(new Actor(username, email), interact, newAct);
            
            try
            {
                tincan.StoreStatements(statements);
            }
            catch (Exception e)
            {
                _offlineQueuedStatements.Enqueue(statements[0]);
            }
        }
    }
}
