

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bakery.Inputs.Test
{
    public class InputUnitTest : MonoBehaviour
    {
        [SerializeField] private InputMap _test1InputMap;
        [SerializeField] private InputMap _test2InputMap;
        [SerializeField] private Button _tes1Button;
        [SerializeField] private Button _test2Button;

        [SerializeField] private TextMeshProUGUI _text;

        void OnEnable()
        {
            _tes1Button.onClick.AddListener(() => InputServices.SwitchInputMap(_test1InputMap));
            _test2Button.onClick.AddListener(() => InputServices.SwitchInputMap(_test2InputMap));

            InputEvents.OnPointerEnter += OnEnter;
            InputEvents.OnPointerExit += OnExit;

            InputMapTest2.OnTest2 += (v) => Debug.Log("Move: " + v);
            InputMapTest1.OnTest1 += () => Debug.Log("Skip");
        }

        void OnDisable()
        {
            _tes1Button.onClick.RemoveAllListeners();
            _test2Button.onClick.RemoveAllListeners();


            InputEvents.OnPointerEnter -= OnEnter;
            InputEvents.OnPointerExit -= OnExit;

            InputMapTest2.OnTest2 -= (v) => Debug.Log("Move: " + v);
            InputMapTest1.OnTest1 -= () => Debug.Log("Skip");

        }

        private void OnEnter()
        {
            var obj = InputServices.GetFirstObjectUnderCursor();
            if (obj != null)
                _text.text = obj.name;
            else
                _text.text = "No object under cursor";

            _text.text += "\n" + InputServices.GetCurrentInputMap().name;
        }

        private void OnExit()
        {
            _text.text = "\n" + InputServices.GetCurrentInputMap().name;
        }
    }
}