using Data;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Artifacts
{
    public class ArtifactUI : DefaultUpdater
    {
        #region fields
        [SerializeField] private ArtifactInit artifact;
        [SerializeField] protected Image mainImage;
        [SerializeField] private LanguageLoad mainText;
        [SerializeField] private ShowHelp help;
        [SerializeField] private bool updateHelp = true;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            artifact.OnValuesUpdate += UpdateValues;
        }
        protected override void OnDisable()
        {
            artifact.OnValuesUpdate -= UpdateValues;
        }
        protected virtual void UpdateValues()
        {
            ArtifactInfo artifactInfo = GetArtifactInfo();
            mainImage.sprite = artifactInfo.sprite;
            mainText.ChangeID(artifact.artifactInfo.id);
            if (!updateHelp) return;
            help.id = artifactInfo.helpID;
        }
        protected ArtifactInfo GetArtifactInfo() => PrefabsData.instance.artifactInfo.Find(x => x.effect == artifact.artifactInfo.effect);
        protected string GetArtifactInfoText() => TextOutline.languageData.helpData[GetArtifactInfo().helpID];
        #endregion methods
    }
}