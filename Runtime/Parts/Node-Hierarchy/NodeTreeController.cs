
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NodeTreeController : UdonSharpBehaviour
{
    public MainUIController mainUIController;

    public RectTransform treeLocation;

    TreeElementHandler previousHandler;
    public GameObject treeElementPrefab;

    public void HandlerSelected(TreeElementHandler handler)
    {
        if (handler == null || handler == previousHandler || handler.representedObject == null) { return; }

        if (previousHandler != null)
        {
            previousHandler.SetSelected(false);
        }
        previousHandler = handler;
        handler.SetSelected(true);

        mainUIController.NodeSelected(handler.representedObject);
    }

    public void ShowHierarchy(GameObject go)
    {
        if (go == null) { return; }
        Clear();

        GameObject tree = Instantiate(treeElementPrefab, treeLocation);
        TreeElementHandler handler = tree.GetComponent<TreeElementHandler>();
        handler.nodeTreeController = this;
        handler.Display(go);
    }

    public void Clear()
    {
        previousHandler = null;
        int nChildren = treeLocation.childCount;
        for (int c = nChildren - 1; c >= 0; c--)
        {
            Transform childT = treeLocation.GetChild(c);
            if (childT == null) { continue; }
            GameObject childGo = childT.gameObject;
            if (childGo  == null) { continue; }
            Destroy(childGo);
        }
    }
}
