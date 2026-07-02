
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class TreeElementHandler : UdonSharpBehaviour
{
    public NodeTreeController nodeTreeController;
    public GameObject leaves;
    public TMPro.TMP_Text leafName;
    public Toggle toggleButton;
    public Image arrow;

    public GameObject representedObject;

    Color32 activeColor = new Color32(0x71, 0x37, 0xc8, 0xff);
    float activeZRotation = -90f;

    Color32 inactiveColor = new Color32(0x52, 0x52, 0x52, 0xff);
    float inactiveZRotation = 0f;

    public override void Interact()
    {
        if (gameObject.activeSelf && leaves != null)
        {
            
            leaves.SetActive(toggleButton.isOn);
            Vector3 angles = arrow.transform.localEulerAngles;
            angles.z = toggleButton.isOn ? activeZRotation : inactiveZRotation;
            arrow.transform.localEulerAngles = angles;  
        }
    }

    public void NameClicked()
    {
        if (gameObject.activeSelf && representedObject != null)
        {
            nodeTreeController.HandlerSelected(this);
        }
        else
        {
            Debug.Log($"NameClicked : {gameObject.activeSelf} - {representedObject != null}");
        }
    }

    public void SetSelected(bool isSelected)
    {
        Color32 color = isSelected ? activeColor : inactiveColor;
        leafName.color = color;
        arrow.color = color;
    }

    public void SetName(string name)
    {
        leafName.text = name;
        gameObject.name = name;
    }

    public void Display(GameObject go)
    {
        if (go == null) { Clear(); return; }

        gameObject.SetActive(true);
        Transform leavesTransform = leaves.transform;

        SetName(go.name);
        representedObject = go;

        Transform objectTransform = go.transform;
        int nObjectChildren = objectTransform.childCount;
        for (int c = 0; c < nObjectChildren; c++)
        {
            Transform child = objectTransform.GetChild(c);
            if (child == null) { continue; }
            GameObject childGo = child.gameObject;
            if (childGo == null) { continue; }
            GameObject treeLeaf = Instantiate(nodeTreeController.treeElementPrefab, leavesTransform);
            TreeElementHandler leafHandler = treeLeaf.GetComponent<TreeElementHandler>();
            leafHandler.nodeTreeController = nodeTreeController;
            leafHandler.Display(childGo);
        }
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        representedObject = null;
    }

    

}
