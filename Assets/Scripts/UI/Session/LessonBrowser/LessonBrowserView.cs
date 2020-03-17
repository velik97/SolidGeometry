using System;
using UI.MVVM;
using UnityEngine;

namespace UI.Session.LessonBrowser
{
    public class LessonBrowserView : View<LessonBrowserVM>
    {
        public override void Bind(LessonBrowserVM viewModel)
        {
            base.Bind(viewModel);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ViewModel.ChoseNext();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ViewModel.ChosePrevious();
            }
        }
    }
}