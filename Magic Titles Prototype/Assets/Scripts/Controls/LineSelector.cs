using UnityEngine;

namespace Apps.Runtime.Controls
{
	public sealed class LineSelector : MonoBehaviour
	{
        [SerializeField] RectTransform[] _lines;

		public RectTransform GetLine()
		{
            // TODO note weighted selection, prevents overrlapping or gap
            int randomLine = Random.Range(0, _lines.Length);
            return _lines[randomLine];
        }

    }
}

