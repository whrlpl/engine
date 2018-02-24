using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Pattern
{
    public class ListBuilder<T>
    {
        protected List<T> list = new List<T>();

        public ListBuilder<T> Add(T item)
        {
            list.Add(item);
            return this;
        }

        public List<T> GetList()
        {
            return list;
        }
    }

    public class KeyValuePairListBuilder<T1, T2> : ListBuilder<KeyValuePair<T1, T2>>
    {
        public KeyValuePairListBuilder<T1, T2> Add(T1 key, T2 value)
        {
            list.Add(new KeyValuePair<T1, T2>(key, value));
            return this;
        }
    }
}
