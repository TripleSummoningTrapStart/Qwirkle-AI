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
        public PrologCommunicator()
        {

        }

        public string Test()
        {
            StringBuilder sb = new StringBuilder();
            //Environment.SetEnvironmentVariable("SWI_HOME_DIR", "C:\\Program Files (x86)\\swipl\\bin");
            if (!PlEngine.IsInitialized)
            {
                String[] param = { "-q" };  // suppressing informational and banner messages
                PlEngine.Initialize(param);
                PlQuery.PlCall("assert(father(martin, inka))");
                PlQuery.PlCall("assert(father(uwe, gloria))");
                PlQuery.PlCall("assert(father(uwe, melanie))");
                PlQuery.PlCall("assert(father(uwe, ayala))");
                using (var q = new PlQuery("father(P, C), atomic_list_concat([P,' is_father_of ',C], L)"))
                {
                    foreach (PlQueryVariables v in q.SolutionVariables)
                    {
                        //Console.WriteLine(v["L"].ToString());
                        sb.Append(v["L"].ToString() + "\n");
                    }

                    //Console.WriteLine("all children from uwe:");
                    sb.Append("all children from uwe:\n");
                    q.Variables["P"].Unify("uwe");
                    foreach (PlQueryVariables v in q.SolutionVariables)
                    {
                        //Console.WriteLine(v["C"].ToString());
                        sb.Append(v["C"].ToString() + "\n");
                    }
                }
                PlEngine.PlCleanup();
                //Console.WriteLine("finshed!");
                sb.Append("finished!\n");
            }
            else
            {
                sb.Append("...\n");
            }
            return sb.ToString();
        }
    }
}