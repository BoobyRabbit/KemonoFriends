using UnityEngine;

namespace Title
{
    public class TitleSceneEntry : SceneEntry
    {
        /// <summary>
        /// 続きから
        /// </summary>
        public void ContinueOnSubmit()
        {

        }

        /// <summary>
        /// 始めから
        /// </summary>
        public void StartOnSubmit()
        {
            GameManager.Instance.ChangeScene("SampleField");
        }

        /// <summary>
        /// 終了
        /// </summary>
        public void ExitOnSubmit()
        {
            Application.Quit();
        }
    }
}
