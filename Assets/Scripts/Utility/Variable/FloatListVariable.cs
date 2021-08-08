using System.Collections.Generic;
using UnityEngine;

namespace Utility.Variable
{
    [CreateAssetMenu(fileName = "New Float List Variable", menuName = "Variable/FloatListVariable", order = 51)]
    public class FloatListVariable : ScriptableObject
    {
        public List<float> Value;
    }
}
