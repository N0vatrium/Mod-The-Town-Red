using TMPro;

namespace MTTR.Extensions
{
    public static class WorldButtonExtensions
    {
        public static void SetText(this WorldButton button, string text)
        {
            var textComps = button.GetComponentsInChildren<TextMeshPro>();
            foreach (var textComp in textComps)
            {
                textComp.text = text;
            }

            button.label = text;
        }
    }
}
