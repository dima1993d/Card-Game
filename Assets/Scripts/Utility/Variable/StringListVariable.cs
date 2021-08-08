using System.Collections.Generic;
using UnityEngine;

namespace Utility.Variable
{
    [CreateAssetMenu(fileName = "New String List Variable", menuName = "Variable/StringListVariable", order = 51)]
    public class StringListVariable : ScriptableObject
    {
        public List<string> Value;
    }
}
