using System;

namespace ColorSorting3D.LevelEditor
{
    public enum MouseDownType
    {
        None = 0,
        Grid,
        Ring,
        Container,
    }

    public enum EventID
    {
        None = 0,
        AddContainerButtonClicked,
        AddBlueRingClicked,
        AddGreenRingClicked,
        AddOrangeRingClicked,
        AddPinkRingClicked,
        AddPurpleRingClicked,
        AddYellowRingClicked,
        KillAddingContainerProcess,
        CancelButtonClicked,
        KillRingProcess,
        ContainerSelected,
        ContainerUnselected,
        AddSizeButtonClicked,
        DeleteContainerButtonClicked,
        RingSelected,
        RingUnselected,
        DeleteRingButtonClicked,
        LoadButtonClicked,
        SaveButtonClicked,
        ClearButtonClicked,
        SetAvailableContainerButton,
        DecreaseSizeContainerButton,
    }
}