using BNG;
using UnityEngine;

namespace Notteam.VRIFExtensions
{
    /// <summary>
    /// Component that allows changing the color of child Renderer components when they are grabbed
    /// </summary>
    public class GrabbableRendererColorChanger : GrabbableEvents
    {
        [Tooltip("If the flag is active, the component will set a random color.")]
        [SerializeField] private bool  setRandomColor;
        [Space]
        [SerializeField] private Color grabbedColor = Color.white;

        private Material   _cachedRendererMaterial;

        private Renderer[] _childRenderers;
        private Material[] _childRenderersInitialMaterials;

        protected override void Awake()
        {
            base.Awake();

            _childRenderers = GetComponentsInChildren<Renderer>();

            // Here we create an array of initial materials of the child renderer components.
            _childRenderersInitialMaterials = new Material[_childRenderers.Length];

            for (var i = 0; i < _childRenderers.Length; i++)
            {
                var renderer = _childRenderers[i];

                // We create an instance of the first material from the render components
                // And cache it to assign it the color we need and not change the material used on all objects.
                if (i == 0)
                    _cachedRendererMaterial = new Material(renderer.sharedMaterial);

                // Getting the initial material from the renderer component
                _childRenderersInitialMaterials[i] = renderer.sharedMaterial;
            }
        }

        /// <summary>
        /// The method sets the "grabbedColor" color for child renderer components.
        /// </summary>
        private void SetGrabbedColor()
        {
            // This sets a random color if "setRandomColor" is enabled, or a "grabbedColor" color if "setRandomColor" is disabled.
            var color = setRandomColor ? Random.ColorHSV() : grabbedColor;

            // Setting color to cached material
            _cachedRendererMaterial.SetColor("_Color", color);

            for (var i = 0; i < _childRenderers.Length; i++)
                _childRenderers[i].sharedMaterial = _cachedRendererMaterial;
        }

        /// <summary>
        /// The method sets the initial color for child renderer components.
        /// </summary>
        private void SetInitialColors()
        {
            for (var i = 0; i < _childRenderers.Length; i++)
            {
                var initialMaterial = _childRenderersInitialMaterials[i];

                _childRenderers[i].sharedMaterial = initialMaterial;
            }
        }

        public override void OnGrab(Grabber grabber)
        {
            SetGrabbedColor();
        }

        public override void OnRelease()
        {
            SetInitialColors();
        }
    }
}