using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace DesktopCapture
{
    public class FocusedWindow : IEquatable<FocusedWindow>
    {
        private static List<string> acceptablePrograms;

        public static void SetupPrograms()
        {
            List<string> popList = new List<string>();
            popList.Add("soffice.bin");
            popList.Add("WINWORD");
            popList.Add("Skype");
            popList.Add("chrome");
            popList.Add("EXCEL");
            popList.Add("AcroRd32");
            popList.Add("wmplayer");

            acceptablePrograms = popList;
        }

        public string WindowTitle
        {
            get; private set;
        }

        public string ProgramName
        {
            get; set;
        }

        public string FileName
        {
            get; private set;
        }

        private int _windowHandle;
        public int WindowHandle {get { return _windowHandle; }}

        public FocusedWindow(string name, int _handle)
        {
            WindowTitle = name;
            this._windowHandle = _handle;
        }

        public bool Equals(FocusedWindow other)
        {
            if (other.WindowHandle == WindowHandle)
            {
                return true;
            }
            return false;
        }


        public bool IsALearningActivity()
        {
            bool programInList = (from program in acceptablePrograms
                                 where program == ProgramName
                                 select program).Any();

            //bool programInList2 = acceptablePrograms.Where(x => x == ProgramName).Any();
            return programInList;

        }

        public DictionaryEntry GetProgramNameAndFileName()
        {
            for (int i = WindowTitle.Length - 1; i > 0; i--)
            {
                if (WindowTitle[i].Equals('-'))
                {
                    FileName = "with " + WindowTitle.Remove(i - 1);
                    break;
                }
                else
                {
                    FileName = WindowTitle;
                }
            }
            return new DictionaryEntry(FileName, ProgramName);
        }

        public static bool AddToProgramList(string programName)
        {
            //TODO
            return true;
        }
    }
}
