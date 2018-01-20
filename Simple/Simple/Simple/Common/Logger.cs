using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple
{
    //TODO:多个Logger通道
    class Logger
    {
        static StringBuilder  m_builder = new StringBuilder();
        static string m_split = " ";
        public static void Error(params string[] message)
        {
            for (int i = 0; i < message.Length; ++i )
            {
                m_builder.Append(message[i]);
                m_builder.Append(m_split);
            }
            Console.WriteLine(m_builder.ToString());
            m_builder.Clear();
        }
    }
}
