using TMPro;
using UnityEngine;

namespace Utility.Variable
{
    [CreateAssetMenu(fileName = "New IntVariable", menuName = "Variable/IntVariable", order = 51)]
    public class IntVariable : ScriptableObject
    {
        public int Value;

        public void SetValue(int Value)
        {
            this.Value = Value;
        }
        public void SetValue(TextMeshProUGUI Value)
        {
            this.Value = int.Parse(Value.text);
        }
    }

}
