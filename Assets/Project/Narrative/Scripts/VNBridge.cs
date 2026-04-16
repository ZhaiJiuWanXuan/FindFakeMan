using System.Collections.Generic;
using Project.Core.Runtime.Framework;
using Project.Core.Runtime.Managers;
using UnityEngine;

namespace Project.Narrative.Scripts
{
    public sealed class VNBridge
    {
        public bool ConditionsMet(IReadOnlyList<VNFlagCondition> requiredFlags, IReadOnlyList<VNFlagCondition> blockedFlags)
        {
            Services.TryGet<FlagManager>(out var flagManager);
            return MatchRequiredFlags(flagManager, requiredFlags) && MatchBlockedFlags(flagManager, blockedFlags);
        }

        public void ApplyFlags(IReadOnlyList<string> setFlags, IReadOnlyList<string> clearFlags)
        {
            if (!Services.TryGet<FlagManager>(out var flagManager))
            {
                return;
            }

            if (setFlags != null)
            {
                foreach (var flagId in setFlags)
                {
                    flagManager.Set(flagId, true);
                }
            }

            if (clearFlags != null)
            {
                foreach (var flagId in clearFlags)
                {
                    flagManager.Set(flagId, false);
                }
            }
        }

        public void EnterVisualNovelState()
        {
            if (Services.TryGet<InteractionManager>(out var interactionManager))
            {
                interactionManager.PauseInteractions();
            }

            if (Services.TryGet<GameManager>(out var gameManager))
            {
                gameManager.SwitchState(GameState.VisualNovel);
            }

            if (Services.TryGet<UIManager>(out var uiManager))
            {
                uiManager.ShowVNPanel();
            }
        }

        public void ExitVisualNovelState()
        {
            if (Services.TryGet<UIManager>(out var uiManager))
            {
                uiManager.HideVNChoices();
                uiManager.HideVNPanel();
            }

            if (Services.TryGet<InteractionManager>(out var interactionManager))
            {
                interactionManager.ResumeInteractions();
            }

            if (Services.TryGet<GameManager>(out var gameManager))
            {
                gameManager.RevertState();
            }
        }

        public void PresentNode(VNNodeConfig node)
        {
            if (node == null)
            {
                return;
            }

            if (Services.TryGet<CGManager>(out var cgManager))
            {
                if (!string.IsNullOrWhiteSpace(node.backgroundId))
                {
                    cgManager.ShowBackground(node.backgroundId);
                }

                if (!string.IsNullOrWhiteSpace(node.cgId))
                {
                    cgManager.PlayCG(node.cgId);
                }
            }

            if (Services.TryGet<AudioManager>(out var audioManager))
            {
                if (!string.IsNullOrWhiteSpace(node.bgmId))
                {
                    audioManager.PlayBGM(node.bgmId, 1f);
                }

                if (!string.IsNullOrWhiteSpace(node.sfxId))
                {
                    audioManager.PlaySFX(node.sfxId, 1f);
                }

                if (!string.IsNullOrWhiteSpace(node.voiceId))
                {
                    audioManager.PlayVoice(node.voiceId, 1f);
                }
            }

            if (Services.TryGet<UIManager>(out var uiManager))
            {
                if (!string.IsNullOrWhiteSpace(node.screenEffectId))
                {
                    uiManager.PlayGlitchEffect(0.5f);
                }

                if (!string.IsNullOrWhiteSpace(node.portraitId))
                {
                    uiManager.ShowVNPortrait(node.portraitId, node.portraitPosition);
                }

                uiManager.ShowVNLine(node.speakerName, node.text, node.secondsPerCharacter);
            }
        }

        public void CompleteLine()
        {
            if (Services.TryGet<UIManager>(out var uiManager))
            {
                uiManager.CompleteVNLine();
            }
        }

        public void ShowChoices(IReadOnlyList<VNChoiceViewData> choices, System.Action<string> onSelected)
        {
            if (Services.TryGet<UIManager>(out var uiManager))
            {
                uiManager.ShowVNChoices(choices, onSelected);
            }
        }

        public void HideChoices()
        {
            if (Services.TryGet<UIManager>(out var uiManager))
            {
                uiManager.HideVNChoices();
            }
        }

        private static bool MatchRequiredFlags(FlagManager flagManager, IReadOnlyList<VNFlagCondition> requiredFlags)
        {
            if (requiredFlags == null)
            {
                return true;
            }

            foreach (var condition in requiredFlags)
            {
                if (condition == null || string.IsNullOrWhiteSpace(condition.flagId))
                {
                    continue;
                }

                var value = flagManager != null && flagManager.Get(condition.flagId);
                if (value != condition.expectedValue)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool MatchBlockedFlags(FlagManager flagManager, IReadOnlyList<VNFlagCondition> blockedFlags)
        {
            if (blockedFlags == null)
            {
                return true;
            }

            foreach (var condition in blockedFlags)
            {
                if (condition == null || string.IsNullOrWhiteSpace(condition.flagId))
                {
                    continue;
                }

                var value = flagManager != null && flagManager.Get(condition.flagId);
                if (value == condition.expectedValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
