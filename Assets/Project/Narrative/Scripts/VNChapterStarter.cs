using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Narrative.Scripts
{
    public sealed class VNChapterStarter : MonoBehaviour
    {
        [SerializeField] private VNChapterConfig chapter;
        [SerializeField] private bool playOnStart = true;

        private async void Start()
        {
            if (!playOnStart || chapter == null)
            {
                return;
            }

            var director = FindAnyObjectByType<VNDirector>();
            if (director == null)
            {
                Debug.LogWarning("VNChapterStarter could not find VNDirector in scene.");
                return;
            }

            await director.StartChapter(chapter);
        }
    }
}
