namespace Utility.Variable
{
    using UnityEngine;
    [CreateAssetMenu(fileName = "New BoolVariable", menuName = "Variable/BoolVariable", order = 51)]
    public class BoolVariable : ScriptableObject
    {
        public bool Value;

        public void SetValue(bool Value)
        {
            this.Value = Value;
        }
    }
}

