using System.Collections.Generic;
using System.Threading;

namespace CommonBaseUI.Common
{
    public static class ThreadManager
    {
        private static Dictionary<string, List<Thread>> ThreadList;

        /// <summary>
        /// 添加指定画面的线程
        /// </summary>
        /// <param name="key">所属画面的全称，如：CommonBaseUI.Controls.MyTab</param>
        /// <param name="thread"></param>
        public static void _AddThread(string key, Thread thread)
        {
            if (ThreadList == null)
            {
                ThreadList = new Dictionary<string, List<Thread>>();
            }
            if (!ThreadList.ContainsKey(key))
            {
                ThreadList.Add(key, new List<Thread>());
            }

            ThreadList[key].Add(thread);
        }

        /// <summary>
        /// 移除指定画面的线程
        /// </summary>
        /// <param name="key"></param>
        public static void _RemoveThread(string key)
        {
            if (ThreadList != null)
            {
                if (ThreadList.ContainsKey(key))
                {
                    var list = ThreadList[key];
                    foreach (var thread in list)
                    {
                        thread.Abort();
                    }
                }
                ThreadList.Remove(key);
            }
        }
    }
}
