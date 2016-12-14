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
            string qString = "findall(tuple(S, N), fuckingGapPred(S, " + board + ", N), L)"; //"b1(B), isgapLeft(1, 3, B, N)";
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

        //[play({0},{1},Tile({3},{4}),....
        //public string Test()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    //Environment.SetEnvironmentVariable("SWI_HOME_DIR", "C:\\Program Files (x86)\\swipl\\bin");
        //    //if (!PlEngine.IsInitialized)
        //    //{
        //    String[] param = { "-q" };  // suppressing informational and banner messages
        //    PlEngine.Initialize(param);
        //    PlQuery.PlCall("assert(father(martin, inka))");
        //    PlQuery.PlCall("assert(father(uwe, gloria))");
        //    PlQuery.PlCall("assert(father(uwe, melanie))");
        //    PlQuery.PlCall("assert(father(uwe, ayala))");
        //    using (var q = new PlQuery("father(P, C), atomic_list_concat([P,' is_father_of ',C], L)"))
        //    {
        //        foreach (PlQueryVariables v in q.SolutionVariables)
        //        {
        //            //Console.WriteLine(v["L"].ToString());
        //            sb.Append(v["L"].ToString() + "\n");
        //        }

        //        //Console.WriteLine("all children from uwe:");
        //        sb.Append("all children from uwe:\n");
        //        q.Variables["P"].Unify("uwe");
        //        foreach (PlQueryVariables v in q.SolutionVariables)
        //        {
        //            //Console.WriteLine(v["C"].ToString());
        //            sb.Append(v["C"].ToString() + "\n");
        //        }
        //    }
        //    //PlEngine.PlCleanup();
        //    //Console.WriteLine("finshed!");
        //    sb.Append("finished!\n");
        //    //}
        //    //else
        //    //{
        //    //    sb.Append("...\n");
        //    //}
        //    return sb.ToString();
        //}

        public void Cleanup()
        {
            PlEngine.PlCleanup();
        }
    }
}
