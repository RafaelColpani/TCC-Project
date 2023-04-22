using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class BoxGroupTest : MonoBehaviour
    {
        [FoldoutAttributeAttribute("Integers")]
        public int int0;
        [FoldoutAttributeAttribute("Integers")]
        public int int1;

        [FoldoutAttributeAttribute("Floats")]
        public float float0;
        [FoldoutAttributeAttribute("Floats")]
        public float float1;

        [FoldoutAttributeAttribute("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 slider0;
        [FoldoutAttributeAttribute("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 slider1;

        public string str0;
        public string str1;

        [FoldoutAttributeAttribute]
        public Transform trans0;
        [FoldoutAttributeAttribute]
        public Transform trans1;
    }
}
