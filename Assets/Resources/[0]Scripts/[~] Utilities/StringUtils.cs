using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    public class StringUtils{

        public static string RedString(string text)
        {
            string tmpString = "<color=red>" + text + "</color>";

            return tmpString;
        }

        public static string YellowString(string text)
        {
            string tmpString = "<color=yellow>" + text + "</color>";

            return tmpString;
        }
    }
}
