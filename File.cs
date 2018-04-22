using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class File
    {
        private string name;
        private List<int> topics;

       public File(string name)
       {
            this.name = name;
            topics = new List<int>();
       }
      
       public void addTopic(int id)
       {
            topics.Add(id);
       }

        public List<int> GetTopics()
        {
            return this.topics;
        }
    }
}
