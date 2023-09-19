using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;

namespace GameMenu.Inventory.Chests
{
    public class ChestParticles : SingleSceneInstance
    {
        #region fields & properties
        public static ChestParticles instance { get; private set; }

        [SerializeField] private ParticleSystem particles;
        [SerializeField] private List<Sprite> cardTierSprites;
        [SerializeField] private Sprite chestSprite;
        [SerializeField] private Sprite potionSprite;
        [SerializeField] private Sprite artifactSprite;
        [SerializeField] private Sprite silverSprite;
        [SerializeField] private Sprite goldSprite;
        [SerializeField] private AudioClip chestOpenClip;
        [SerializeField] private AudioClip chestLootClip;
        #endregion fields & properties

        #region methods
        public void PlayChestOpenClip() => AudioManager.PlayClip(chestOpenClip, SoundType.Sound);
        public void PlayChestLootClip() => AudioManager.PlayClip(chestLootClip, SoundType.Sound);
        public void StartParcticleSystem(ChestSpawned chestSpawned)
        {
            ClearParticleSprites();
            particles.textureSheetAnimation.AddSprite(cardTierSprites[0]);
            var newEmission = particles.emission;
            newEmission.rateOverTime = 2;
            particles.textureSheetAnimation.SetSprite(0, chestSpawned.lootType == LootType.Card ?
                 cardTierSprites[PrefabsData.instance.cardPrefabs[chestSpawned.id].rareTier] : GetSpriteFromLootType(chestSpawned.lootType));
            particles.Play();
        }
        public void StopParticleSystem() => particles.Stop();
        public void StartParcticleSystem(ChestInfo chestInfo, List<ChestSpawned> chestSpawneds)
        {
            var newEmission = particles.emission;
            newEmission.rateOverTime = 2 * chestSpawneds.Count;
            ClearParticleSprites();
            foreach (ChestLoot el in chestInfo.chestLoot)
                particles.textureSheetAnimation.AddSprite(el.type == LootType.Card ? cardTierSprites[el.tier] : GetSpriteFromLootType(el.type));
            particles.Play();
        }
        private Sprite GetSpriteFromLootType(LootType lootType) => lootType switch
        {
            LootType.Chest => chestSprite,
            LootType.Potion => potionSprite,
            LootType.Artifact => artifactSprite,
            LootType.Silver => silverSprite,
            LootType.Gold => goldSprite,
            _ => throw new System.NotImplementedException(),
        };
        private void ClearParticleSprites()
        {
            int spritesCount = particles.textureSheetAnimation.spriteCount;
            for (int i = 0; i < spritesCount; i++)
                particles.textureSheetAnimation.RemoveSprite(0);
        }
        public IEnumerator SpawnedLootAnimation(Transform lootObject, Vector3 finalPosition, Vector3 finalLocalScale)
        {
            float animationDuration = 2f;
            lootObject.transform.localScale = Vector3.one * 0.1f;
            StartCoroutine(SetLocalScaleUp(lootObject, finalLocalScale, animationDuration));
            yield return RotateObjectToDegrees(lootObject, 1080, animationDuration);
            lootObject.transform.position = finalPosition;
        }
        private IEnumerator RotateObjectToDegrees(Transform obj, float angle, float duration)
        {
            int stepsCount = (int)angle / 180;
            float dividedDuration = duration / stepsCount;
            float durationSpent = 0f;
            for (int i = 0; i < stepsCount; i++)
            {
                if (i % 2 == 0)
                {
                    yield return RotateToDegrees(obj, 180, dividedDuration);
                }
                else
                {
                    yield return RotateToDegrees(obj, 1, dividedDuration);
                    yield return RotateToDegrees(obj, 0, Time.fixedDeltaTime);
                    yield return RotateToDegrees(obj, -1, Time.fixedDeltaTime);
                    durationSpent += Time.fixedDeltaTime * 2;
                }
                durationSpent += dividedDuration;
            }
            yield return RotateToDegrees(obj, angle - stepsCount * 180, dividedDuration);
        }
        private IEnumerator SetLocalScaleUp(Transform obj, Vector3 scale, float duration)
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            float lerp = fixedDeltaTime;
            float step = Mathf.Abs(obj.localScale.x - scale.x) * fixedDeltaTime / duration;

            while (duration > lerp)
            {
                obj.transform.localScale += Vector3.one * step;
                lerp += fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            obj.transform.localScale = scale;
        }
        private IEnumerator RotateToDegrees(Transform obj, float angle, float duration)
        {
            Vector3 axis = Vector3.forward;
            float fixedDeltaTime = Time.fixedDeltaTime;
            Vector3 localEulerAngles = obj.transform.localEulerAngles;
            float speed = 1f / duration;
            float startAngle = (localEulerAngles * axis.x).x + (localEulerAngles * axis.y).y + (localEulerAngles * axis.z).z;
            float maxDegreesDelta = Mathf.Abs(startAngle - angle) * fixedDeltaTime * speed;
            float lerp = fixedDeltaTime;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, axis);
            while (duration > lerp)
            {
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, angleAxis, maxDegreesDelta);
                lerp += fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, angleAxis, 360);
        }
        #endregion methods

        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
    }
}