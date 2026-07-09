
using UdonSharp;
using UnityEngine;
using UnityEngine.UIElements;

public class SidePanelLeftController : UdonSharpBehaviour
{
    public const int statsPanelIndex = 0;
    public const int hierarchyPanelIndex = 1;

    public GameObject[] panels;
    public TMPro.TMP_Dropdown dropdown;

    bool ShowPanel(GameObject panel)
    {
        if (panel == null) return false;
        HideAllPanels();
        panel.SetActive(true);
        return true;
    }

    public bool ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return false;
        return ShowPanel(panels[index]);
    }

    void HideAllPanels()
    {
        int nPanels = panels.Length;
        for (int i = 0; i < nPanels; i++)
        {
            GameObject go = panels[i];
            if (go == null) continue;
            go.SetActive(false);
        }
    }

    public void DropDownElementSelected()
    {
        int selectedElement = dropdown.value;
        ShowPanel(selectedElement);
    }
}
