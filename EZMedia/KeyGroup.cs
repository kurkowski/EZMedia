using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZMedia
{
    public class StringKeyGroup<T> : List<T>
    {
        public string Key { get; private set; }

        public StringKeyGroup(string key)
        {
            Key = key;
        }

        public static List<StringKeyGroup<IMediaInfo>> CreateGroups(IEnumerable<IMediaInfo> mediaInfo)
        {
            List<StringKeyGroup<IMediaInfo>> groupedInfo = new List<StringKeyGroup<IMediaInfo>>();

            //creates 27 lists with keys for each letter of the alphabet and 1 extra for songs that don't start
            //with a letter
            for (int i = 0; i < 27; i++)
            {
                StringKeyGroup<IMediaInfo> list = new StringKeyGroup<IMediaInfo>(Convert.ToChar(i + 64).ToString());
                groupedInfo.Add(list);
            }

            foreach (IMediaInfo mi in mediaInfo)
            {
                string str = mi.Name + " ";
                char key = Convert.ToChar(str.Substring(0, 1).ToUpper());

                if (key >= 'A' && key <= 'Z')
                {
                    groupedInfo[key - 64].Add(mi);
                }
                else
                {
                    groupedInfo[0].Add(mi);
                }
            }

            return groupedInfo;
        }
    }
}
