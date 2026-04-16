using System;
using System.Collections.Generic;
using Project.Core.Runtime.Framework;

namespace Project.Narrative.Scripts
{
    [Serializable]
    public sealed class VNSaveData
    {
        public bool isPlaying;
        public string chapterId;
        public string sequenceId;
        public string nodeId;
        public List<string> visitedNodeIds = new();
    }

    public sealed class VNChoiceViewData
    {
        public VNChoiceViewData(string choiceId, string text)
        {
            ChoiceId = choiceId;
            Text = text;
        }

        public string ChoiceId { get; }
        public string Text { get; }
    }

    public interface IVNSaveable : ISaveable<VNSaveData>
    {
    }
}
