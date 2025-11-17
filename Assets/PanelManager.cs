using UnityEngine;

namespace Assets
{
    public class PanelManager : MonoBehaviour
    {
        [System.Serializable]
        public class PanelCategory
        {
            public string name;
            public GameObject panel;
        }

        public PanelCategory[] categories;

        private GameObject currentPanel;

        void Start()
        {
            foreach (var c in categories)
            {
                if (c.panel != null)
                    c.panel.SetActive(false);
            }
            ShowPanel(categories[0].name);
        }
        public void ShowPanel(string categoryName)
        {
            if (currentPanel != null)
                currentPanel.SetActive(false);

            foreach (var c in categories)
            {
                if (c.name == categoryName)
                {
                    c.panel.SetActive(true);
                    currentPanel = c.panel;
                    return;
                }
            }

        }
    }
}