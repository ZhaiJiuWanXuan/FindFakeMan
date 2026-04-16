using System;
using System.Collections.Generic;
using UnityEngine;
using Project.Core.Runtime.Framework;

namespace Project.Narrative.Scripts
{
    public enum VNPortraitPosition
    {
        Center = 0,
        Left = 1,
        Right = 2
    }

    public enum VNEndActionType
    {
        None = 0,
        ReturnToPreviousState = 1,
        SwitchGameState = 2,
        LoadScene = 3,
        StartChapter = 4
    }

    [Serializable]
    public sealed class VNFlagCondition
    {
        public string flagId;
        public bool expectedValue = true;
    }

    [Serializable]
    public sealed class VNChoiceConfig
    {
        public string choiceId;
        [TextArea] public string text;
        public string targetSequenceId;
        public string targetNodeId;
        public List<VNFlagCondition> requiredFlags = new();
        public List<VNFlagCondition> blockedFlags = new();
        public List<string> setFlags = new();
        public List<string> clearFlags = new();
    }

    [Serializable]
    public sealed class VNNodeConfig
    {
        public string nodeId;
        public string speakerId;
        public string speakerName;
        [TextArea(2, 6)] public string text;
        public float secondsPerCharacter = 0.04f;
        public string backgroundId;
        public string cgId;
        public string screenEffectId;
        public string portraitId;
        public VNPortraitPosition portraitPosition = VNPortraitPosition.Center;
        public string voiceId;
        public string sfxId;
        public string bgmId;
        public bool autoContinue;
        public float autoContinueDelay = 0.2f;
        public bool waitForExternalSignal;
        public List<VNFlagCondition> requiredFlags = new();
        public List<VNFlagCondition> blockedFlags = new();
        public List<string> setFlags = new();
        public List<string> clearFlags = new();
        public string nextNodeId;
        public string elseNodeId;
        public List<VNChoiceConfig> choices = new();
    }

    [Serializable]
    public sealed class VNSequenceConfig
    {
        public string sequenceId;
        public List<VNFlagCondition> requiredFlags = new();
        public List<VNFlagCondition> blockedFlags = new();
        public List<VNNodeConfig> nodes = new();
        public string nextSequenceId;
    }

    [Serializable]
    public sealed class VNEndAction
    {
        public VNEndActionType actionType = VNEndActionType.ReturnToPreviousState;
        public string targetSceneName;
        public string targetChapterId;
        public GameState targetGameState = GameState.Exploration;
    }

    [CreateAssetMenu(menuName = "Project/Narrative/VN Chapter Config")]
    public sealed class VNChapterConfig : ScriptableObject
    {
        [SerializeField] private string chapterId;
        [SerializeField] private string title;
        [SerializeField] private string startSequenceId;
        [SerializeField] private List<string> setFlagsOnStart = new();
        [SerializeField] private List<string> clearFlagsOnStart = new();
        [SerializeField] private List<VNSequenceConfig> sequences = new();
        [SerializeField] private VNEndAction endAction = new();

        public string ChapterId => chapterId;
        public string Title => title;
        public string StartSequenceId => startSequenceId;
        public IReadOnlyList<string> SetFlagsOnStart => setFlagsOnStart;
        public IReadOnlyList<string> ClearFlagsOnStart => clearFlagsOnStart;
        public IReadOnlyList<VNSequenceConfig> Sequences => sequences;
        public VNEndAction EndAction => endAction;
    }
}
