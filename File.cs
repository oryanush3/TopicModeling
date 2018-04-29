using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class File
    {
        public string name { get; set; }
        public List<int> topics { get; set; }

       public File(string name)
       {
            this.name = name;
            topics = new List<int>();
       }
      
       public void addTopic(int id)
       {
            topics.Add(id);
       }
    }
}
