using Data;
using GameMenu.Inventory.ItemLists;
using GameMenu.Inventory;
using GameMenu.Potions;
using UnityEngine;
using UnityEngine.EventSystems;
using Universal;
using UnityEngine.UI;
using GameMenu.Artifacts;

public class ShowArtifactPrefab : ShowObject, IPointerClickHandler, ItemList.IListUpdater
{
    #region fields
    private int listPosition;
    [SerializeField] private TextOutline textOutline;
    [SerializeField] private Text mainText;
    public GameObject rootObject => gameObject;
    public int listParam => listPosition;
    #endregion fields

    #region methods
    public void UpdateValues(int deskPosition)
    {
        ArtifactData artifact = GameDataInit.deskArtifact(deskPosition);
        mainText.text = TextOutline.languageData.artifactsNameData[artifact.id];
        textOutline.SetAll();
        listPosition = artifact.listPosition;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        RemoveFromDesk();
    }
    public override GameObject SpawnObject()
    {
        ArtifactInit newObject = base.SpawnObject().GetComponent<ArtifactInit>();
        newObject.UpdateValues(listPosition);
        newObject.gameObject.SetActive(true);
        return newObject.gameObject;
    }
    private void RemoveFromDesk()
    {
        GameDataInit.RemoveArtifactFromDesk(listPosition);
        ItemList rightIL = InventoryPanelInit.instance.inventoryDeskArtifacts;
        ItemList centerIL = InventoryPanelInit.instance.inventoryArtifactsCenter;
        FindObjectOfType<InventoryArtifactsList>().GetUpdatedObject(listPosition, out ItemList.IListUpdater listUpdater);
        centerIL.Add(listUpdater, true, true);
        rightIL.RemoveAtListParam(listPosition, true, true);
        DestroyChild();
        GameDataInit.instance.OnArtifactEffectsChanged?.Invoke();
    }

    public void OnListUpdate(int param) => UpdateValues(param);
    #endregion methods
}
