using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;


namespace vr_vs_kms
{
    public class ContaminationArea : MonoBehaviour
    {
        [System.Serializable]
        public struct BelongToProperties
        {
            public Color mainColor;
            public Color secondColor;

        }

        public BelongToProperties nobody;
        public BelongToProperties virus;
        public BelongToProperties scientist;

        private float faerieSpeed;
        public float cullRadius = 5f;

        private float radius = 1f;
        private ParticleSystem pSystem;
        private WindZone windZone;
        private int remainingGrenades;
        public float inTimer = 0f;
        private CullingGroup cullGroup;

        private bool playerInZone = false;
        private bool virusInZone = false;

        void Start()
        {
            populateParticleSystemCache();
            setupCullingGroup();

            BelongsToNobody();
        }

        private void populateParticleSystemCache()
        {
            pSystem = this.GetComponentInChildren<ParticleSystem>();
        }


        /// <summary>
        /// This manage visibility of particle for the camera to optimize the rendering.
        /// </summary>
        private void setupCullingGroup()
        {
            Debug.Log($"setupCullingGroup {Camera.main}");
            cullGroup = new CullingGroup();
            cullGroup.targetCamera = Camera.main;
            cullGroup.SetBoundingSpheres(new BoundingSphere[] { new BoundingSphere(transform.position, cullRadius) });
            cullGroup.SetBoundingSphereCount(1);
            cullGroup.onStateChanged += OnStateChanged;
        }

        void OnStateChanged(CullingGroupEvent cullEvent)
        {
            //Debug.Log($"cullEvent {cullEvent.isVisible}");
            if (cullEvent.isVisible)
            {
                pSystem.Play(true);
            }
            else
            {
                pSystem.Pause();
            }
        }


        void Update()
        {
            //get time in zone and change zone color after 5s
            if (playerInZone && !virusInZone)
            {
                inTimer += Time.deltaTime;
                if (inTimer >= 5f)
                    BelongsToScientists();
            }
            else if (!playerInZone && virusInZone)
            {
                inTimer += Time.deltaTime;
                if (inTimer >= 5f)
                    BelongsToVirus();
            }
            else inTimer = 0f;
        }

        //Verify if a player enter the zone
        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "VRPlayer")
            {
                virusInZone = true;
            }
            if (collider.gameObject.tag == "Player")
                playerInZone = true;
        }

        //Verify if a player exit the zone
        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                playerInZone = false;
            }

            if (collider.gameObject.tag == "VRPlayer")
                virusInZone = false;
        }

        private void ColorParticle(ParticleSystem pSys, Color mainColor, Color accentColor)
        {
            // TODO: Solution to color particle 
            //ParticleSystem.ColorOverLifetimeModule colors = new ParticleSystem.ColorOverLifetimeModule();
            //colors.color.colorMin = mainColor;
            //colors.color.colorMax = accentColor;
            //pSys.colorOverLifetime = colors;

            pSys.startColor = mainColor;
        }

        public void BelongsToNobody()
        {
            ColorParticle(pSystem, nobody.mainColor, nobody.secondColor);
        }

        public void BelongsToVirus()
        {
            ColorParticle(pSystem, virus.mainColor, virus.secondColor);
            Debug.Log("colrs");
        }

        public void BelongsToScientists()
        {
            ColorParticle(pSystem, scientist.mainColor, scientist.secondColor);
        }

        void OnDestroy()
        {
            if (cullGroup != null)
                cullGroup.Dispose();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, cullRadius);
        }
    }
}