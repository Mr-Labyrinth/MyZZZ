using UnityEngine;

public class PartyManager
{
    private PartySaveData partySaveData;
    private PlayerModel[] playerModel;
    private int currentModelIndex;
    public PlayerModel CurrentModel => playerModel[currentModelIndex];

    public void Initialize()
    {
        partySaveData = SaveSystem.LoadParty();
    }

    public void switchToNext()
    {
        currentModelIndex++;
        currentModelIndex %= playerModel.Length;
    }

    public void switchToPrevious()
    {
        currentModelIndex--;
        if(currentModelIndex < 0)
        {
            currentModelIndex = playerModel.Length - 1;
        }
    }


}
