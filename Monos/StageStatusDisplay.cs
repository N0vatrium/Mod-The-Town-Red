using TMPro;
using UnityEngine;
using static MTTR.Datastore;

namespace MTTR.Monos
{
    public class StageStatusDisplay : MonoBehaviour
    {
        public string LoadedName = string.Empty;
        private TextMeshPro _textMeshPro;

        public void UpdateStatus()
        {
            _textMeshPro = gameObject.GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
            var size = 6;
            _textMeshPro.fontSize = size;
            _textMeshPro.fontSizeMin = size;
            _textMeshPro.fontSizeMax = size;

            _textMeshPro.enableAutoSizing = true;

            SetStatus(Instance.GetStageStatus());
        }

        public void SetStatus(StageStatus status, string postfix = "")
        {
            var text = "";

            text = status switch
            {
                StageStatus.MISSING => "File asset.json is missing<br>from " + STAGE_PATH_NAME,
                StageStatus.UNLOADED => "Unloaded",
                StageStatus.LOADED => "Loaded: " + Instance.Store["mttr.stage"].GameObject.name,
                StageStatus.DATA_ERROR => "Data error, check logs",
                StageStatus.MODEL_ERROR => "Model error, check logs",
                _ => "How did you get this error??",
            };

            var header = "<b>Status</b><br>";

            _textMeshPro.text = header + text + postfix;
        }
    }
}
