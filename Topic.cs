using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Topic
    {
        private int id;
        private List<string> keys;

        public Topic(int id)
        {
            this.id = id;
            this.keys = new List<string>();
        }

        public void AddKey(string key)
        {
            this.keys.Add(key);
        }

        public List<String> GetKeys()
        {
            return this.keys;
        }

        public int GetId()
        {
            return this.id;
        }
    }
}
