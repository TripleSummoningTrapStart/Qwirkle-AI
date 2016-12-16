using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SbsSW.SwiPlCs;

namespace Qwirkle
{
    public class PrologCommunicator
    {
        /// <summary>
        /// ONLY CALL ONCE PER EXECUTION
        /// </summary>
        public PrologCommunicator()
        {
            string baseDirectory = Environment.CurrentDirectory.Replace(@"\Qwirkle\Qwirkle\bin\Debug", "");
            Environment.SetEnvironmentVariable("SWI_HOME_DIR", baseDirectory + @"\swipl\boot32.prc");
            string pathvar = Environment.GetEnvironmentVariable("PATH");
            pathvar += baseDirectory + @"\swipl\bin";
            Environment.SetEnvironmentVariable("PATH", pathvar);
            if (!PlEngine.IsInitialized)
            {
                String[] param = { "-q", "-f", "QwirkleFacts.pl" }; // suppressing informational and banner messages
                PlEngine.Initialize(param);
            }
        }

        public bool TestMove(string move)
        {
            string qString = "isLegalPlay(" + move + ")";
            using (PlQuery q = new PlQuery(qString))
            {
                var v = new List<PlTermV>(q.Solutions);
                return v.Count > 0;
            }
        }

        public string GetMoves(string board, string hand)
        {
            StringBuilder sb = new StringBuilder();
            string qString = "plays(" + board + ',' + hand + ", P)"; //"b1(B), isgapLeft(1, 3, B, N)";
            using (PlQuery q = new PlQuery(qString))
            {
                foreach (PlQueryVariables v in q.SolutionVariables)
                {
                    sb.Append(v["P"]);
                }
            }
            return sb.ToString();

        }

        public string GetGaps(string board)
        {
            StringBuilder sb = new StringBuilder();
            string qString = "findall(tuple(S, N), Gaps(S, " + board + ", N), L)"; //"b1(B), isgapLeft(1, 3, B, N)";
            using (PlQuery q = new PlQuery(qString))
            {
                foreach (PlQueryVariables v in q.SolutionVariables)
                {
                    sb.Append(v["L"]);
                }
            }
            return sb.ToString();
        }

        public string Test()
        {
            StringBuilder sb = new StringBuilder();
            string qString = "b1(B), isgapMultiple([space(1, 3), space(8,3)], B, N)"; //"b1(B), isgapLeft(1, 3, B, N)";
            using (PlQuery q = new PlQuery(qString))
            {
                foreach (PlQueryVariables v in q.SolutionVariables)
                {
                    sb.Append(v["N"]);
                }
            }
            return sb.ToString();
        }


        public void Cleanup()
        {
            PlEngine.PlCleanup();
        }
    }
}
