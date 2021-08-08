using System.Collections.Generic;
using UnityEngine;

namespace Utility.Variable
{
    [CreateAssetMenu(fileName = "New Float Variable List", menuName = "Variable/FloatVariableList", order = 51)]
    public class FloatVariableList : ScriptableObject
    {
        public List<FloatVariable> Value;
    }
}
