using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

namespace Util
{
    public static class SimpleScrollSnapExtensions
    {
        public static T Add<T>(this SimpleScrollSnap scroll, T panel, int index) where T : MonoBehaviour
        {
            if (scroll.NumberOfPanels != 0 && (index < 0 || index > scroll.NumberOfPanels))
            {
                Debug.LogError(
                    "<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + scroll.NumberOfPanels + ".",
                    scroll.gameObject);
                return null;
            }

            panel = Object.Instantiate(panel, Vector2.zero, Quaternion.identity, scroll.Content);
            panel.transform.SetSiblingIndex(index);

            if (scroll.Validate())
            {
                if (scroll.TargetPanel <= index)
                {
                    scroll.startingPanel = scroll.TargetPanel;                 
                }
                else
                {
                    scroll.startingPanel = scroll.TargetPanel + 1;
                }
                scroll.Setup(true);
            }

            return panel;
        }
    }
}