using System.Collections.Generic;
using UI.MVVM;
using UniRx;
using UnityEngine;

namespace UI.MainMenu.FileSystem
{
    public class AssetListView : View<AssetListVM>
    {
        [SerializeField]
        private RectTransform m_Container;
        
        [SerializeField]
        private AssetListElementView m_ElementViewPrefab;

        private List<AssetListElementView> m_AssetElementViews;

        public override void Bind(AssetListVM viewModel)
        {
            base.Bind(viewModel);

            m_AssetElementViews = new List<AssetListElementView>();
            AddDisposable(ViewModel.OnListUpdated.Subscribe(_ => UpdateList()));
            UpdateList();
        }

        private void UpdateList()
        {
            foreach (AssetListElementView elementView in m_AssetElementViews)
            {
                Destroy(elementView.gameObject);
            }
            m_AssetElementViews.Clear();

            foreach (AssetListElementVM viewModelElementVM in ViewModel.ElementVMs)
            {
                AssetListElementView elementView = Instantiate(m_ElementViewPrefab, m_Container, false);
                m_AssetElementViews.Add(elementView);
                elementView.Bind(viewModelElementVM);
            }
        }
    }
}