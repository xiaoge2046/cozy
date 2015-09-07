﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyPoker.Engine.Method
{
    public class CardCollectMethodParser
    {
        public static CardCollectMethod ParseCardCollectMethod(string s)
        {
            var ss = s.Split(':');
            switch (ss[0])
            {
                case "cpp":
                    return cppCardCollectMethod(ss[1]);
                case "lua":
                    return luaCardCollectMethod(ss[1]);
            }
            return null;
        }

        private static CardCollectMethod cppCardCollectMethod(string s)
        {
            return null;
        }

        private static CardCollectMethod luaCardCollectMethod(string s)
        {
            return null;
        }
    }
}
