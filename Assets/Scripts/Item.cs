// The Item struct only contains the dynamic item properties and a name, so that
// the static properties can be read from the scriptable object.
//
// Items have to be structs in order to work with SyncLists.
//
// The player inventory actually needs Item slots that can sometimes be empty
// and sometimes contain an Item. The obvious way to do this would be a
// InventorySlot class that can store an Item, but SyncLists only work with
// structs - so the Item struct needs an option to be _empty_ to act like a
// slot. The simple solution to it is the _valid_ property in the Item struct.
// If valid is false then this Item is to be considered empty.
//
// _Note: the alternative is to have a list of Slots that can contain Items and
// to serialize them manually in OnSerialize and OnDeserialize, but that would
// be a whole lot of work and the workaround with the valid property is much
// simpler._
//
// Items can be compared with their name property, two items are the same type
// if their names are equal.
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct Item {
    // name used to reference the database entry (cant save template directly
    // because synclist only support simple types)
    public string name;

    // dynamic stats (cooldowns etc. later)
    public bool valid; // acts as slot. false means there is no item in here.
    public int amount;

    // constructors
    public Item(ItemTemplate template, int _amount=1) {
        name = template.name;
        amount = _amount;
        valid = true;
    }

    // does the template still exist?
    public bool TemplateExists() {
        return name != null && ItemTemplate.dict.ContainsKey(name);
    }

    // database item property access
    public string category {
        get { return ItemTemplate.dict[name].category; }
    }
    public int maxStack {
        get { return ItemTemplate.dict[name].maxStack; }
    }
    public int buyPrice {
        get { return ItemTemplate.dict[name].buyPrice; }
    }
    public int sellPrice {
        get { return ItemTemplate.dict[name].sellPrice; }
    }
    public int minLevel {
        get { return ItemTemplate.dict[name].minLevel; }
    }
    public bool sellable {
        get { return ItemTemplate.dict[name].sellable; }
    }
    public bool destroyable {
        get { return ItemTemplate.dict[name].destroyable; }
    }
    public string description {
        get { return ItemTemplate.dict[name].description; }
    }
    public Sprite image {
        get { return ItemTemplate.dict[name].image; }
    }
    public bool usageDestroy {
        get { return ItemTemplate.dict[name].usageDestroy; }
    }
    public int usageHp {
        get { return ItemTemplate.dict[name].usageHp; }
    }
    public int usageMp {
        get { return ItemTemplate.dict[name].usageMp; }
    }
    public int usageExp {
        get { return ItemTemplate.dict[name].usageExp; }
    }
    public int equipHpBonus {
        get { return ItemTemplate.dict[name].equipHpBonus; }
    }
    public int equipMpBonus {
        get { return ItemTemplate.dict[name].equipMpBonus; }
    }
    public int equipDamageBonus {
        get { return ItemTemplate.dict[name].equipDamageBonus; }
    }
    public int equipDefenseBonus {
        get { return ItemTemplate.dict[name].equipDefenseBonus; }
    }
    public GameObject model {
        get { return ItemTemplate.dict[name].model; }
    }

    // tooltip
    public string Tooltip(bool showBuyPrice = false, bool showSellPrice = false) {
        var tip = ItemTemplate.dict[name].Tooltip(showBuyPrice, showSellPrice);

        // amount
        if (amount > 1) tip += "Amount: " + amount + "\n";

        return tip;
    }
}

public class SyncListItem : SyncListStruct<Item> { }
