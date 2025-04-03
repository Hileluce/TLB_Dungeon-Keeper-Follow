using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Behaiver : MonoBehaviour
{
    EventSystem cEvS;
    public static UI_Behaiver S;
    public GameObject highPanel;
    public GameObject selectedButtonOnEnable;
    public GameObject dropDownPanel;
    public GameObject selectedButtonOnDropMenu;

    //array of buttons with any
    public GameObject[] inventoryCell;
    public Image[] inventoryImages;
    //public any [] for IGadgets mayby
    public IGadget[] invGadgetsLinks;
    GameObject tempGOforGadget;
    public Inventory inventory;

    void Awake()
    {
        if (S == null) S = this;
        cEvS = EventSystem.current;
        inventoryImages = new Image[inventoryCell.Length];
        for(int i = 0; i < inventoryCell.Length; i++)
        {
            inventoryImages[i] = inventoryCell[i].GetComponent<Image>();
            //make button unselectable
            inventoryCell[i].GetComponent<Button>().enabled = false;
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && GameMenu.isGamePaused)
        {
            ReturnHighPanel();
        }
        
    }
    private void OnEnable()
    {
        ReturnHighPanel();
    }
    public void ActivateMenu(GameObject go)
    {
        highPanel.SetActive(false);
        cEvS.SetSelectedGameObject(go);
    }
    public void ReturnHighPanel()
    {
        highPanel.SetActive(true);
        cEvS.SetSelectedGameObject(selectedButtonOnEnable);
        dropDownPanel.SetActive(false);
        //spriteOfIcon = null
        // disable/enable button for picking

    }
    public void DropDownMenu(GameObject go)
    {
        tempGOforGadget = go;
        dropDownPanel.SetActive(true);
        dropDownPanel.transform.position = go.transform.position;
        //spriteOfIcon
        cEvS.SetSelectedGameObject(selectedButtonOnDropMenu);
    }
    public void SetCurrentGadget()
    {
        if (tempGOforGadget == null) Debug.LogError("tempGOgadget not defined");
        IGadget gadg = tempGOforGadget.GetComponent<ItemButton>().GiveIGadget();
        if (gadg == null) return;
        inventory.SetGadget(gadg);
        tempGOforGadget = null;
    }
    public void SetSecondGadget()
    {
        if (tempGOforGadget == null) Debug.LogError("tempGOgadget not defined");
        IGadget gadg = tempGOforGadget.GetComponent<ItemButton>().GiveIGadget();
        if (gadg == null) return;
        inventory.SetSecondGadget(gadg);
        tempGOforGadget = null;
    }
    public static void InstallGadgetInCell(GameObject go)
    {
        foreach(Image image in S.inventoryImages)
        {
            if(image.sprite == null)
            {
                image.sprite = go.GetComponent<IGadget>().gadgetSprite;
                image.gameObject.GetComponent<ItemButton>().SetGadget(go);
                //TO DO set button selectable
                image.gameObject.GetComponent<Button>().enabled = true;
                return;
            }
        }
    }
}
