using TMPro;
using DG.Tweening;
using Cubeman.ScriptableObjects;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Cubeman.UI
{
    public sealed class UIUpgradeZone : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(6)]

        [SerializeField] private Image upgradeImage;

        [Space(6)]

        [SerializeField] private TextMeshProUGUI tmpUpgradeName;
        [SerializeField] private TextMeshProUGUI tmpUpgradeCost;
        [SerializeField] private TextMeshProUGUI tmpUpgradeDetails;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;

        [Space(6)]

        [SerializeField] private Color withResourcesColor = Color.white;
        [SerializeField] private Color outOfResourcesColor = Color.red;

        private StringBuilder _stringBuilder = new StringBuilder();

        public void RefreshUpgrade(PlayerData playerData, ResourcesData resourcesData)
        {
            upgradeImage.sprite = playerData.UpgradePreview;

            _stringBuilder.Length = 0;

            _stringBuilder.Append(playerData.UpgradeName);
            _stringBuilder.Append(" ");

            if(playerData.HealthLevel >= 5)
            {
                _stringBuilder.Append("LV MAX");
            }
            else
            {
                _stringBuilder.Append("LV ");
                _stringBuilder.Append(playerData.HealthLevel);
            }

            tmpUpgradeName.text = _stringBuilder.ToString();

            _stringBuilder.Length = 0;

            _stringBuilder.Append("Armature Increase: ");
            _stringBuilder.Append(playerData.Health);
            _stringBuilder.Append(" to ");
            _stringBuilder.Append(playerData.GetNextUpgrade());

            tmpUpgradeDetails.text = _stringBuilder.ToString();

            _stringBuilder.Length = 0;

            var convertLevelToArrayIndex = playerData.HealthLevel - 1;

            _stringBuilder.Append("Resources needed: ");
            _stringBuilder.Append(playerData.AmountToResourcesToUpgrade[convertLevelToArrayIndex]);
            _stringBuilder.Append(" / ");
            _stringBuilder.Append(resourcesData.Resources);

            _stringBuilder.Append("\n");
            _stringBuilder.Append("Capacitors needed: ");
            _stringBuilder.Append(playerData.AmountCapacitorsToUpgrade[convertLevelToArrayIndex]);
            _stringBuilder.Append(" / ");
            _stringBuilder.Append(resourcesData.Capacitors);

            tmpUpgradeCost.text = _stringBuilder.ToString();

            _stringBuilder.Length = 0;

            if (resourcesData.Resources < playerData.AmountToResourcesToUpgrade[convertLevelToArrayIndex]
                || resourcesData.Capacitors < playerData.AmountCapacitorsToUpgrade[convertLevelToArrayIndex])
            {
                tmpUpgradeCost.color = outOfResourcesColor;
            }
            else
            {
                tmpUpgradeCost.color = withResourcesColor;
            }
        }

        public void RefreshUpgrade(ProjectileData projectileData, ResourcesData resourcesData)
        {
            upgradeImage.sprite = projectileData.UpgradePreview;

            _stringBuilder.Length = 0;

            _stringBuilder.Append(projectileData.Name);
            _stringBuilder.Append(" ");

            if (projectileData.Level >= 5)
            {
                _stringBuilder.Append("LV MAX");
            }
            else
            {
                _stringBuilder.Append("LV ");
                _stringBuilder.Append(projectileData.Level);
            }

            tmpUpgradeName.text = _stringBuilder.ToString();

            _stringBuilder.Length = 0;

            _stringBuilder.Append("Damage Increase: ");
            _stringBuilder.Append(projectileData.Damage);
            _stringBuilder.Append(" to ");
            _stringBuilder.Append(projectileData.GetNextUpgrade());

            tmpUpgradeDetails.text = _stringBuilder.ToString();

            _stringBuilder.Length = 0;

            var convertLevelToArrayIndex = projectileData.Level - 1;

            _stringBuilder.Append("Resources needed: ");
            _stringBuilder.Append(projectileData.AmountToResourcesToUpgrade[convertLevelToArrayIndex]);
            _stringBuilder.Append(" / ");
            _stringBuilder.Append(resourcesData.Resources);

            _stringBuilder.Append("\n");
            _stringBuilder.Append("Capacitors needed: ");
            _stringBuilder.Append(projectileData.AmountCapacitorsToUpgrade[convertLevelToArrayIndex]);
            _stringBuilder.Append(" / ");
            _stringBuilder.Append(resourcesData.Capacitors);

            tmpUpgradeCost.text = _stringBuilder.ToString();

            _stringBuilder.Length = 0;

            if (resourcesData.Resources < projectileData.AmountToResourcesToUpgrade[convertLevelToArrayIndex]
                || resourcesData.Capacitors < projectileData.AmountCapacitorsToUpgrade[convertLevelToArrayIndex])
            {
                tmpUpgradeCost.color = outOfResourcesColor;
            }
            else
            {
                tmpUpgradeCost.color = withResourcesColor;
            }
        }

        public void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(EnableInteraction);
        }

        public void FadeOut()
        {
            DisableInteraction();

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration);
        }

        private void EnableInteraction()
        {
            canvasGroup.blocksRaycasts = true;
        }

        private void DisableInteraction()
        {
            canvasGroup.blocksRaycasts = false;
        }
    }
}